using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DEvTL.DiscordBot.Commands
{
    public class LinkCommands : ModuleBase
    {
        [Command("links")]
        public async Task Links()
        {
            var builder = new EmbedBuilder()
                .WithTitle("Useful Links")
                .WithColor(255, 0, 0)
                .AddField("Codecks", "test");

            var embed = builder.Build();

            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}