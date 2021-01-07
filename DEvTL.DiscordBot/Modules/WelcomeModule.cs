using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Modules
{
    public class WelcomeModule : IModule
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<WelcomeModule> _logger;
        private readonly IOptionsMonitor<DiscordBotOptions> _options;

        public WelcomeModule(DiscordSocketClient client, ILogger<WelcomeModule> logger, IOptionsMonitor<DiscordBotOptions> options)
        {
            _client = client;
            _logger = logger;
            _options = options;
        }

        public Task InitializeAsync()
        {
            _client.UserJoined += OnUserJoined;

            return Task.CompletedTask;
        }

        private async Task OnUserJoined(SocketGuildUser arg)
        {
            _logger.LogInformation("User joined server");

            if (!(_client.GetChannel(_options.CurrentValue.ModuleConfiguration.Welcome.WelcomeChannelId) is IMessageChannel channel))
            {
                return;
            }

            await channel.SendMessageAsync(
                $"Hi {MentionUtils.MentionUser(arg.Id)} welcome to this very nice server. Pleas read the rules in {MentionUtils.MentionChannel(_options.CurrentValue.ModuleConfiguration.Welcome.RulesChannelId)} and accept them. After that you will gain full acces to this server.");
        }
    }
}
