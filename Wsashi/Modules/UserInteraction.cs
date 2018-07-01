using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class UserInteraction : WsashiModule
    {
        [Command("Iam")]
        [Alias("iam")]
        [Summary("Gives you a self role")]
        public async Task GiveYourselfRole([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var user = Context.User as SocketGuildUser;
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255);
            if (config == null)
            {
                embed.WithDescription("This server doesn't have any self roles set.");
            }
            else
            {
                if (config.SelfRoles.Contains(role))
                {
                    embed.WithDescription($"Gave you the **{role}** role.");
                    var r = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
                    await user.AddRoleAsync(r);
                }
                else
                {
                    embed.WithDescription("That role isn't in the self roles list for this server. Remember that this command is cAsE sEnSiTiVe!");
                }
            }

            await ReplyAsync("", false, embed.Build());
        }

        [Command("Iamnot"), Alias("Iamn", "iamnot", "iamn")]
        [Summary("Remove a self role from you")]
        public async Task TakeAwayRole([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var user = Context.User as SocketGuildUser;
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255);
            if (config == null)
            {
                embed.WithDescription("This server doesn't have any self roles set.");
            }
            else
            {
                if (config.SelfRoles.Contains(role))
                {
                    embed.WithDescription($"Removed your **{role}** role.");
                    var r = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
                    await user.RemoveRoleAsync(r);
                }
                else
                {
                    embed.WithDescription("That role isn't in the self roles list for this server. Remember that this command is cAsE sEnSiTiVe!");
                }
            }

            await ReplyAsync("", false, embed.Build());
        }
    }
}
