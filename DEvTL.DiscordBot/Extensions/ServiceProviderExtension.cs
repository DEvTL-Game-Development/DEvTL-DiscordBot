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
        public static void AddDiscordBot (this IServiceCollection services, IConfiguration BotConfiguration)
        {
            services.AddOptions<DiscordBotOptions>().Bind(BotConfiguration);
            services.AddSingleton<DEvTL.DiscordBot.Hosting.Bot>();
            services.AddSingleton<DiscordSocketClient>();

            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandler>();


            services.AddHostedService<HostingBackgroundService>();

        }


        private static DiscordSocketClient DiscordSocketClientFactory()
        {
            return new DiscordSocketClient(new DiscordSocketConfig
            {

            });
        }


    }
}
