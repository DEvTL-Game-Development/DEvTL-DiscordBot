using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;

namespace DEvTL.DiscordBot.Services
{
    public class SuggestionService
    {
        private readonly IOptionsMonitor<DiscordBotOptions> _options;
        private readonly ILogger<SuggestionService> _logger;
        private readonly DiscordSocketClient _client;

        public SuggestionService(IOptionsMonitor<DiscordBotOptions> options, ILogger<SuggestionService> logger, DiscordSocketClient client)
        {
            _options = options;
            _logger = logger;
            _client = client;
        }

        public IMessageChannel Process (string type)
        {
            if (TryGetMessageChannel(out var messageChannel, GetConfiguration(type).ChannelId))
            {
                return messageChannel;
            }

            return null;
        }

        private ModuleConfiguration.SuggestionModuleConfiguration.Suggestion GetConfiguration(string type)
        {
            return _options.CurrentValue.ModuleConfiguration.Sugguestion.Items.SingleOrDefault(
                sugguestion => type == sugguestion.Type);
        }


        private bool TryGetMessageChannel(out IMessageChannel messageChannel, ulong channelId)
        {
            messageChannel = null;

            var channel = _client.GetChannel(channelId);

            if(channel is IMessageChannel msgChannel)
            {
                messageChannel = msgChannel;
                return true;
            }

            return false;
        }
    }
}
