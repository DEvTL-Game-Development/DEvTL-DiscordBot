using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Extensions
{
    public static class ServiceProviderExtension
    {
        public static void AddDiscordBot (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DiscordBotOptions>().Bind(configuration);
        }


    }
}
