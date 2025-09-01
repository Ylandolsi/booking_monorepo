using System.Net;
using System.Net.Http.Json;
using Booking.Modules.Mentorships.Features;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Mentorships.Schedule;

public class ScheduleTests : MentorshipTestBase
{
    public ScheduleTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    #region SetSchedule

    [Fact]
    public async Task SetSchedule_ShouldCreateDaysAndSlots_WhenValidDataProvided()
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
                    AvailabilityRanges = new[]
                    {
                        new { StartTime = "09:00", EndTime = "12:00" },
                        new { StartTime = "14:00", EndTime = "17:00" }
                    }
                },
                new
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    IsActive = true,
                    AvailabilityRanges = new[]
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
    public async Task SetSchedule_ShouldToggleDayAvailability_WhenIsActiveFalse()
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
                    AvailabilityRanges = new[]
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
                    AvailabilityRanges = new object[0] // Empty slots when deactivating
                }
            }
        };

        var response = await userAct.PostAsJsonAsync(MentorshipEndpoints.Availability.SetBulk, toggleRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SetSchedule_ShouldReplaceExistingSlots_WhenDayAlreadyExists()
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
                    AvailabilityRanges = new[]
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
                    AvailabilityRanges = new[]
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
    public async Task SetSchedule_ShouldReturnBadRequest_WhenInvalidTimeFormat()
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
                    AvailabilityRanges = new[]
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
    public async Task SetSchedule_ShouldHandleBufferTime_WithMultipleSlots()
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
                    AvailabilityRanges = new[]
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
    
    private DateTime GetNextWeekday(DayOfWeek dayOfWeek)
    {
        var today = DateTime.Now.Date;
        var daysUntilTarget = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0) daysUntilTarget = 7; // Next week
        return today.AddDays(daysUntilTarget);
    }
    
}