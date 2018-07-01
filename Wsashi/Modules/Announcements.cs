using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    [Group("Announcements"), Alias("Announcement"), Summary("Settings for announcements")]
    public class Announcement : ModuleBase<SocketCommandContext>
    {
        [Command("SetChannel"), Alias("Set")]
        [Remarks("Sets the channel where to post announcements")]
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
                var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("UnsetChannel"), Alias("Unset", "Off")]
        [Remarks("Turns posting announcements to a channel off")]
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
                var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }

    }
        [Group("Welcome")]
        [Summary("DM a joining user a random message out of the ones defined.")]
        public class WelcomeMessages : ModuleBase<SocketCommandContext>
        {

            [Command("channel"), Alias("Wc")]
            public async Task SetIdIntoConfig(SocketGuildChannel chnl)
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($"Set this guild's welcome channel to #{chnl}.");
                    config.WelcomeChannel = chnl.Id;
                    GlobalGuildAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }

            [Command("add")]
            [Remarks("Example: `welcome add <usermention>, welcome to **<guildname>**! " +
                     "Try using ```@<botname>#<botdiscriminator> help``` for all the commands of <botmention>!`\n" +
                     "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                     "`<botname>`, `<botdiscriminator>`, `<botmention>` ")]
            public async Task AddWelcomeMessage([Remainder] string message)
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var response = $"Failed to add this Welcome Message...";
                    if (guildAcc.WelcomeMessages.Contains(message) == false)
                    {
                        guildAcc.WelcomeMessages.Add(message);
                        GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                        response = $"Successfully added ```\n{message}\n``` as Welcome Message!";
                    }
                    await Context.Channel.SendMessageAsync(response);
                }

                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }

            [Command("remove"), Remarks("Removes a Welcome Message from the ones availabe")]
            public async Task RemoveWelcomeMessage(int messageIndex)
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
                    var response = $"Failed to remove this Welcome Message... Use the number shown in `welcome list` next to the `#` sign!";
                    if (messages.Count > messageIndex - 1)
                    {
                        messages.RemoveAt(messageIndex - 1);
                        GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                        response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
                    }

                    await ReplyAsync(response);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }

            [Command("list"), Remarks("Shows all currently set Welcome Messages")]
            public async Task ListWelcomeMessages()
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var welcomeMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
                    var embB = new EmbedBuilder().WithTitle("No Welcome Messages set yet... add some if you want to greet incoming people!");
                    if (welcomeMessages.Count > 0) embB.WithTitle("Possible Welcome Messages:");

                    for (var i = 0; i < welcomeMessages.Count; i++)
                    {
                        embB.AddField($"Message #{i + 1}:", welcomeMessages[i], true);
                    }
                    await ReplyAsync("", false, embB.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }
        }

        [Group("Leave")]
        [Summary("Announce a leaving user in the set announcement channel" +
                 "with a random message out of the ones defined.")]
        public class LeaveMessages : ModuleBase<SocketCommandContext>
        {
            [Command("add")]
            [Remarks("Example: `leave add Oh noo! <usermention>, left <guildname>...`\n" +
                     "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                     "`<botname>`, `<botdiscriminator>`, `<botmention>`")]
            public async Task AddLeaveMessage([Remainder] string message)
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var response = $"Failed to add this Leave Message...";
                    if (guildAcc.LeaveMessages.Contains(message) == false)
                    {
                        guildAcc.LeaveMessages.Add(message);
                        GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                        response = $"Successfully added `{message}` as Leave Message!";
                    }

                    await ReplyAsync(response);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }

            [Command("remove"), Remarks("Removes a Leave Message from the ones available")]
            public async Task RemoveLeaveMessage(int messageIndex)
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                    var response = $"Failed to remove this Leave Message... Use the number shown in `leave list` next to the `#` sign!";
                    if (messages.Count > messageIndex - 1)
                    {
                        messages.RemoveAt(messageIndex - 1);
                        GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                        response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
                    }

                    await ReplyAsync(response);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }

            [Command("list"), Remarks("Shows all currently set Leave Messages")]
            public async Task ListLeaveMessages()
            {
                var guser = Context.User as SocketGuildUser;
                if (guser.GuildPermissions.Administrator)
                {
                    var leaveMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                    var embB = new EmbedBuilder().WithTitle("No Leave Messages set yet... add some if you want a message to be shown if someone leaves.");
                    if (leaveMessages.Count > 0) embB.WithTitle("Possible Leave Messages:");

                    for (var i = 0; i < leaveMessages.Count; i++)
                    {
                        embB.AddField($"Message #{i + 1}:", leaveMessages[i], true);
                    }
                    await ReplyAsync("", false, embB.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }
        }
    }
}
