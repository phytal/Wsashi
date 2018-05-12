using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Modules.ServerManagement
{
    public class Reports_Channel : ModuleBase<SocketCommandContext>
    {
        [Command("reports")]
        [Summary("If the reports channel isn't automatically created, you can use this command to manually create it")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task Text()
        {
            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
            var perms = new OverwritePermissions(
                sendMessages: PermValue.Deny,
                addReactions: PermValue.Deny,
                readMessages: PermValue.Allow
                );
            var channel = await Context.Guild.CreateTextChannelAsync("Reports");
            channel.AddPermissionOverwriteAsync(role, perms);
        }

        [Command("Report")]
        [Summary("Reports @Username")]
        public async Task ReportAsync(SocketGuildUser user, [Remainder] string reason)
        {
            //var reportchannel = await 
            if ((user == null)
                || (string.IsNullOrWhiteSpace(reason)))
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":hand_splayed:  | You must mention a user and provide a reason. Ex: w!report @Username <reason>");
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
            else
            {
                var muteRoleName = "Reports";
                var channels = Context.Guild.TextChannels;
                var reports = channels.FirstOrDefault(r => r.Name == muteRoleName) as SocketTextChannel;
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $"{Context.User}'s report of {user.Username}";
                embed.Description = $"**Username: **{user.Username}\n**Reported by: **{Context.User.Mention}\n**Reason: **{reason}";
                await reports.SendMessageAsync("", false, embed);
                await ReplyAsync(":white_check_mark:  | *Your report has been furthered to staff.* ***(ABUSE OF THIS COMMAND IS PUNISHABLE)***");
            }
        }

        public async Task GetReportChannel(IGuild guild)
        {
            var muteRoleName = "Reports";
            var channels = Context.Guild.TextChannels;

            var TextChannel = channels.FirstOrDefault(r => r.Name == muteRoleName);

            if (TextChannel == null)
            {
                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                var perms = new OverwritePermissions(
                    sendMessages: PermValue.Deny,
                    addReactions: PermValue.Deny,
                    readMessages: PermValue.Allow
                    );
                var channel = await Context.Guild.CreateTextChannelAsync("Reports");
                channel.AddPermissionOverwriteAsync(role, perms);
            }

            return;
        }
    }   
}
