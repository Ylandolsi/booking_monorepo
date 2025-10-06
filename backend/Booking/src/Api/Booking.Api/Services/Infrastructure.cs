using System.Text.Json.Serialization;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Booking.Common;
using Booking.Common.Authentication;
using Booking.Common.Email;
using Booking.Common.Options;
using Booking.Modules.Users;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Persistence;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;

namespace Booking.Api.Services;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        WebApplicationBuilder builder)
    {
        return services
            .AddCors()
            .AddEnumToString()
            .AddOptions(configuration)
            .AddServices(configuration)
            .AddCache(configuration)
            //.AddResielenecPipelines(configuration)
            .AddAWS(configuration)
            .AddIdentityCore()
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();
    }
    //.AddObservability(builder);

    /*private static IServiceCollection AddObservability(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Meetini"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddNpgsql();

                tracing.AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;

            logging.AddOtlpExporter();
        });

        return  services;


    }*/

    public static IServiceCollection AddEnumToString(this IServiceCollection services)
    {
        // Map enums to strings 
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHttpClient();
        // move clients name to static 
        var konnectOptions = configuration.GetSection(KonnectOptions.OptionsKey)?.Get<KonnectOptions>() ??
                             throw new InvalidOperationException("konnect options are not configured.");

        services.AddHttpClient("KonnectClient", client =>
            {
                //make this more reselllient
                client.BaseAddress = new Uri(konnectOptions.ApiUrl);
                client.DefaultRequestHeaders.Add("x-api-key", konnectOptions.ApiKey);
                client.Timeout = TimeSpan.FromSeconds(300);
                /* Enable this for prod 
                client.Timeout = TimeSpan.FromSeconds(konnectOptions.PaymentLifespan);
            */
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
            // Consider logging this or throwing a more specific exception
            throw new InvalidOperationException("Database connection string is not configured.");

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
        return services;
    }


    private static IServiceCollection AddAWS(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Backblaze B2 S3-compatible API configuration
        var awsSection = configuration.GetSection("AWS");

        var accessKeyId = awsSection["AccessKeyId"];
        var secretAccessKey = awsSection["SecretAccessKey"];
        var serviceUrl = awsSection["ServiceURL"];
        var region = awsSection["Region"] ?? "us-west-004";

        // Configure S3 client for Backblaze B2
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var s3Config = new AmazonS3Config
            {
                // Backblaze B2 S3-compatible endpoint
                ServiceURL = serviceUrl ?? "https://s3.us-west-004.backblazeb2.com",

                // Set authentication region
                AuthenticationRegion = region,

                // Force path style for Backblaze compatibility
                ForcePathStyle = true
            };

            return new AmazonS3Client(accessKeyId, secretAccessKey, s3Config);
        });

        // SES (can remain unchanged if using AWS SES, or remove if not needed)
        var awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey),
            Region = RegionEndpoint.GetBySystemName(region)
        };

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonSimpleEmailService>();

        return services;
    }
}