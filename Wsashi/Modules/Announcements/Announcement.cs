using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Announcements
{
    public class Announcement : WsashiModule
    {
        [Command("Announcements SetChannel"), Alias("ASet")]
        [Summary("Sets the channel where to post announcements")]
        [Remarks("w!Aset <channel where you want announcements to be sent> Ex: w!Aset #announcemnts")]
        public async Task SetAnnouncementChannel(ITextChannel channel)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                guildAcc.AnnouncementChannelId = channel.Id;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                await ReplyAsync("The Announcement-Channel has been set to " + channel.Mention);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("AnnouncementsUnsetChannel"), Alias("AUnset", "AOff")]
        [Summary("Turns posting announcements to a channel off")]
        [Remarks("w!Aoff")]
        public async Task UnsetAnnouncementChannel()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                guildAcc.AnnouncementChannelId = 0;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                await ReplyAsync("Now there is no Announcement-Channel anymore! No more Announcements from now on... RIP!");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
