using DEvTL.DiscordBot.Commands;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Hosting
{
    public class Bot : IDisposable
    {
        private readonly ILogger<Bot> _logger;
        private readonly IOptionsMonitor<DiscordBotOptions> _optionsMonitor;
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;

        public Bot(
          ILogger<Bot> logger,
          IOptionsMonitor<DiscordBotOptions> optionsMonitor,
          DiscordSocketClient discordSocketClient,
          CommandHandler commandHandler
        )
        {
            _logger = logger;
            _optionsMonitor = optionsMonitor;
            _client = discordSocketClient;
            _commandHandler = commandHandler;
        }

        public async Task StartAsync()
        {

            await LoginAsync();
            await _commandHandler.InitializeAsync();
            await InternalStartAsync();
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

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
