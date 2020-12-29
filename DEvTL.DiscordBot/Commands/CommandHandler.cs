using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IOptionsMonitor<DiscordBotOptions> _botOptions;
        private readonly IOptionsMonitor<DiscordHostOptions> _hostOptions;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IOptionsMonitor<DiscordBotOptions> botOptions, IOptionsMonitor<DiscordHostOptions> hostOptions, ILogger<CommandHandler> logger)
        {
            _client = client;
            _commandService = commandService;
            _botOptions = botOptions;
            _hostOptions = hostOptions;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation($"Prefix: {_hostOptions.CurrentValue.Prefix}");
            _client.MessageReceived += MessageReceived;
            _commandService.CommandExecuted += CommandExecuted;
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            if (rawMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;

            if (!message.HasCharPrefix(_hostOptions.CurrentValue.Prefix, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            _logger.LogInformation($"Message with the prefix {_hostOptions.CurrentValue.Prefix} recived from #{context.Channel.Name}");
            

        }

        private async Task CommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                return;
            }

            if (result.IsSuccess)
            {
                return;
            }

            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }
}