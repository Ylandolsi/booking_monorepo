using System.Data.Common;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Booking.Api;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Users.Presistence;
using IntegrationsTests.Mocking;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace IntegrationsTests.Abstractions.Base;

// docker pull datalust/seq:latest
// docker ps 
//private readonly IContainer _seqContainer = new ContainerBuilder()
//            .WithImage("datalust/seq:latest")
//            .WithName("seq-test-container")
//            .WithEnvironment("ACCEPT_EULA", "Y")
//            .WithPortBinding(80, true)
//            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(80))
//            .Build();
public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;


    private Respawner _respawner = default!;
    private string _connectionString = default!;
    private DbConnection _dbConnection = default!;

    public List<SendEmailRequest> CapturedEmails { get; private set; } = new();

    public IntegrationTestsWebAppFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("MentorMentee__test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();



        // Start the container and set the connection string synchronously
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        _connectionString = _dbContainer.GetConnectionString();

        Environment.SetEnvironmentVariable("ConnectionStrings:Database", _connectionString);
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables();
        });
      
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add dummy Google auth settings that will be used during registration
            var testConfig = new Dictionary<string, string>
            {
                ["Google:ClientId"] = "test-client-id",
                ["Google:ClientSecret"] = "test-client-secret"
            };

            config.AddInMemoryCollection(testConfig);
        });


        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<UsersDbContext>));


            if (descriptor != null)
            {
                services.Remove(descriptor);
            }


            services.AddDbContext<UsersDbContext>((sp, options)=> options
                .UseNpgsql(_connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "users");
                    })
                .UseSnakeCaseNamingConvention());

            // Add MentorshipsDbContext for testing
            var mentorshipsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MentorshipsDbContext>));
            if (mentorshipsDescriptor != null)
            {
                services.Remove(mentorshipsDescriptor);
            }
            
            services.AddDbContext<MentorshipsDbContext>((sp, options) => options
                .UseNpgsql(_connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "mentorships");
                    })
                .UseSnakeCaseNamingConvention());

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAmazonSimpleEmailService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var mockSes = CaptureAmazonSESServiceMock.CreateMock(out List<SendEmailRequest> capturedEmails);
            CapturedEmails = capturedEmails;
            services.AddSingleton<IAmazonSimpleEmailService>(mockSes);




        });
      


    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using (var scope = Services.CreateScope())
        {
            var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            await usersDbContext.Database.MigrateAsync();
            await SeedData.Initialize(usersDbContext);
            
            var mentorshipsDbContext = scope.ServiceProvider.GetRequiredService<MentorshipsDbContext>();
            await mentorshipsDbContext.Database.MigrateAsync();
        }
        await InitializeDbRespawner();
    }


    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();

        if (_dbConnection?.State == System.Data.ConnectionState.Open)
        {
            await _dbConnection.CloseAsync();
        }
        _dbConnection?.Dispose();

        NpgsqlConnection.ClearAllPools();

        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task ResetDatabase()
    {

        await _respawner.ResetAsync(_dbConnection);
        using (var scope = Services.CreateScope())
        {
            var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            await SeedData.Initialize(usersDbContext);
        }
    }

    // respawner : library used to reset db between tests 
    private async Task InitializeDbRespawner()
    {
        _connectionString = _dbContainer.GetConnectionString();
        _dbConnection = new NpgsqlConnection(_connectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            // specify that the db is a postgres db
            // and the schema to be reset is public 
            DbAdapter = DbAdapter.Postgres,
            // TODO : add more schemas if needed
            SchemasToInclude = new[] { "users", "mentorships" }
        });
    }


}