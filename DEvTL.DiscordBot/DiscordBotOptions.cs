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
        public class ReactionModuleConfiguration
        {
            public ICollection<Reaction> Items { get; set; }

            public class Reaction
            {
                public ulong ChannelID { get; set; }
                public ulong MessageID { get; set; }
                public ulong RoleID { get; set; }
                public string Emoji { get; set; }
            }
        }

        public class HostingConfiguration
        {
            public char Prefix { get; set; }
            public string BotToken { get; set; }
        }
    }

  
}
