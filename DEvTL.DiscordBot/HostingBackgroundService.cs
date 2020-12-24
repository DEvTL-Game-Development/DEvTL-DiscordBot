using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DEvTL.DiscordBot
{
    public class HostingBackgroundService : BackgroundService
    {
        private readonly DiscordBot _discordBot;

        public HostingBackgroundService(DiscordBot discordBot)
        {
            _discordBot = discordBot;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _discordBot.StartAsync();

            await Task.Delay(-1, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            await _discordBot.StopAsync();
        }

        public override void Dispose()
        {
            base.Dispose();

            _discordBot.Dispose();
        }
    }
}
