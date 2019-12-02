using CommandSchedular;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandScheduler
{
    public static class CommandsSchedulerServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandsScheduler(this IServiceCollection services)
        {
            services.AddScoped<CommandsExecutor>();
            services.AddScoped<AsyncCommand>();

            return services;
        }
    }
}
