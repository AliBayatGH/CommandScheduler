using CommandScheduler;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

namespace CommandScheduler
{
    public static class CommandsSchedulerServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandsScheduler(this IServiceCollection services, string hangfireConnection, params Type[] handlerAssemblyMarkerTypes)
        {
            services.AddScoped<CommandsExecutor>();
            services.AddScoped<AsyncCommand>();


            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(GetHangfireConnectionString(hangfireConnection), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddMediatR(handlerAssemblyMarkerTypes);

            return services;
        }

        private static string GetHangfireConnectionString(string hangfireConnection)
        {

            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();

            connBuilder.ConnectionString = hangfireConnection;

            string hangfireDb = connBuilder.InitialCatalog;

            var masterConnection = hangfireConnection.Replace(hangfireDb, "master");

            using (var connection = new SqlConnection(masterConnection))
            {

                connection.Open();

                using (var command = new SqlCommand($@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{hangfireDb}') create database [{hangfireDb}];", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            hangfireConnection = masterConnection.Replace("master", hangfireDb);

            return hangfireConnection;
        }
    }
}