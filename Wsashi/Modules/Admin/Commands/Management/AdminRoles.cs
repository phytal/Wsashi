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
    public class AdminRoles : WsashiModule
    {
        [Command("AdminRole")]
        [Summary("Sets the server Admin role")]
        [Remarks("w!adminrole <admin role name> Ex: w!adminrole Administrator")]
        [Cooldown(5)]
        public async Task SetServerAdminRole([Remainder] string roleName)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);

                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role == null)
                {
                    embed.WithDescription(
                        $"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
                }
                else
                {
                    embed.WithDescription($"Set the Administrator role to **{roleName}** for this server!");
                    config.AdminRole = role.Id;
                    config.AdminRoleName = role.Name;
                    GlobalGuildAccounts.SaveAccounts();
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ModRole")]
        [Summary("Sets the server Moderator role")]
        [Remarks("w!ModRole <mod role name> Ex: w!ModRole Moderator")]
        [Cooldown(5)]
        public async Task SetServerModRole([Remainder] string roleName)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);

                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role == null)
                {
                    embed.WithDescription(
                        $"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
                }
                else
                {
                    embed.WithDescription($"Set the Moderator role to **{roleName}** for this server!");
                    config.ModRole = role.Id;
                    config.ModRoleName = role.Name;
                    GlobalGuildAccounts.SaveAccounts();
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("HelperRole")]
        [Summary("Sets the server Moderator role")]
        [Remarks("w!HelperRole <helper role name> Ex: w!HelperRole Helper")]
        [Cooldown(5)]
        public async Task SetServerHelperRole([Remainder] string roleName)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);

                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role == null)
                {
                    embed.WithDescription(
                        $"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
                }
                else
                {
                    embed.WithDescription($"Set the Helper role to **{roleName}** for this server!");
                    config.HelperRole = role.Id;
                    config.HelperRoleName = role.Name;
                    GlobalGuildAccounts.SaveAccounts();
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
