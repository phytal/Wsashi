using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.Management.Commands
{
    public class PromoteAndDemote : WsashiModule
    {
        [Command("promote")]
        [Summary("Promotes a user to a certain rank")]
        [Remarks("w!promo <rank (admin/mod/helper)> <person you want to promote> Ex: w!promo admin @Phytal")]
        [Cooldown(5)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Promote(string rank, IGuildUser user = null)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                try
                {
                    if (rank == "admin" || rank == "administrator")
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName); ;
                        await user.AddRoleAsync(role);
                        await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.AdminRoleName + " rank! Congratulations!");
                    }
                    if (rank == "mod" || rank == "moderator")
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName); ;
                        await user.AddRoleAsync(role);
                        await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.ModRoleName + " rank! Congratulations!");
                    }
                    if (rank == "helper")
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.HelperRoleName); ;
                        await user.AddRoleAsync(role);
                        await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.HelperRoleName + " rank! Congratulations!");
                    }
                }
                catch
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to promote the user to. Ex: w!promote <rank> <@username>");
                    await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("demote")]
        [Summary("Demotes a user to a certain role")]
        [Remarks("w!demote <rank (mod/helper/member)> <person you want to demote> Ex: w!demote mod @Phytal")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Cooldown(5)]
        public async Task Demote(string rank, IGuildUser user = null)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                try
                {
                    var role1 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.HelperRoleName);
                    var role2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName);
                    var role3 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName);
                    if (rank == "mod" || rank == "moderator")
                    {
                        await user.AddRoleAsync(role2);
                        await user.RemoveRoleAsync(role3);
                        await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to the " + config.ModRoleName + " rank.");
                    }
                    if (rank == "helper")
                    {
                        await user.AddRoleAsync(role1);
                        await user.RemoveRoleAsync(role2);
                        await user.RemoveRoleAsync(role3);
                        await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to the " + config.HelperRoleName + " rank.");
                    }
                    if (rank == "member")
                    {
                        await user.RemoveRoleAsync(role1);
                        await user.RemoveRoleAsync(role2);
                        await user.RemoveRoleAsync(role3);
                        await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention);
                    }
                }
                catch
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to demote the user to. Ex: w!demote <@username> <rank>");
                    await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
