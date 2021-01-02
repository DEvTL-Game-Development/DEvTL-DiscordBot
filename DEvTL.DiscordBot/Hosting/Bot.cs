using DEvTL.DiscordBot.Commands;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Hosting
{
    public class Bot : IDisposable
    {
        private readonly ILogger<Bot> _logger;
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly IEnumerable<IModule> _modules;

        public Bot(
            ILogger<Bot> logger,
            DiscordSocketClient discordSocketClient,
            CommandHandler commandHandler,
            IEnumerable<IModule> modules
            )
        {
            _logger = logger;
            _client = discordSocketClient;
            _commandHandler = commandHandler;
            _modules = modules;
        }

        public async Task StartAsync()
        {

            await LoginAsync();
            await _commandHandler.InitializeAsync();
            await InternalStartAsync();

            foreach (var module in _modules)
            {
                _logger.LogInformation("Initializing {module}", module.GetType().Name);
                await module.InitializeAsync();
            }
        }

        private async Task InternalStartAsync()
        {
            _logger.LogInformation("Starting up...");

            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            await _client.StopAsync();
        }

        private async Task LoginAsync()
        {
            //if (_optionsMonitor.CurrentValue.Token == null)
            //{
            //    _logger.LogError("Please specify a Token in your configuration");
            //    return;
            //}
            _logger.LogInformation("Logging in...");

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));

            _logger.LogInformation("Logged in...");
        }

        public void Dispose() => _client?.Dispose();
    }
}
