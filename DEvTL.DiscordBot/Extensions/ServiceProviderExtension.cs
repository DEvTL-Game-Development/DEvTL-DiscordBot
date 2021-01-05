using System;
using System.Linq;
using System.Reflection;
using DEvTL.DiscordBot.BackgroundServices;
using DEvTL.DiscordBot.Commands;
using DEvTL.DiscordBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace DEvTL.DiscordBot.Extensions
{
    public static class ServiceProviderExtension
    {
        public static void AddDiscordBot(this IServiceCollection services, IConfiguration BotConfiguration)
        {
            services.AddOptions<DiscordBotOptions>().Bind(BotConfiguration);
            services.AddSingleton<DEvTL.DiscordBot.Hosting.Bot>();

            services.AddSingleton(DiscordSocketClientFactory);

            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandler>();

            services.AddTransient<ReactionService>();


            services.AddHostedService<HostingBackgroundService>();

            RegisterModules(services);
        }

        private static void RegisterModules(IServiceCollection services)
        {
            var modules = Assembly.GetExecutingAssembly().DefinedTypes
              .Where(type => type.ImplementedInterfaces.Contains(typeof(IModule)))
              .Where(type => !type.IsAbstract)
              .ToList();

            foreach (var module in modules)
            {
                services.AddSingleton(typeof(IModule), module);
            }
        }

        private static DiscordSocketClient DiscordSocketClientFactory(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<DiscordBotOptions>>();

            return new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = options.Value.AlwaysDownloadUsers
            });
        }
    }
}
