using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Booking.Common.Authentication;
using Booking.Common.Email;
using Booking.Common.Options;
using Booking.Modules.Mentorships.Features.Payment;
using Booking.Modules.Mentorships.Options;
using Booking.Modules.Users;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.AspNetCore.Identity;

namespace Booking.Api.Services;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddCors()
            .AddOptions(configuration)
            .AddServices(configuration)
            .AddCache(configuration)
            //.AddResielenecPipelines(configuration)
            .AddAWS(configuration)
            .AddIdentityCore()
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHttpClient();
        /*// TODO : move clients name to static !*/
        var konnectOptions = configuration.GetSection(KonnectOptions.OptionsKey)?.Get<KonnectOptions>() ??
                             throw new InvalidOperationException("konnect options are not configured.");

        services.AddHttpClient("KonnectClient", client =>
            {
                // TODO : make this more reselllient
                client.BaseAddress = new Uri(konnectOptions.ApiUrl);
                client.DefaultRequestHeaders.Add("x-api-key",konnectOptions.ApiUrl);
                // TOdo : Restore it for prod client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddStandardResilienceHandler();


        services.AddScoped<KonnectService>();


        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultCors", builder =>
            {
                builder.WithOrigins("http://localhost:3000",
                        "http://localhost:3001",
                        "http://localhost:5000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                // TODO 
                // For production 
                // .WithOrigins("https://yourproductiondomain.com")

                // If you need to expose custom headers
                // .WithExposedHeaders("access_token", "refresh_token");
            });
        });
        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration config)
    {
        services.AddMemoryCache();
        services.AddFusionCache();


        /*
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Cache") ??
                                    throw new InvalidOperationException(
                                        "Redis connection string is not configured.");
        });*/
        return services;
    }


    public static IServiceCollection AddIdentityCore(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<int>>(o =>
                {
                    o.User.RequireUniqueEmail = true;
                    o.SignIn.RequireConfirmedAccount = true;
                    o.Password.RequiredLength = 8;
                    o.Password.RequireDigit = true;
                    o.Password.RequireUppercase = true;
                    o.Password.RequireNonAlphanumeric = false;

                    // Configure lockout settings
                    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    o.Lockout.MaxFailedAccessAttempts = 5;
                    o.Lockout.AllowedForNewUsers = true;

                    // configure token providers
                    o.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                }
            )
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        return services;
    }


    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrEmpty(connectionString))
        {
            // Consider logging this or throwing a more specific exception
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString);

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JwtOptionsKey));
        services.Configure<GoogleOAuthOptions>(configuration.GetSection(GoogleOAuthOptions.GoogleOptionsKey));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.EmailOptionsKey));
        services.Configure<FrontendApplicationOptions>(
            configuration.GetSection(FrontendApplicationOptions.FrontEndOptionsKey));
        services.Configure<KonnectOptions>(configuration.GetSection(KonnectOptions.OptionsKey));
        // In your test setup
        /*services.Configure<KonnectOptions>(options =>
        {
            options.ApiUrl = "http://127.0.0.1:8080";
            options.ApiKey = "mock-api-key";
            options.WalletKey = "mock-receiver-wallet";
            options.Webhook = "http://localhost:5000/api/mentorships/payment/webhook";
            options.PayoutWebhook = "http://localhost:5000/api/mentorships/payout/webhook";
            options.PaymentLifespan = 10;
        });*/
        return services;
    }


    private static IServiceCollection AddAWS(this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO : use environment variables or secrets manager for sensitive data
        var awsOptions = configuration.GetSection("AWS");
        var awsAccessKey = awsOptions["AccessKey"];
        var awsSecretKey = awsOptions["SecretKey"];
        var awsRegion = awsOptions["Region"];

        services.AddDefaultAWSOptions(new AWSOptions
        {
            Credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey),
            Region = Amazon.RegionEndpoint.GetBySystemName(awsRegion)
        });
        // have its own polling and retry policies
        // TODO : recheck it 
        // SES 
        services.AddAWSService<IAmazonSimpleEmailService>();

        // S3 
        services.AddAWSService<IAmazonS3>();

        return services;
    }
}