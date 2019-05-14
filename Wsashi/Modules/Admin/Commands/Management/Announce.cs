using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Management.Commands
{
    public class Announce : WsashiModule
    {
        [Command("annouce")]
        [Summary("Announces a message to the set announcement channel")]
        [Remarks("w!announce <announcement you want to make> Ex: w!announce free burgers on friday")]
        public async Task AnnounceCMD([Remainder] string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255)
                    .WithTitle($"**:mega:  Announcement!**");
                embed.WithDescription(arg);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Messages Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
