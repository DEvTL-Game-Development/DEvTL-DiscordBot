using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot
{
    public class DiscordBot
    {
        private readonly ILogger<DiscordBot> _logger;
        private readonly DiscordBotOptions _options;
        private readonly DiscordSocketClient _socketClient;
        private readonly CommandHandler _command;

        public DiscordBot(ILogger<DiscordBot> logger,DiscordBotOptions options, DiscordSocketClient socketClient, CommandHandler command)
        {
            _logger = logger;
            _options = options;
            _socketClient = socketClient;
            _command = command;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("Logging in ...");
            await _socketClient.LoginAsync(TokenType.Bot, _options.Token);
        }
    }
}
