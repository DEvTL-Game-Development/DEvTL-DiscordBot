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

        public bool Process (out IMessageChannel channel, string type)
        {
            channel = null;

            bool temp = TryGetMessageChannel(out var messageChannel, type);
            if (temp)
            {
                channel = messageChannel;
                return true;
            }
            return false;
        }

        private bool GetConfiguration(out ModuleConfiguration.SuggestionModuleConfiguration.Suggestion configuration, string type)
        {
            configuration = _options.CurrentValue.ModuleConfiguration.Suggestion.Items.SingleOrDefault(
                sugguestion => type == sugguestion.Type);

            if (configuration is not null)
            {
                return true;
            }

            return false;
        }


        private bool TryGetMessageChannel(out IMessageChannel messageChannel, string type)
        {
            messageChannel = null;

            if (GetConfiguration(out var config, type))
            {
                var channel = _client.GetChannel(config.ChannelId);

                if (channel is IMessageChannel msgChannel)
                {
                    messageChannel = msgChannel;
                    return true;
                }
            }
            return false;
        }
    }
}
