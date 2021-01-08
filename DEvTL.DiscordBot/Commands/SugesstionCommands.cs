using DEvTL.DiscordBot.Services;
using DEvTL.DiscordBot.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Commands
{
    public class SugesstionCommands : ModuleBase
    {
        private readonly IOptionsMonitor<DiscordBotOptions> _options;
        private readonly SuggestionService _suggestionService;

        public SugesstionCommands(IOptionsMonitor<DiscordBotOptions> options, SuggestionService sugguestionService)
        {
            _options = options;
            _suggestionService = sugguestionService;
        }

        [Command("Suggest")]
        public async Task SuggestAsync(string type, [Remainder] string suggestion)
        {
            var messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();


            if (_suggestionService.Process(out var channel, type.ToLower()))
            {
                var builder = new EmbedBuilder()
                .WithInformationColor()
                .AddField($"New suggestion made by {Context.User.Username} in #{Context.Channel.Name}", suggestion)
                .WithCurrentTimestamp();

                var embed = builder.Build();

                await channel.SendMessageAsync(embed: embed);

                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
            }

            return;
        }
    }
}
