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
    public class SuguesstionCommands : ModuleBase
    {
        private readonly IOptionsMonitor<DiscordBotOptions> _options;
        private readonly SugguestionService _sugguestionService;

        public SuguesstionCommands(IOptionsMonitor<DiscordBotOptions> options, SugguestionService sugguestionService)
        {
            _options = options;
            _sugguestionService = sugguestionService;
        }

        [Command("Suggest")]
        public async Task SuggestAsync(string type, string suggestion)
        {
            var (channel, _) = _sugguestionService.Process(type);

            await (channel as ISocketMessageChannel).SendMessageAsync(suggestion);
        }
    }
}
