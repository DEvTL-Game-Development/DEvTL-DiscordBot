using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot
{
    public class DiscordBot : IDisposable
    {
        private readonly ILogger<DiscordBot> _logger;
        private readonly IOptionsMonitor<DiscordBotOptions> _optionsMonitor;
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;

        public DiscordBot(
          ILogger<DiscordBot> logger,
          IOptionsMonitor<DiscordBotOptions> optionsMonitor,
          DiscordSocketClient discordSocketClient
        )
        {
            _logger = logger;
            _optionsMonitor = optionsMonitor;
            _client = discordSocketClient;
        }

        public async Task StartAsync()
        {
            await LoginAsync();
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
            _logger.LogInformation("Logging in...");

            await _client.LoginAsync(TokenType.Bot, _optionsMonitor.CurrentValue.Token);

            _logger.LogInformation("Logged in...");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
