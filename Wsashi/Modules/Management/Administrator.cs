using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Helpers;

namespace Wsashi.Core.Modules
{
    public class Administrator : WsashiModule
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
        [Summary("Unban A User")]
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

        [Command("Softban"), Alias("Sb")]
        [Summary("Bans then unbans a user.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanThenUnbanUser(SocketGuildUser user)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (MiscHelpers.UserHasRole(Context, config.ModRole))
            {
                var embed = MiscHelpers.CreateEmbed(Context, $"{Context.User.Mention} softbanned <@{user.Id}>, deleting the last 7 days of messages from that user.");
                await MiscHelpers.SendMessage(Context, embed);
                await Context.Guild.AddBanAsync(user, 7);
                await Context.Guild.RemoveBanAsync(user);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await MiscHelpers.SendMessage(Context, embed);
            }

        }

        [Command("IdBan")]
        [Summary("Ban a user by their ID")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserById(ulong userid, [Remainder]string reason = "")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (MiscHelpers.UserHasRole(Context, config.ModRole))
            {
                if (reason == "")
                {
                    reason = $"Banned by {Context.User.Username}#{Context.User.Discriminator}";
                }
                await Context.Guild.AddBanAsync(userid, 7, reason);
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{userid}** was banned";
                    embed.Description = $"**Username: **{userid}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";
                await MiscHelpers.SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await MiscHelpers.SendMessage(Context, embed);
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who and what you want to promote the user to. Ex: w!promote <rank> <@username>");
                    var use = await Context.Channel.SendMessageAsync("", false, embed);
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
                else
                {
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName);;
                    await user.AddRoleAsync(role);
                    await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.AdminRoleName + " rank! Congratulations!");
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName); ;
                    await user.AddRoleAsync(role);
                    await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.ModRoleName + " rank! Congratulations!");
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.HelperRoleName); ;
                    await user.AddRoleAsync(role);
                    await ReplyAsync(":confetti_ball:   | " + Context.User.Mention + " promoted " + user.Mention + " to the " + config.HelperRoleName + " rank! Congratulations!");
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName); ;
                    await user.AddRoleAsync(role);
                    var role2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName); ;
                    await user.RemoveRoleAsync(role2);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to the " + config.HelperRoleName + " rank due to inappropiate behavior.");
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.HelperRoleName);
                    var role2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName);
                    var role3 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName);
                    await user.AddRoleAsync(role);
                    await user.RemoveRoleAsync(role2);
                    await user.RemoveRoleAsync(role3);
                    await ReplyAsync(":exclamation:  | " + Context.User.Mention + " demoted " + user.Mention + " to the " + config.HelperRoleName + " rank due to inappropiate behavior.");
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
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.HelperRoleName);
                    var role2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.ModRoleName);
                    var role3 = Context.Guild.Roles.FirstOrDefault(x => x.Name == config.AdminRoleName);
                    await user.RemoveRoleAsync(role);
                    await user.RemoveRoleAsync(role2);
                    await user.RemoveRoleAsync(role3);

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
        
        /*private static IUser ThisIsMe;

        [Command("dm")]
        [Summary("DMs Phytal (The bot creator), Please do not abuse.")]
        public async Task Dm([Remainder] string dm)
        {
            var myId = Context.User.Mention;
            if (ThisIsMe == null)
            {
                foreach (var user in Context.Guild.().Result)
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
        */

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

        [Command("Filter"), Alias("blacklist", "bl", "fil")]
        [Summary("Turns on or off filter. Usage: w!filter true/false")]
        public async Task SetBoolIntoConfigFilter(bool setting)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.Filter = setting;
            GlobalGuildAccounts.SaveAccounts();
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (setting)
            {
                embed.WithDescription(":white_check_mark:  | Filter successfully turned back on. Stay safe!");
            }
            if (setting == false)
            {
                embed.WithDescription(":white_check_mark:  | Filter successfully turned off. Daredevil!");
            }
            await ReplyAsync("", false, embed);

        }

        [Command("ServerName")]
        [Summary("Changes the name of the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ModifyServerName([Remainder]string name)
        {
            await Context.Guild.ModifyAsync(x => x.Name = name);
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set this server's name to **{name}**!");
            embed.WithColor(37, 152, 255);

            await ReplyAsync("", false, embed);
        }

        [Command("PingChecks"), Alias("Pc")]
        [Summary("Turns on or off mass ping checks. Usage: w!pc true/false")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetBoolToJson(bool arg)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithDescription(arg
                ? "Enabled mass ping checks for this server."
                : "Disabled mass ping checks for this server.");

            config.MassPingChecks = arg;
            GlobalGuildAccounts.SaveAccounts();
            await ReplyAsync("", false, embed);
        }

        [Command("Antilink"), Alias("Al")]
        [Summary("Turns on or off the link filter. Usage: w!al true/false")]
        public async Task SetBoolIntoConfig(bool setting)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.Antilink = setting;
            GlobalGuildAccounts.SaveAccounts();
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (setting)
            {
                embed.WithDescription("Enabled Antilink for this server.");
            }
            if (setting == false)
            {
                embed.WithDescription("Disabled Antilink for this server.");
            }
            await ReplyAsync("", false, embed);

        }

        [Command("AntilinkIgnore"), Alias("Ali")]
        [Summary("Sets a channel that if Antilink is turned on, it will be disabled there")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetChannelToBeIgnored(string type, SocketGuildChannel chnl = null)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            switch (type)
            {
                case "add":
                case "Add":
                    config.AntilinkIgnoredChannels.Add(chnl.Id);
                    GlobalGuildAccounts.SaveAccounts();
                    embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Antilink.");
                    break;
                case "rem":
                case "Rem":
                    config.AntilinkIgnoredChannels.Remove(chnl.Id);
                    GlobalGuildAccounts.SaveAccounts();
                    embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Antilink.");
                    break;
                case "clear":
                case "Clear":
                    config.AntilinkIgnoredChannels.Clear();
                    GlobalGuildAccounts.SaveAccounts();
                    embed.WithDescription("List of channels to be ignored by Antilink has been cleared.");
                    break;
                default:
                    embed.WithDescription($"Valid types are `add`, `rem`, and `clear`. Syntax: `w!ali {{add/rem/clear}} [channelMention]`");
                    break;
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Rename")]
        [Summary("Changes a user's nickname")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder]string nick)
        {
            await user.ModifyAsync(x => x.Nickname = nick);
            var embed = MiscHelpers.CreateEmbed(Context, $"Set <@{user.Id}>'s nickname on this server to **{nick}**!").WithColor(37, 152, 255);
            await MiscHelpers.SendMessage(Context, embed);
        }


        /*[Command("WelcomeMessage"), Alias("Wmsg"), Priority(0)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetTextIntoConfig([Remainder]string msg)
        {
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($"Set this guild's welcome message to:\n\n ```{msg}```");
                config.WelcomeMessage = msg;
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync("", false, embed);

                if (config.WelcomeChannel != 0)
                {
                    var a = config.WelcomeMessage.Replace("{UserMention}", Context.User.Mention);
                    var b = a.Replace("{ServerName}", Context.Guild.Name);
                    var c = b.Replace("{UserName}", Context.User.Username);
                    var d = c.Replace("{OwnerMention}", Context.Guild.Owner.Mention);
                    var e = d.Replace("{UserTag}", Context.User.DiscriminatorValue.ToString());

                    var channel = Context.Guild.GetTextChannel(config.WelcomeChannel);
                    var embed2 = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed2.WithDescription(e);
                    embed2.WithThumbnailUrl(Context.Guild.IconUrl);
                    await channel.SendMessageAsync("", false, embed2);
                }
            }
        }

        [Command("WelcomeMessage"), Alias("Wmsg"), Priority(1)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SendWMSGToUser()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithDescription($"The welcome message for this server is: `{config.WelcomeMessage}`");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("LeavingMessage"), Alias("Lmsg")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetTextIntoConfigLol([Remainder]string msg)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set this guild's leaving message to:\n\n ```{msg}```\n\nSending a test welcome message to <#{config.WelcomeChannel}>");
            config.LeavingMessage = msg;
            GlobalGuildAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("", false, embed);

            if (config.WelcomeChannel != 0)
            {
                var a = config.LeavingMessage.Replace("{UserMention}", Context.User.Mention);
                var b = a.Replace("{ServerName}", Context.Guild.Name);
                var c = b.Replace("{UserName}", Context.User.Username);
                var d = c.Replace("{OwnerMention}", Context.Guild.Owner.Mention);
                var e = d.Replace("{UserTag}", Context.User.DiscriminatorValue.ToString());

                var channel = Context.Guild.GetTextChannel(config.WelcomeChannel);
                var embed2 = new EmbedBuilder();
                embed2.WithDescription(e);
                embed.WithColor(37, 152, 255);
                embed2.WithFooter($"Guild Owner: {Context.Guild.Owner.Username}#{Context.Guild.Owner.Discriminator}");
                embed2.WithThumbnailUrl(Context.Guild.IconUrl);
                await channel.SendMessageAsync("", false, embed2);
            }
        }
        */

        [Command("ServerPrefix")]
        [Summary("Sets teh prefix for the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetGuildPrefix([Remainder]string prefix)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithDescription($"Set server prefix to {prefix}");

            config.CommandPrefix = prefix;
            GlobalGuildAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("ServerLogging"), Alias("Sl", "logging")]
        [Summary("Enables server logging (such as bans, message edits, deletions, kicks, channel additions, etc)")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerLoggingChannel(bool isEnabled)
        {
            var chnl = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
            if (chnl == null)
            {
                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                var perms = new OverwritePermissions(
                    readMessages: PermValue.Deny
                    );
                var channel = await Context.Guild.CreateTextChannelAsync("logs");
                await channel.AddPermissionOverwriteAsync(role, perms);
            }
            var cjhale = chnl as SocketTextChannel;
            string lol;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (isEnabled) { lol = "Enabled server logging"; } else { lol = "Disabled server logging"; }

            config.IsServerLoggingEnabled = isEnabled;
            config.ServerLoggingChannel = cjhale.Id;
            GlobalGuildAccounts.SaveAccounts();
            var embed = MiscHelpers.CreateEmbed(Context, $"{lol}, and set the channel to <#{cjhale.Id}>.").WithColor(37, 152, 255);
            await MiscHelpers.SendMessage(Context, embed);
        }

        [Command("AdminRole")]
        [Summary("Sets the server Admin role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerAdminRole(string roleName)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
            }
            else
            {
                embed.WithDescription($"Set the Administrator role to **{roleName}** for this server!");
                config.AdminRole = role.Id;
                config.AdminRoleName = role.Name;
                GlobalGuildAccounts.SaveAccounts();
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("ModRole")]
        [Summary("Sets the server Moderator role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerModRole(string roleName)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
            }
            else
            {
                embed.WithDescription($"Set the Moderator role to **{roleName}** for this server!");
                config.ModRole = role.Id;
                config.ModRoleName = role.Name;
                GlobalGuildAccounts.SaveAccounts();
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("HelperRole")]
        [Summary("Sets the server Moderator role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetServerHelperRole(string roleName)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
            }
            else
            {
                embed.WithDescription($"Set the Helper role to **{roleName}** for this server!");
                config.HelperRole = role.Id;
                config.HelperRoleName = role.Name;
                GlobalGuildAccounts.SaveAccounts();
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("PingChecks"), Alias("Pc")]
        [Summary("Enables or disables mass ping checks.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetBoolToJsonPing(bool arg)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id); ;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithDescription(arg
                ? "Enabled mass ping checks for this server."
                : "Disabled mass ping checks for this server.");

            config.MassPingChecks = arg;
            GlobalGuildAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("SelfRoleAdd"), Alias("SRA")]
        [Summary("Adds a role a user can add themselves with w!Iam or w!Iamnot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddStringToList([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255)
                .WithDescription($"Added the {role} to the Config.");
            config.SelfRoles.Add(role);
            GlobalGuildAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("SelfRoleRem"), Alias("SRR")]
        [Summary("Removes a Self Role. Users can add a role themselves with w!Iam or w!Iamnot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveStringFromList([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (config.SelfRoles.Contains(role))
            {
                config.SelfRoles.Remove(role);
                embed.WithDescription($"Removed {role} from the Self Roles list.");
            }
            else
            {
                embed.WithDescription("That role doesn't exist in your Guild Config.");
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("SelfRoleClear"), Alias("SRC")]
        [Summary("Clears all Self Roles. Users can add a role themselves with w!Iam or w!Iamnot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClearListFromConfig()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (config == null)
            {
                embed.WithDescription("You don't have a Guild Config created.");
            }
            else
            {
                embed.WithDescription($"Cleared {config.SelfRoles.Count} roles from the self role list.");
                config.SelfRoles.Clear();
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("SelfRoleList"), Summary("Shows all currently set Self Roles")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ListWelcomeMessages()
        {
            var sr = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).SelfRoles;
            var embB = new EmbedBuilder().WithTitle("No Self Roles set yet..");
            if (sr.Count > 0) embB.WithTitle("All Self Roles:");

            for (var i = 0; i < sr.Count; i++)
            {
                embB.AddField($"Self Role #{i + 1}:", sr[i]);
            }
            await ReplyAsync("", false, embB.Build());
        }

        [Command("Leveling"), Alias("L")]
        [Summary("Enables or disables leveling for the server. Use w!leveling <true or false>")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Leveling(bool arg)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithDescription(arg ? "Enabled leveling for this server." : "Disabled leveling for this server.");
            config.Leveling = arg;
            GlobalGuildAccounts.SaveAccounts();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("AutoRole")]
        [Summary("Adds a role that new members will recieve automatically")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AutoRoleRoleAdd([Remainder]string arg = "")
        {

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.Autorole = arg;
            GlobalGuildAccounts.SaveAccounts();

            var embed = new EmbedBuilder();
            embed.WithDescription($"AutoroleCommandText : {arg}");
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            embed.WithColor(37, 152, 255);

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}