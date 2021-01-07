using DEvTL.DiscordBot.Services;
using DEvTL.DiscordBot.Extensions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Modules
{
    public class EventLoggerModule : IModule
    {
        private readonly ILogger<EventLoggerModule> _logger;
        private readonly ChannelLogger _channelLogger;
        private readonly DiscordSocketClient _client;

        public EventLoggerModule(ILogger<EventLoggerModule> logger, ChannelLogger channelLogger, DiscordSocketClient client)
        {
            _logger = logger;
            _channelLogger = channelLogger;
            _client = client;
        }

        public Task InitializeAsync()
        {
            _client.GuildMemberUpdated += LogGuildMemberUpdatedAsync;
            return Task.CompletedTask;
        }

        private Task OnRoleUpdated(SocketRole arg1, SocketRole arg2)
        {
            throw new NotImplementedException();
        }

        private async Task LogGuildMemberUpdatedAsync(SocketGuildUser beforeUser, SocketGuildUser afterUser)
        {
            var builder = CreateDefaultEmbedBuilder(afterUser);

            var removedRoles = beforeUser.Roles.Except(afterUser.Roles).ToList();
            var addedRoles = afterUser.Roles.Except(beforeUser.Roles).ToList();

            if (removedRoles.Count > 0)
            {
                foreach (var role in removedRoles)
                {
                    await LogDestructiveAsync(builder
                      .WithBoldDescription($"{MentionUtils.MentionUser(beforeUser.Id)} was removed from `{role.Name}` role.")
                      .WithFooter($"User ID: {beforeUser.Id} | Role ID: {role.Id}")
                    );
                }
            }

            if (addedRoles.Count > 0)
            {
                foreach (var role in addedRoles)
                {
                    await LogInformationAsync(builder
                      .WithBoldDescription($"{MentionUtils.MentionUser(beforeUser.Id)} was added to `{role.Name}` role.")
                      .WithFooter($"User ID: {beforeUser.Id} | Role ID: {role.Id}")
                    );
                }
            }
        }

        private EmbedBuilder CreateDefaultEmbedBuilder(IUser author = null)
        {
            var builder = new EmbedBuilder()
              .WithCurrentTimestamp();

            if (author != null)
            {
                builder = builder.WithAuthor(author.Username, author.GetAvatarUrl());
            }

            return builder;
        }

        private async Task LogDestructiveAsync(EmbedBuilder builder)
        {
            await _channelLogger.LogAsync(builder.WithErrorColor());
        }

        private async Task LogInformationAsync(EmbedBuilder builder)
        {
            await _channelLogger.LogAsync(builder.WithInformationColor());
        }
    }
}
