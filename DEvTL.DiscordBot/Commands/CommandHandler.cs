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
        private readonly IOptionsMonitor<DiscordBotOptions> _hostOptions;
        private readonly ILogger<CommandHandler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(DiscordSocketClient client, CommandService commandService,  IOptionsMonitor<DiscordBotOptions> hostOptions, ILogger<CommandHandler> logger, IServiceProvider serviceProvider)
        {
            _client = client;
            _commandService = commandService;
            _hostOptions = hostOptions;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            //_logger.LogInformation($"Prefix: {_hostOptions.CurrentValue.HostingConfiguration.Prefix}");
            _client.MessageReceived += MessageReceived;
            _commandService.CommandExecuted += CommandExecuted;

            _client.Ready += OnReady;

            await _commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                        services: null);
        }

        private Task OnReady()
        {
            _logger.LogInformation("Client is ready");
            return Task.CompletedTask;
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


            /*using (var serviceScope = _serviceProvider.CreateScope())
            {
                await _commandService.ExecuteAsync(context, argPos, serviceScope.ServiceProvider);
            }*/

            await _commandService.ExecuteAsync(context, argPos, _serviceProvider);


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