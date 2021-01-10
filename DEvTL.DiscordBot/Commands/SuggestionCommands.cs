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
    public class SuggestionCommands : ModuleBase
    {
        private readonly IOptionsMonitor<DiscordBotOptions> _options;
        private readonly SuggestionService _suggestionService;

        public SuggestionCommands(IOptionsMonitor<DiscordBotOptions> options, SuggestionService sugguestionService)
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

                var successEmbedBuilder = new EmbedBuilder()
                    .WithSucessColor()
                    .WithBoldDescription($"I have added you suggestion succesfully and posted it in {MentionUtils.MentionChannel(channel.Id)} to be voted on.")
                    .WithCurrentTimestamp();

                var successEmbed = successEmbedBuilder.Build();

                var msg = await (Context.Channel as SocketTextChannel).SendMessageAsync(embed: successEmbed);

                await Task.Delay(5000);
                await msg.DeleteAsync();
            }

            return;
        }
    }
}
