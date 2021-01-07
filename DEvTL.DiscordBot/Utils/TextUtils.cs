using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Utils
{
    public static class TextUtils
    {
        public static string Bold(string text) => $"**{text}**";

        public static string Italic(string text) => $"_{text}_";
        public static string Spoiler(string text) => $"||{text}||";

        public static string MessageLink(string text, ulong serverId, ulong channelId, ulong messageId) => $"[{text}](https://discordapp.com/channels/{serverId}/{channelId}/{messageId})";
    }
}
