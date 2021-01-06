using DEvTL.DiscordBot.Services;
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
        private readonly SuggestionService _sugguestionService;

        public SugesstionCommands(IOptionsMonitor<DiscordBotOptions> options, SuggestionService sugguestionService)
        {
            _options = options;
            _sugguestionService = sugguestionService;
        }

        [Command("Suggest")]
        public async Task SuggestAsync(string type, string suggestion)
        {
            var channel = _sugguestionService.Process(type);

            await channel.SendMessageAsync(suggestion);
        }
    }
}
