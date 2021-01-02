using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Services
{
    public class ReactionService
    {
        private readonly IOptionsMonitor<DiscordBotOptions> _options;
        private readonly ILogger<ReactionService> _logger;
        private readonly DiscordSocketClient _client;

        public ReactionService(IOptionsMonitor<DiscordBotOptions> options, ILogger<ReactionService> logger, DiscordSocketClient client)
        {
            _options = options;
            _logger = logger;
            _client = client;
        }

        public (IRole Role, SocketGuildUser User, ModuleConfiguration.ReactionModuleConfiguration.Reaction configuration) Process(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {

            if (!reaction.User.IsSpecified || !(reaction.User.Value is SocketGuildUser guildUser))
            {
                _logger.LogWarning("No user specified for for {ChannelId} {MessageId} {Emoji}", channel.Id, message.Id, reaction.Emote.Name);
                return (null, null, null);
            }

            var configuration = GetConfigurationForMessage(reaction.Emote.Name, channel.Id, message.Id);

            if (configuration == null)
            {
                _logger.LogWarning("No configuration found for {ChannelId} {MessageId} {Emoji}", channel.Id, message.Id, reaction.Emote.Name);
                return (null, null, null);
            }

            return (guildUser.Guild.GetRole(configuration.RoleId), guildUser, configuration);
        }


        private ModuleConfiguration.ReactionModuleConfiguration.Reaction GetConfigurationForMessage(string emoji, ulong channelId, ulong messageId)
        {
            return _options.CurrentValue.ModuleConfiguration.Reactions.Items.FirstOrDefault(
                reaction => emoji == reaction.Emoji && channelId == reaction.ChannelId && messageId == reaction.MessageId);
        }
    }
}
