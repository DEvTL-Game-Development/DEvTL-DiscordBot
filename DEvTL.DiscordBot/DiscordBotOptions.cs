﻿using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot
{
    public class DiscordBotOptions
    {
        public char Prefix { get; set; }
        public ModuleConfiguration ModuleConfiguration { get; set; }
    }

    public class ModuleConfiguration
    {

        public ReactionModuleConfiguration Reactions { get; set; }

        public class ReactionModuleConfiguration
        {
            public IReadOnlyCollection<Reaction> Items { get; set; }

            public class Reaction
            {
                public ulong ChannelId { get; set; }
                public ulong MessageId { get; set; }
                public ulong RoleId { get; set; }
                public string Emoji { get; set; }
            }
        }

    }

}
