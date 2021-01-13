using System;
using System.Linq;
using System.Threading.Tasks;
using DEvTL.DiscordBot.Extensions;
using DEvTL.DiscordBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DEvTL.DiscordBot.Modules
{
    public class EventLoggerModule : IModule
    {
        private readonly ILogger<EventLoggerModule> _logger;
        private readonly ChannelLogger _channelLogger;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;

        public EventLoggerModule(ILogger<EventLoggerModule> logger, ChannelLogger channelLogger, DiscordSocketClient client, CommandService commandService)
        {
            _logger = logger;
            _channelLogger = channelLogger;
            _client = client;
            _commandService = commandService;
        }

        public Task InitializeAsync()
        {
            _client.Log += DiscordLogAsync;
            _commandService.Log += DiscordLogAsync;

            _client.GuildMemberUpdated += LogGuildMemberUpdatedAsync;
            _client.UserLeft += LogUserLeftAsync;
            return Task.CompletedTask;
        }

        private async Task LogUserLeftAsync(SocketGuildUser arg)
        {
            var builder = CreateDefaultEmbedBuilder(arg);

            await LogDestructiveAsync(builder.WithBoldDescription($"{MentionUtils.MentionUser(arg.Id)} has left the server"));


        }

        private Task DiscordLogAsync(LogMessage logMessage)
        {
            _logger.Log(logMessage.Severity.ToLogLevel(), logMessage.Exception, "{0}: {1}", logMessage.Source, logMessage.Message);

            return Task.CompletedTask;
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
