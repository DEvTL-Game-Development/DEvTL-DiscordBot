using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DEvTL.DiscordBot.Extensions;

namespace DEvTL.DiscordBot.Commands
{
    public class ModerationCommands : ModuleBase
    {
        [Command("clear", true)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Clear(int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            try
            {
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                var builder = new EmbedBuilder()
                    .WithSucessColor()
                    .WithTitle($"I have {messages.Count()} Messages deleted successfully")
                    .WithCurrentTimestamp();
                var message = await Context.Channel.SendMessageAsync(embed: builder.Build());

                await Task.Delay(5000);
                await message.DeleteAsync();
            }
            catch (ArgumentOutOfRangeException)
            {
                var builder = new EmbedBuilder()
                    .WithErrorColor()
                    .WithTitle("These messages are probably too old for me to delete them!")
                    .WithBoldDescription("I can only delete messages younger than two weeks.")
                    .WithCurrentTimestamp();
                var message = await Context.Channel.SendMessageAsync(embed: builder.Build());

                await Task.Delay(5000);
                await message.DeleteAsync();
            }
        }
    }
}
