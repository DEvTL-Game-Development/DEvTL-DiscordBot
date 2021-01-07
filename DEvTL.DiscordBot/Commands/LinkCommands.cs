using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEvTL.DiscordBot.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DEvTL.DiscordBot.Commands
{
    public class LinkCommands : ModuleBase<ICommandContext>
    {
        [Command("links")]
        public async Task Links()
        { 
            var builder = new EmbedBuilder()
                .WithTitle("Useful Links")
                .WithInformationColor()
                .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                .AddField("Codecks", "https://devtl.codecks.io/decks")
                .AddField("GitHub Repository", "https://github.com/DEvTL-Game-Development/Unititled-Tycoon-Game")
                .AddField("Our e-mail", "devtl@outlook.de")
                .AddField("Discord invite link", "https://discord.gg/tPYfzgC");

            var embed = builder.Build();

            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}