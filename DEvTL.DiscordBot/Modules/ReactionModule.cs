using DEvTL.DiscordBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Modules
{
    public class ReactionModule : IModule
    {
        private readonly ReactionService _reactionService;
        private readonly DiscordSocketClient _client;
        private readonly ILogger<ReactionModule> _logger;

        public ReactionModule(
            ReactionService reactionService,
            DiscordSocketClient client,
            ILogger<ReactionModule> logger)
        {
            _reactionService = reactionService;
            _client = client;
            _logger = logger;

        }

        public Task InitializeAsync()
        {
            _client.ReactionAdded += OnReactionAddedAsync;
            _client.ReactionRemoved += OnReactionRemovedAsync;

            return Task.CompletedTask;
        }

        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            _logger.LogInformation("ReactionAdded");
            var (role, user, _) = _reactionService.Process(message, channel, reaction);

            if (role == null || user == null)
            {
                _logger.LogInformation("Role or user is null");
                return;
            }

            _logger.LogInformation("Add role {role}{roleId} to user {user}{userId}", role.Name, role.Id, user.Username, user.Id);
            await user.AddRoleAsync(role);
            _logger.LogInformation("Added role");
        }

        private async Task OnReactionRemovedAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var (role, user, _) = _reactionService.Process(message, channel, reaction);
        
            if (role == null || user == null)
            {
                return;
            }
        
            _logger.LogInformation("Remove role {role}{roleId} to user {user}{userId}", role.Name, role.Id, user.Username, user.Id);
            await user.RemoveRoleAsync(role);
        }
    }
}
