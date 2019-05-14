using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.Management.Commands
{
    public class ServerPrefix : WsashiModule
    {
        [Command("ServerPrefix")]
        [Alias("setprefix")]
        [Summary("Changes the prefix for the bot on the current server")]
        [Remarks("w!serverprefix <desired prefix> Ex: w!serverprefix ~")]
        [Cooldown(5)]
        public async Task SetGuildPrefix([Remainder]string prefix = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                if (prefix == null)
                {
                    config.CommandPrefix = "w!";
                    GlobalGuildAccounts.SaveAccounts();

                    embed.WithDescription($"Set server prefix to the default prefix **(w!)**");
                }
                else
                {
                    config.CommandPrefix = prefix;
                    GlobalGuildAccounts.SaveAccounts();

                    embed.WithDescription($"Set server prefix to {prefix}");
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
