using DEvTL.DiscordBot.Utils;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEvTL.DiscordBot.Extensions
{
    public static class EmbedBuilderExtension
    {
        private static readonly Color red = new Color(255, 56, 20);
        private static readonly Color yellow = new Color(255, 56, 20);
        private static readonly Color green = new Color(21, 255, 0);
        private static readonly Color blue = new Color(0, 162, 255);

        public static EmbedBuilder WithDefaultColor(this EmbedBuilder builder)
        {
            return builder.WithColor(yellow);
        }

        public static EmbedBuilder WithErrorColor(this EmbedBuilder builder)
        {
            return builder.WithColor(red);
        }

        public static EmbedBuilder WithSucessColor(this EmbedBuilder builder)
        {
            return builder.WithColor(green);
        }
        public static EmbedBuilder WithInformationColor (this EmbedBuilder builder)
        {
            return builder.WithColor(blue);
        }

        public static EmbedBuilder WithBoldDescription(this EmbedBuilder builder, string Description)
        {
            return builder.WithDescription(TextUtils.Bold(Description));
        }
    }
}
