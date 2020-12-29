using DEvTL.DiscordBot.BackgroundServices;
using DEvTL.DiscordBot.Commands;
using DEvTL.DiscordBot.Hosting;
using Discord.Commands;
using Discord.WebSocket;
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
        public static void AddDiscordBot (this IServiceCollection services, IConfiguration ModuleConfiguration, IConfiguration Data)
        {
            services.AddOptions<DiscordBotOptions>().Bind(ModuleConfiguration);
            services.AddOptions<DiscordBotOptions>().Bind(Data);
            services.AddSingleton<DEvTL.DiscordBot.Hosting.Bot>();
            services.AddSingleton<DiscordSocketClient>();

            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandler>();


            services.AddHostedService<HostingBackgroundService>();

        }

        public static void AddDiscordBotHost (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DiscordBotOptions>().Bind(configuration);
        }


        private static DiscordSocketClient DiscordSocketClientFactory()
        {
            return new DiscordSocketClient(new DiscordSocketConfig
            {

            });
        }


    }
}
