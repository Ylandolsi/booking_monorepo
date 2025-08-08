using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Authentication.Me;
using IntegrationsTests.Abstractions;

namespace IntegrationsTests.Tests.Mentorships.Availability;

public class AvailabilityTests : MentorshipTestBase
{
    public AvailabilityTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region Get


    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldFilterPastDays_WhenIncludePastDaysIsFalse()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentorTest");
        
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
            {
                DayOfWeek = day,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(12, 0)
            });
            
            await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
            {
                DayOfWeek = day,
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(17, 0)
            });
        }
        
        
        var getMyInfo = await userArrange.GetAsync(UsersEndpoints.GetCurrentUser);
        var info = await getMyInfo.Content.ReadFromJsonAsync<MeData>();

        var mentorSlug =  info.Slug;
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        var response =
            await userAct.GetAsync(
                $"{MentorshipEndpoints.Availability.GetMonthly}?mentorSlug={mentorSlug}&year={currentYear}&month={currentMonth}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<MonthlyAvailabilityResponse>();
        Assert.NotNull(availability);

        // Verify no past days are included
        var pastDays = availability.Days.Where(d => d.Date.Date < DateTime.Now.Date).ToList();
        Assert.Empty(pastDays);
    }
    
    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldFilterPastDays_WhenIncludePastDaysIsFalse()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentorTest");
        
            await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
            {
                DayOfWeek = 1,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(12, 0)
            });
            
            await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
            {
                DayOfWeek = 1,
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(17, 0)
            });
        
        
        var getMyInfo = await userArrange.GetAsync(UsersEndpoints.GetCurrentUser);
        var info = await getMyInfo.Content.ReadFromJsonAsync<MeData>();

        var mentorSlug =  info.Slug;
        var response =
            await userAct.GetAsync(
                $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={mentorSlug}&date={2025}-{08}-{04}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(availability);
        
    }

    
    #endregion


    [Fact]
    public async Task UpdateAvailability_ShouldUpdateSlot_WhenValidData()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        var createResponse = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Friday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        });

        var availabilityId = await createResponse.Content.ReadFromJsonAsync<int>();

        var updateData = new
        {
            DayOfWeek = DayOfWeek.Friday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(16, 0)
        };

        var response = await userAct.PutAsJsonAsync($"/mentorships/availability/{availabilityId}", updateData);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    /*
    [Fact]
    public async Task Availability_ShouldHandleTimeZoneCorrectly_WhenQueryingByDate()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Sunday,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(20, 0)
        });

        var nextSunday = GetNextSunday();

        // Act
        var response = await userAct.GetAsync(
            $"/mentorships/mentors/{loginData.UserSlug}/availability/day?date={nextSunday:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<DailyAvailabilityResponse>();
        Assert.NotNull(availability);
        Assert.Equal(nextSunday.Date, availability.Date.Date);
        Assert.True(availability.IsAvailable);
    }
    */

    #region Delete

    [Fact]
    public async Task DeleteAvailability_ShouldReturnBadRequest_WhenHasBookedSessions()
    {
        // Arrange
        var (mentorArrange, mentorAct) = GetClientsForUser("delete_booked_test");
        var mentorLogin = await CreateUserAndLogin(null, null, mentorArrange);

        await mentorAct.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 80.0m });

        var createResponse = await mentorAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        });

        var createResult = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        int availabilityId = createResult.availabilityId;

        // Create mentee and book a session
        var (menteeArrange, menteeAct) = GetClientsForUser("delete_booked_mentee");
        await CreateUserAndLogin(null, null, menteeArrange);

        var nextMonday = GetNextMonday();
        await menteeAct.PostAsJsonAsync(MentorshipEndpoints.Sessions.Book, new
        {
            MentorId = 1, // This would be the actual mentor ID
            StartDateTime = nextMonday.AddHours(10),
            DurationMinutes = 60,
            Note = "Test session"
        });

        // Act
        var response = await mentorAct.DeleteAsync($"/mentorships/availability/{availabilityId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAvailability_ShouldRemoveSlot_WhenNoBookedSessions()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("delete_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userAct.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 55.0m });

        var createResponse = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(14, 0)
        });

        var createResult = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        int availabilityId = createResult.availabilityId;

        // Act
        var response = await userAct.DeleteAsync($"/mentorships/availability/{availabilityId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    #endregion

    #region SetAvailability

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenInvalidTimeIncrement()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("increment_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 50.0m });

        var invalidData = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(9, 15), // Not 30-minute increment
            EndTime = new TimeOnly(10, 45) // Not 30-minute increment
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, invalidData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenDurationDifferentThan30()
    {
        var (userArrange, userAct) = GetClientsForUser("short_duration_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 60.0m });

        var shortDurationData = new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(10, 15) // Only 15 minutes
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, shortDurationData);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenOverlappingWithExisting()
    {
        var (userArrange, userAct) = GetClientsForUser("overlap_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 65.0m });

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(14, 0)
        });

        var overlappingData = new
        {
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = new TimeOnly(12, 0),
            EndTime = new TimeOnly(16, 0)
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, overlappingData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenEndTimeBeforeStartTime()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(8, 30) // End time before start time
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, availabilityData);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetAvailability_ShouldCreateAvailabilityAndReturnId_WhenValidDataProvided()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, availabilityData);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<int>();
        Assert.NotNull(result);
    }

    #endregion

    #region SetBulk

    [Fact]
    public async Task SetBulkAvailability_ShouldCreateMultipleAvailabilities_WhenValidDataProvided_()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");
        var bulkAvailabilityData = new
        {
            Availabilities = new[]
            {
                new
                {
                    DayOfWeek = 1, // Monday
                    TimeSlots = new[]
                    {
                        new { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0) },
                        new { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(17, 0) }
                    }
                },
                new
                {
                    DayOfWeek = 2, // Tuesday
                    TimeSlots = new[]
                    {
                        new { StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) }
                    }
                }
            }
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkAvailabilityData);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<List<int>>();
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // 2 slots for Monday + 1 slot for Tuesday
    }

    #endregion

    #region ToggleDay

    [Fact]
    public async Task ToggleAvailability_ShouldToggleIndividualSlot_WhenValidId()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityResponse = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0)
        });

        var availabilityId = await availabilityResponse.Content.ReadFromJsonAsync<int>();

        var toggleResponse = await userAct.PatchAsync($"/mentorships/availability/{availabilityId}/toggle", null);


        Assert.Equal(HttpStatusCode.OK, toggleResponse.StatusCode);
        var result = await toggleResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ToggleDayAvailability_ShouldToggleAllSlotsForDay_WhenValidDay()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        });

        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(17, 0)
        });

        // Act
        var response =
            await userAct.PatchAsync($"/mentorships/availability/day/toggle?dayOfWeek={DayOfWeek.Thursday}", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    #endregion

    private DateTime GetNextMonday()
    {
        var today = DateTime.Now.Date;
        var daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilMonday == 0) daysUntilMonday = 7;
        return today.AddDays(daysUntilMonday);
    }

    private DateTime GetNextSunday()
    {
        var today = DateTime.Now.Date;
        var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilSunday == 0) daysUntilSunday = 7;
        return today.AddDays(daysUntilSunday);
    }
}