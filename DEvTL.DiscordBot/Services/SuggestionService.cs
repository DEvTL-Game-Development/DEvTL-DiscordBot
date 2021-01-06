using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.WebSocket;
using Microsoft.Extensions.Logging;

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

        public (SocketChannel channel, ModuleConfiguration.SuggestionModuleConfiguration.Sugguestion configuration) Process (string type)
        {
            var configuration = GetConfiguration(type);

            if(configuration is null)
            {
                _logger.LogWarning("No sugguestion type found for type {type}", type);
                return (null, null);
            }

            var channel = _client.GetChannel(configuration.ChannelId);

            return (channel, configuration);
        }

        private ModuleConfiguration.SuggestionModuleConfiguration.Sugguestion GetConfiguration(string type)
        {
            return _options.CurrentValue.ModuleConfiguration.Sugguestion.Items.SingleOrDefault(
                sugguestion => type == sugguestion.Type);
        }
    }
}
