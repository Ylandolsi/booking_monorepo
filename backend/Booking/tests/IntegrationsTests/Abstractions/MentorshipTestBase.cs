using System.Net.Http.Json;
using System.Text.Json;
using Booking.Modules.Mentorships.Features;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTests.Abstractions;

public abstract class MentorshipTestBase : AuthenticationTestBase
{
    protected readonly Guid _mentorUserId = Guid.NewGuid();
    protected readonly string _mentorUserEmail = "mentor@example.com";
    protected readonly Guid _menteeUserId = Guid.NewGuid();
    protected readonly string _menteeUserEmail = "mentee@example.com";

    protected MentorshipTestBase(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Creates an authenticated HTTP client specifically for a mentor user
    /// Similar to Factory.CreateAuthenticatedClient pattern
    /// Authentication is handled via cookies after login
    /// </summary>
    protected async Task<(HttpClient client, LoginResponse data)> CreateAuthenticatedMentorClient(
        string? email = null,
        decimal hourlyRate = 75.0m) 
    {
        email ??= Fake.Internet.Email();
        var password = "MentorPassword123!";
        
        await RegisterAndVerifyUser(email, password);
        var loginData = await LoginUser(email, password);
        
        var becomeMentorPayload = new
        {
            HourlyRate = hourlyRate,
        };

        var response = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);
        response.EnsureSuccessStatusCode();
        
        return (_client, loginData);
    }

    /// <summary>
    /// Creates an authenticated HTTP client specifically for a mentee user
    /// Similar to Factory.CreateAuthenticatedClient pattern
    /// Authentication is handled via cookies after login
    /// </summary>
    protected async Task<(HttpClient client, LoginResponse data)> CreateAuthenticatedMenteeClient(string? email = null)
    {
        email ??= Fake.Internet.Email();
        var password = "MenteePassword123!";
        
        await RegisterAndVerifyUser(email, password);
        var loginData = await LoginUser(email, password);
        
        // After login, _client has authentication cookies
        return (_client, loginData);
    }

   

    protected static class MentorshipTestData
    {
        public static class Availability
        {
            public static readonly (DayOfWeek day, TimeOnly start, TimeOnly end)[] WeekdayAvailability = 
            {
                (DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
                (DayOfWeek.Tuesday, new TimeOnly(10, 0), new TimeOnly(16, 0)),
                (DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(17, 0)),
                (DayOfWeek.Thursday, new TimeOnly(10, 0), new TimeOnly(16, 0)),
                (DayOfWeek.Friday, new TimeOnly(9, 0), new TimeOnly(15, 0))
            };

            public static readonly (DayOfWeek day, TimeOnly start, TimeOnly end)[] WeekendAvailability = 
            {
                (DayOfWeek.Saturday, new TimeOnly(10, 0), new TimeOnly(14, 0)),
                (DayOfWeek.Sunday, new TimeOnly(12, 0), new TimeOnly(16, 0))
            };

            public static readonly (DayOfWeek day, TimeOnly start, TimeOnly end)[] FullWeekAvailability = 
                WeekdayAvailability.Concat(WeekendAvailability).ToArray();
        }

        public static class Sessions
        {
            public static DateTime NextWeekMonday => DateTime.UtcNow.AddDays(7 - (int)DateTime.UtcNow.DayOfWeek + 1);
            public static DateTime NextWeekTuesday => NextWeekMonday.AddDays(1);
            public static DateTime NextWeekWednesday => NextWeekMonday.AddDays(2);
            public static DateTime NextWeekThursday => NextWeekMonday.AddDays(3);
            public static DateTime NextWeekFriday => NextWeekMonday.AddDays(4);
            public static DateTime NextWeekSaturday => NextWeekMonday.AddDays(5);
            public static DateTime NextWeekSunday => NextWeekMonday.AddDays(6);

            public static readonly int[] ValidDurations = { 30, 45, 60, 90, 120 };
            public static readonly int[] InvalidDurations = { 0, 15, 10, 300, -30 };

            public static string[] SampleSessionNotes = 
            {
                "Looking forward to learning clean architecture patterns",
                "Need help with microservices design",
                "Want to improve my coding interview skills",
                "Interested in learning about system design",
                "Need guidance on career progression in tech"
            };
        }

        public static class Mentors
        {
            public static readonly (decimal rate, string bio)[] SampleMentorProfiles = 
            {
                (50.0m, "Senior Software Engineer with 8+ years in web development"),
                (75.0m, "Tech Lead with expertise in cloud architecture and team management"),
                (100.0m, "Principal Engineer with 12+ years in distributed systems"),
                (125.0m, "Engineering Manager with experience in scaling teams and products"),
                (150.0m, "CTO with deep knowledge in startup technology strategy")
            };

            public static readonly string[] SampleBios = 
            {
                "Experienced full-stack developer specializing in .NET and React",
                "Cloud architect with expertise in Azure and AWS",
                "DevOps engineer passionate about CI/CD and infrastructure automation",
                "Mobile developer with 5+ years in iOS and Android development",
                "Data engineer with expertise in big data and machine learning"
            };
        }

        public static class Reviews
        {
            public static readonly (int rating, string comment)[] SampleReviews = 
            {
                (5, "Excellent mentor! Very knowledgeable and patient."),
                (4, "Great session, learned a lot about system design."),
                (5, "Amazing experience, highly recommend!"),
                (4, "Very helpful, good explanations of complex topics."),
                (5, "Perfect mentor for career guidance and technical skills.")
            };
        }

        public static class MentorshipRelationships
        {
            public static readonly string[] SampleRequestMessages = 
            {
                "I would love to learn from your expertise in software architecture",
                "Your background in tech leadership aligns perfectly with my career goals",
                "I'm interested in your guidance on transitioning to senior roles",
                "Would appreciate mentorship on building scalable systems",
                "Seeking guidance on technical decision making and best practices"
            };
        }
    }

    #region Legacy Methods (kept for backward compatibility)
    protected async Task<(LoginResponse mentorData, HttpClient mentorClient)> CreateMentorAndLogin(
        decimal hourlyRate = 50.0m, 
        string bio = "Experienced software developer")
    {
        // Create and login mentor user
        var mentorEmail = Fake.Internet.Email();
        var mentorPassword = "MentorPassword123!";
        await RegisterAndVerifyUser(mentorEmail, mentorPassword);
        
        var mentorData = await LoginUser(mentorEmail, mentorPassword);
        
        // After login, _client has authentication cookies
        // Become a mentor
        var becomeMentorPayload = new
        {
            HourlyRate = hourlyRate,
            Bio = bio
        };

        var becomeMentorResponse = await _client.PostAsJsonAsync(MentorshipsEndpoints.BecomeMentor, becomeMentorPayload);
        becomeMentorResponse.EnsureSuccessStatusCode();

        return (mentorData, _client);
    }

    protected async Task<(LoginResponse menteeData, HttpClient menteeClient)> CreateMenteeAndLogin()
    {
        // Create and login mentee user  
        var menteeEmail = Fake.Internet.Email();
        var menteePassword = "MenteePassword123!";
        await RegisterAndVerifyUser(menteeEmail, menteePassword);
        
        var menteeData = await LoginUser(menteeEmail, menteePassword);
        
        // After login, _client has authentication cookies
        return (menteeData, _client);
    }
    #endregion
}
