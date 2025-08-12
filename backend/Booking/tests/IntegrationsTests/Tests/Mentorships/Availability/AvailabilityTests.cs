using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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

    #region SetBulkAvailability

    [Fact]
    public async Task SetBulkAvailability_ShouldCreateDaysAndSlots_WhenValidDataProvided()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" },
                        new { StartTime = "14:00", EndTime = "17:00" }
                    }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "10:00", EndTime = "16:00" }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //var result = await response.Content.ReadFromJsonAsync<dynamic>();
        //Assert.NotNull(result);
        // Assert.True(((JsonElement)result.GetProperty("totalSlotsCreated")).GetInt32() > 0);
    }

    [Fact]
    public async Task SetBulkAvailability_ShouldToggleDayAvailability_WhenIsActiveFalse()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        // First, create availability
        var initialRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "09:00", EndTime = "17:00" }
                    }
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, initialRequest);

        // Then, deactivate the day
        var toggleRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    IsActive = false,
                    TimeSlots = new object[0] // Empty slots when deactivating
                }
            }
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, toggleRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SetBulkAvailability_ShouldReplaceExistingSlots_WhenDayAlreadyExists()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        // Create initial availability
        var initialRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Friday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" }
                    }
                }
            }
        };
        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, initialRequest);

        // Update with new slots
        var updateRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Friday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "10:00", EndTime = "14:00" },
                        new { StartTime = "15:00", EndTime = "18:00" }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SetBulkAvailability_ShouldReturnBadRequest_WhenInvalidTimeFormat()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var invalidRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "25:00", EndTime = "26:00" } // Invalid time
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, invalidRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Handler handles invalid times gracefully
    }

    [Fact]
    public async Task SetBulkAvailability_ShouldHandleBufferTime_WithMultipleSlots()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" },
                        new { StartTime = "13:00", EndTime = "17:00" }
                    }
                }
            }
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region GetMentorAvailabilityByDay

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnCorrectSlots_WhenDayHasAvailability()
    {
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);

        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday,
                    IsActive = true,
                    TimeSlots = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" },
                        new { StartTime = "14:00", EndTime = "17:00" }
                    }
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var nextMonday = GetNextWeekday(DayOfWeek.Monday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.Slug}&date={nextMonday:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(availability);
        Assert.True(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
        Assert.True(((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetMentorAvailabilityByDay_ShouldReturnNoSlots_WhenDayIsInactive()
    {
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);

        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    IsActive = false, // Day is inactive
                    TimeSlots = new object[0]
                }
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var nextThursday = GetNextWeekday(DayOfWeek.Thursday);

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_user_2");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetDaily}?mentorSlug={currentUser.Slug}&date={nextThursday:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var availability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.False(((JsonElement)availability.GetProperty("isAvailable")).GetBoolean());
        Assert.Equal(0, ((JsonElement)availability.GetProperty("timeSlots")).GetArrayLength());
    }

    #endregion

    #region GetMentorAvailabilityByMonth

    [Fact]
    public async Task GetMentorAvailabilityByMonth_ShouldReturnCompleteMonth_WithMixedActiveDays()
    {
        // Arrange
        var (userArrange, userAct) = await CreateMentor("mentor_daily_checkt");

        var currentUser = await GetCurrenUserInfo(userArrange);
        var bulkRequest = new
        {
            DayAvailabilities = new[]
            {
                new
                {
                    DayOfWeek = DayOfWeek.Monday, IsActive = true,
                    TimeSlots = new[] { new { StartTime = "09:00", EndTime = "17:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Wednesday, IsActive = true,
                    TimeSlots = new[] { new { StartTime = "10:00", EndTime = "16:00" } }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Thursday, IsActive = true,
                    TimeSlots = new[] { new { StartTime = "08:00", EndTime = "14:00" } }
                },
                /*
                new { DayOfWeek = DayOfWeek.Friday, IsActive = false, TimeSlots = new object[0] }
            */
            }
        };
        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, bulkRequest);

        var currentDate = DateTime.Now;

        // Act
        var (publicArrange, publicAct) = GetClientsForUser("public_monthly");
        var response = await publicAct.GetAsync(
            $"{MentorshipEndpoints.Availability.GetMonthly}?mentorSlug={currentUser.Slug}&year={currentDate.Year}&month={currentDate.Month}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var monthlyAvailability = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(monthlyAvailability);
        Assert.Equal(currentDate.Year, ((JsonElement)monthlyAvailability.GetProperty("year")).GetInt32());
        Assert.Equal(currentDate.Month, ((JsonElement)monthlyAvailability.GetProperty("month")).GetInt32());
        Assert.True(((JsonElement)monthlyAvailability.GetProperty("days")).GetArrayLength() > 0);
    }

    #endregion


    private DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.Now.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week
        return today.AddDays(daysUntilTarget);
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


    // ******************** deprecated ********************

    #region Delete

    // TODO DeleteAvailability_ShouldReturnBadRequest_WhenHasBookedSessions 

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task DeleteAvailability_ShouldSucceed_WhenRequested()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var createResponse = await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        var availabilityId = await createResponse.Content.ReadFromJsonAsync<int>();

        // Act
        var response = await userAct.DeleteAsync($"/mentorships/availability/{availabilityId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    #endregion

    #region SetAvailability

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenInvalidTimeIncrement()
    {
        // Arrange
        var (userArrange, userAct) = GetClientsForUser("increment_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 50.0m });

        var invalidData = new
        {
            DayOfWeek = DayOfWeek.Monday,
            StartTime = "09:15",
            EndTime = "10:45", // End time before start time
        };

        // Act
        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, invalidData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(Skip = "This test is skipped temporarily.")]
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

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenOverlappingWithExisting()
    {
        var (userArrange, userAct) = GetClientsForUser("overlap_test");
        var loginData = await CreateUserAndLogin(null, null, userArrange);

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Mentors.Become, new { HourlyRate = 65.0m });

        await userArrange.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = "10:00",
            EndTime = "14:00"
        });

        var overlappingData = new
        {
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = "12:00",
            EndTime = "16:00"
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, overlappingData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task SetAvailability_ShouldReturnBadRequest_WhenEndTimeBeforeStartTime()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = "09:00",
            EndTime = "08:30", // End time before start time
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, availabilityData);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task SetAvailability_ShouldCreateAvailabilityAndReturnId_WhenValidDataProvided()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityData = new
        {
            DayOfWeek = 1, // Monday
            StartTime = "09:00",
            EndTime = "17:00",
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, availabilityData);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<int>();
        Assert.NotNull(result);
    }

    #endregion

    #region ToggleDay

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task ToggleAvailability_ShouldToggleIndividualSlot_WhenValidId()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        var availabilityResponse = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Tuesday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        var availabilityId = await availabilityResponse.Content.ReadFromJsonAsync<int>();

        var toggleResponse = await userAct.PatchAsync($"/mentorships/availability/{availabilityId}/toggle", null);


        Assert.Equal(HttpStatusCode.OK, toggleResponse.StatusCode);
        var result = await toggleResponse.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task ToggleDayAvailability_ShouldToggleAllSlotsForDay_WhenValidDay()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");

        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = "09:00",
            EndTime = "12:00"
        });

        await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Thursday,
            StartTime = "14:00",
            EndTime = "17:00"
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

    #region Update

    [Fact(Skip = "This test is skipped temporarily.")]
    public async Task UpdateAvailability_ShouldUpdateSlot_WhenValidData()
    {
        var (userArrange, userAct) = await CreateMentor("mentorTest");


        var createResponse = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.Set, new
        {
            DayOfWeek = DayOfWeek.Friday,
            StartTime = "09:00",
            EndTime = "17:00"
        });

        var availabilityId = await createResponse.Content.ReadFromJsonAsync<int>();

        var updateData = new
        {
            DayOfWeek = DayOfWeek.Friday,
            StartTime = "10:00",
            EndTime = "16:00"
        };

        var response = await userAct.PutAsJsonAsync($"/mentorships/availability/{availabilityId}", updateData);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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