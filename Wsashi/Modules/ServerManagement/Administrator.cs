using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using System.Net;
using System.IO;
using Wsashi.Handlers;
using Wsashi.Features.GlobalAccounts;
using Wsashi;

namespace Watchdog.Modules
{
    public class Administrator : ModuleBase
    {
        private static readonly OverwritePermissions denyOverwrite = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, attachFiles: PermValue.Deny);
        DiscordSocketClient _client;

        //(37, 152, 255) is the color code

        [Command("ban")]
        [Summary("Ban @Username")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                if (user == null)
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a user");
                }
                else
                {
                    var kb = (Context.Client as DiscordSocketClient).GetChannel(416859601590419457) as SocketTextChannel;
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{user.Username}** was banned";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";
                    if (user == null) throw new ArgumentException("You must mention a user");
                    await gld.AddBanAsync(user);
                    await Context.Channel.SendMessageAsync("", false, embed);
                    await kb.SendMessageAsync("", false, embed);
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("unban")]
        [Remarks("Unban A User")]
        public async Task Unban([Remainder]string user2)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.BanMembers)
            {
            var bans = await Context.Guild.GetBansAsync();

            var theUser = bans.FirstOrDefault(x => x.User.ToString().ToLowerInvariant() == user2.ToLowerInvariant());

            await Context.Guild.RemoveBanAsync(theUser.User).ConfigureAwait(false);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }


        [Command("kick")]
        [Summary("Kicks @Username")]
        public async Task KickAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.KickMembers)
            {
                if (user == null)
                {
                    var use = await ReplyAsync(":hand_splayed:  | You must mention a user");
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var kb = (Context.Client as DiscordSocketClient).GetChannel(416859601590419457) as SocketTextChannel;
                    await user.KickAsync();
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $" {user.Username} has been kicked from {user.Guild.Name}";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Kicked by: **{Context.User.Mention}\n**Reason: **{reason}";
                    await Context.Channel.SendMessageAsync("", false, embed);
                    await kb.SendMessageAsync("", false, embed);
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Kick Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

    [Command("mute")]
        [Summary("Mutes @Username")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task MuteAsync(SocketGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var use = await ReplyAsync(":hand_splayed:  | You must mention a user");
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var muteRole = await GetMuteRole(user.Guild);
                    if (!user.Roles.Any(r => r.Id == muteRole.Id))
                    await user.AddRoleAsync(muteRole).ConfigureAwait(false);
                    var gld = Context.Guild as SocketGuild;
                    var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $"**{user.Username}** was muted";
                    embed.Description = $"**Username: **{user.Username}\n**Muted by: **{Context.User.Username}";
                    await user.AddRoleAsync(muted);
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("unmute")]
        [Summary("Unmutes @Username")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task UnmuteAsync(SocketGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var use = await ReplyAsync(":hand_splayed:  | You must mention a user");
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    try { await user.ModifyAsync(x => x.Mute = false).ConfigureAwait(false); } catch { }
                    try { await user.RemoveRoleAsync(await GetMuteRole(user.Guild)).ConfigureAwait(false); } catch { }
                    var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                    await ReplyAsync(":white_check_mark:  | " + Context.User.Mention + " unmuted " + user.Username);
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Purges A User's Last 100 Messages")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Clear(SocketGuildUser user)
        {
            if (user.GuildPermissions.ManageMessages)
            {
                var messages = await Context.Message.Channel.GetMessagesAsync(100).Flatten();

                var result = messages.Where(x => x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));

                await Context.Message.Channel.DeleteMessagesAsync(result);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Clears *x* amount of messages")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Purge([Remainder] int num = 0)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.ManageMessages)
            {
                if (num <= 100)
                {
                    var messagesToDelete = await Context.Channel.GetMessagesAsync(num + 1).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messagesToDelete);
                    if (num == 1)
                    {
                        var use = await Context.Channel.SendMessageAsync(":white_check_mark:  | Deleted 1 message.");
                        await Task.Delay(5000);
                        await use.DeleteAsync();
                    }
                    else
                    {
                        var use = await Context.Channel.SendMessageAsync(":white_check_mark:  | Cleared " + num + " messages.");
                        await Task.Delay(5000);
                        await use.DeleteAsync();
                    }
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = ":x:  | You cannot delete more than 100 messages at once!";
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("promote admin")]
        [Alias("promote Admin", "promo admin" , "Promote administrator")]
        [Summary("Promotes a user to a Administrator")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task PromoteAdmin(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to promote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var admin = user.Guild.Roles.Where(input => input.Name.ToUpper() == "ADMIN").FirstOrDefault() as SocketRole;
                    var mod = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MODERATOR").FirstOrDefault() as SocketRole;
                    await user.AddRoleAsync(admin);
                    await user.RemoveRoleAsync(mod);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " promoted " + user.Mention + " to Admin rank! Congratulations!");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("promote moderator")]
        [Alias("promote mod", "promo mod")]
        [Summary("Promotes a user to a Moderator")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task PromoteModerator(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to promote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var mod = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MODERATOR").FirstOrDefault() as SocketRole;
                    var helper = user.Guild.Roles.Where(input => input.Name.ToUpper() == "HELPER").FirstOrDefault() as SocketRole;

                    await user.AddRoleAsync(mod);
                    await user.RemoveRoleAsync(helper);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " promoted " + user.Mention + " to Moderator rank! Congratulations!");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("promote helper")]
        [Alias("promote helper", "promo helper")]
        [Summary("Promotes a user to a Helper")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task PromoteHelper(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to promote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var helper = user.Guild.Roles.Where(input => input.Name.ToUpper() == "HELPER").FirstOrDefault() as SocketRole;
                    await user.AddRoleAsync(helper);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " promoted " + user.Mention + " to Helper rank! Congratulations!");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("demote moderator")]
        [Alias("demote mod")]
        [Summary("Demotes a user to a Moderator")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task DemoteMod(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to demote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var admin = user.Guild.Roles.Where(input => input.Name.ToUpper() == "ADMIN").FirstOrDefault() as SocketRole;
                    var mod = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MODERATOR").FirstOrDefault() as SocketRole;

                    await user.AddRoleAsync(mod);
                    await user.RemoveRoleAsync(admin);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to Moderator rank due to inappropiate behavior.");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("demote helper")]
        [Summary("Demotes a user to a helper")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task DemoteHelper(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to demote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var helper = user.Guild.Roles.Where(input => input.Name.ToUpper() == "HELPER").FirstOrDefault() as SocketRole;
                    var mod = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MODERATOR").FirstOrDefault() as SocketRole;
                    var admin = user.Guild.Roles.Where(input => input.Name.ToUpper() == "ADMIN").FirstOrDefault() as SocketRole;

                    await user.AddRoleAsync(helper);
                    await user.RemoveRoleAsync(mod);
                    await user.RemoveRoleAsync(admin);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to Helper rank due to inappropiate behavior.");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("demote member")]
        [Summary("Demotes a user to a member")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task DemoteMember(IGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to demote the user to. Ex: w!demote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var admin = user.Guild.Roles.Where(input => input.Name.ToUpper() == "ADMIN").FirstOrDefault() as SocketRole;
                    var mod = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MODERATOR").FirstOrDefault() as SocketRole;
                    var helper = user.Guild.Roles.Where(input => input.Name.ToUpper() == "HELPER").FirstOrDefault() as SocketRole;

                    await user.RemoveRoleAsync(admin);
                    await user.RemoveRoleAsync(mod);
                    await user.RemoveRoleAsync(helper);

                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " due to inappropiate behavior.");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        private static IUser ThisIsMe;

        [Command("dm")]
        [Summary("DMs Phytal (The bot creator), Please do not abuse.")]
        public async Task Dm([Remainder] string dm)
        {
            var myId = Context.User.Mention;
            if (ThisIsMe == null)
            {
                foreach (var user in Context.Guild.GetUsersAsync().Result)
                {
                    if (user.Id == 264897146837270529)
                    {
                        ThisIsMe = user;
                        myId = user.Mention;
                        break;
                    }
                }
            }

            var application = await Context.Client.GetApplicationInfoAsync();
            var message = await application.Owner.GetOrCreateDMChannelAsync();

            var embed = new EmbedBuilder()
            {
                Color = new Color(37, 152, 255)
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message from: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {myId}, he will read the message soon.";

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Warn")]
        [Summary("Warns a User")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task WarnUser(IGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var userAccount = GlobalUserAccounts.GetUserAccount((SocketUser)user);
                var dmchannel = await Context.User.GetOrCreateDMChannelAsync();
                userAccount.NumberOfWarnings++;
                GlobalUserAccounts.SaveAccounts();

                if (userAccount.NumberOfWarnings >= 5)
                {
                    await user.Guild.AddBanAsync(user, 5);
                    await dmchannel.SendMessageAsync($":exclamation:  **You have been banned from {Context.Guild} from having too many warnings.**");
                }
                else if (userAccount.NumberOfWarnings == 3)
                {
                    await user.KickAsync();
                    await dmchannel.SendMessageAsync($":exclamation:  **You have been kicked from {Context.Guild}. Think over your actions and you may rejoin the server once you are ready.**");
                }
                else if (userAccount.NumberOfWarnings == 1)
                {
                    await dmchannel.SendMessageAsync($":exclamation:  **You have been warned in {Context.Guild}. (5 Warnings = Ban)**");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("say")]
        [Summary("Lets you speak for the bot")]
        public async Task Say([Remainder] string input)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var messagesToDelete = await Context.Channel.GetMessagesAsync(1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messagesToDelete);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(input);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        public async Task<IRole> GetMuteRole(IGuild guild)
        {
            const string defaultMuteRoleName = "Muted";

            var muteRoleName = "Muted";

            var muteRole = guild.Roles.FirstOrDefault(r => r.Name == muteRoleName);

            if (muteRole == null)
            {
                try
                {
                    muteRole = await guild.CreateRoleAsync(muteRoleName, GuildPermissions.None).ConfigureAwait(false);
                }
                catch
                {
                    muteRole = guild.Roles.FirstOrDefault(r => r.Name == muteRoleName) ?? await guild.CreateRoleAsync(defaultMuteRoleName, GuildPermissions.None).ConfigureAwait(false);
                }
            }

            foreach (var toOverwrite in (await guild.GetTextChannelsAsync()))
            {
                try
                {
                    if (!toOverwrite.PermissionOverwrites.Any(x => x.TargetId == muteRole.Id && x.TargetType == PermissionTarget.Role))
                    {
                        await toOverwrite.AddPermissionOverwriteAsync(muteRole, denyOverwrite)
                                .ConfigureAwait(false);

                        await Task.Delay(200).ConfigureAwait(false);
                    }
                }
                catch
                {

                }
            }

            return muteRole;
        }


        [Command("Vote")]
        [Summary("Creates a voting poll")]
        public async Task Vote([Remainder] string Input)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Vote Started");
                embed.WithDescription(Input);
                embed.WithFooter($"requested by: {Context.User.Username}");
                embed.WithColor(37, 152, 255);

                var CheckMark = new Emoji("✅");
                var XMark = new Emoji("❌");

                var msg = await Context.Channel.SendMessageAsync("", false, embed);
                await msg.AddReactionAsync(CheckMark);
                await msg.AddReactionAsync(XMark);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", false, embed);
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("FilterOn")]
        public async Task TurnFilteringOn()
        {
            Global.Client.MessageReceived += Filter.Filtering;
            //var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            //await guildAcc.(x => x.Filter = true);
            await Context.Channel.SendMessageAsync(":white_check_mark:  | Filter successfully turned back on. Stay safe!");
        }

        [Command("FilterOff")]
        public async Task TurnFilteringOff()
        {
            Global.Client.MessageReceived -= Filter.Filtering;
            await Context.Channel.SendMessageAsync(":white_check_mark:  | Filter successfully turned off. Daredevil!");
        }

    }
}