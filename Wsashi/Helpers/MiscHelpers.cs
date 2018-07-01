using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Helpers
{
    class MiscHelpers
    {
        public static async Task SendMessage(SocketCommandContext ctx, EmbedBuilder embed = null, string msg = "")
        {
            if (embed == null)
            {
                await ctx.Channel.SendMessageAsync(msg);
            }
            else
            {
                await ctx.Channel.SendMessageAsync(msg, false, embed.Build());
            }
        }

        internal static bool UserHasRole(SocketCommandContext ctx, ulong roleId)
        {
            var targetRole = ctx.Guild.Roles.FirstOrDefault(r => r.Id == roleId);
            var gUser = ctx.User as SocketGuildUser;

            return (gUser.Roles.Contains(targetRole));
        }

        public static EmbedBuilder CreateEmbed(SocketCommandContext ctx, string desc)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(ctx.Guild.Id);
            var embed = new EmbedBuilder()
                .WithDescription(desc)
                .WithColor(37, 152, 255);
            return embed;
        }
    }
}
