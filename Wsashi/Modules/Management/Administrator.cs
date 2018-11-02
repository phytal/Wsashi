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
using System.IO;

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
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("Softban"), Alias("Sb")]
        [Summary("Bans then unbans a user.")]
        public async Task BanThenUnbanUser(SocketGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.BanMembers)
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("IdBan")]
        [Summary("Ban a user by their ID")]
        public async Task BanUserById(ulong userid, [Remainder]string reason = "")
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.BanMembers)
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
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
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Kick Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("mute")]
        [Summary("Mutes @Username")]
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
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("unmute")]
        [Summary("Unmutes @Username")]
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Purges A User's Last 100 Messages")]
        public async Task Clear(SocketGuildUser user)
        {
            if (user.GuildPermissions.ManageMessages)
            {
                var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
                var result = messages.Where(x => x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
                if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(result);

            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
            if (Context.Channel is ITextChannel text)
            {
                var user = Context.User as SocketGuildUser;
                if (user.GuildPermissions.ManageMessages)
                {
                    if (num <= 100)
                    {
                        var messagesToDelete = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
                        if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(messagesToDelete);
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
                        var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                        await Task.Delay(5000);
                        await use.DeleteAsync();
                    }
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await Task.Delay(5000);
                    await use.DeleteAsync();
                }
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                    var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
            await message.SendMessageAsync("", embed: embed.Build());
            embed.Description = $"You have sent a message to {myId}, he will read the message soon.";

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        */

        [Command("Warn")]
        [Summary("Warns a User")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task WarnUser(IGuildUser user, string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user);
                var dmchannel = await user.GetOrCreateDMChannelAsync();
                userAccount.NumberOfWarnings++;
                userAccount.Warnings.Add(reason);
                GlobalGuildUserAccounts.SaveAccounts();

                if (userAccount.NumberOfWarnings >= 5)
                {
                    await user.Guild.AddBanAsync(user);
                    try
                    {
                        await dmchannel.SendMessageAsync($":exclamation:  **You have been banned from** ***{Context.Guild}*** ** from having too many warnings.**");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been banned from** ***{Context.Guild}*** ** from having too many warnings.** \n*This message was shown in a server text channel because you had DMs turned off.*");
                    }
                    await Context.Channel.SendMessageAsync($"Successfully warned and banned**{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                }
                else if (userAccount.NumberOfWarnings == 3 || userAccount.NumberOfWarnings == 4)
                {
                    await user.KickAsync();
                    try
                    {
                        await dmchannel.SendMessageAsync($":exclamation:  **You have been kicked from** ***{Context.Guild}*** **. Think over your actions and you may rejoin the server once you are ready. (5 Warnings = Ban)**");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been kicked from** ***{Context.Guild}*** **. Think over your actions and you may rejoin the server once you are ready. (5 Warnings = Ban)** \n*This message was shown in a server text channel because you had DMs turned off.*");
                    }
                    await Context.Channel.SendMessageAsync($"Successfully warned and kicked **{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                }
                else if (userAccount.NumberOfWarnings == 1 || userAccount.NumberOfWarnings == 2)
                {
                    try
                    {
                        await dmchannel.SendMessageAsync($":exclamation:  **You have been warned in** ***{Context.Guild}*** **. (5 Warnings = Ban)**");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been warned in** ***{Context.Guild}*** **. (5 Warnings = Ban)**\n*This message was shown in a server text channel because you had DMs turned off.*");
                    }
                    await Context.Channel.SendMessageAsync($"Successfully warned **{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("Warnings")]
        [Summary("Shows all of a user's warnings")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Warnings(IGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                var num = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user).NumberOfWarnings;
                var warnings = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user).Warnings;
                var embed = new EmbedBuilder();
                embed.WithTitle($"{user}'s Warnings");
                embed.WithDescription($"Total of **{num}** warnings");
                for (var i = 0; i < warnings.Count; i++)
                {
                    embed.AddField($"Warning #{i + 1}: ", warnings[i], true);
                }
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("ClearWarnings")]
        [Summary("Clears all of a user's warnings")]
        [Alias("cw")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task ClearWarnings(IGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user);
                userAccount.NumberOfWarnings = 0;
                userAccount.Warnings.Clear();
                GlobalGuildUserAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":white_check_mark:  Succesfully cleared all of **{user.Username}'s** warnings.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("say")]
        [Summary("Lets you speak for the bot anonymously")]
        public async Task Say([Remainder] string input)
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (user.GuildPermissions.Administrator)
            {
                if (config.MassPingChecks == true)
                {
                    if (input.Contains("@everyone") || input.Contains("@here")) return;
                }

                var messagesToDelete = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
                if (Context.Channel is ITextChannel text) await text.DeleteMessagesAsync(messagesToDelete);
                //input.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
                await Context.Channel.SendMessageAsync(input);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
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

                var msg = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await msg.AddReactionAsync(CheckMark);
                await msg.AddReactionAsync(XMark);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("Filter"), Alias("blacklist", "bl", "fil")]
        [Summary("Turns on or off filter. Usage: w!filter true/false")]
        public async Task SetBoolIntoConfigFilter(bool setting)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
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
                await ReplyAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("ServerName")]
        [Summary("Changes the name of the server")]
        public async Task ModifyServerName([Remainder]string name)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                await Context.Guild.ModifyAsync(x => x.Name = name);
                var embed = new EmbedBuilder();
                embed.WithDescription($"Set this server's name to **{name}**!");
                embed.WithColor(37, 152, 255);

                await ReplyAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("PingChecks"), Alias("Pc")]
        [Summary("Turns on or off mass ping checks. Usage: w!pc true/false")]
        public async Task SetBoolToJson(bool arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription(arg
                    ? "Enabled mass ping checks for this server."
                    : "Disabled mass ping checks for this server.");

                config.MassPingChecks = arg;
                GlobalGuildAccounts.SaveAccounts();
                await ReplyAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("Antilink"), Alias("Al")]
        [Summary("Turns on or off the link filter. Usage: w!al true/false")]
        public async Task SetBoolIntoConfig(bool setting)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
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
                await ReplyAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("AntilinkIgnore"), Alias("Ali")]
        [Summary("Sets a channel that if Antilink is turned on, it will be disabled there")]
        public async Task SetChannelToBeIgnored(string type, SocketGuildChannel chnl = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
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

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("Rename")]
        [Summary("Changes a user's nickname")]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder]string nick)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                await user.ModifyAsync(x => x.Nickname = nick);
                var embed = MiscHelpers.CreateEmbed(Context, $"Set <@{user.Id}>'s nickname on this server to **{nick}**!").WithColor(37, 152, 255);
                await MiscHelpers.SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
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
                await Context.Channel.SendMessageAsync("", embed: embed.Build());

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
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
            await Context.Channel.SendMessageAsync("", embed: embed.Build());

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
        [Summary("Sets the prefix for the server")]
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("levelingmsg")]
        [Alias("lvlmsg")]
        [Summary("Sets the way leveling messages are sent")]
        [Remarks("w!lvlmsg <dm/server> Ex: w!lvlmsg dm")]
        public async Task SetLvlingMsgStatus([Remainder]string preset)
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.Administrator)
            {
                if (config.Leveling == false)
                {
                    await Context.Channel.SendMessageAsync("You need to enable leveling on this server first!");
                    return;
                }
                if (preset == "dm" || preset == "server")
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($"Set leveling messages to {preset}");

                    config.LevelingMsgs = preset;
                    GlobalGuildAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Make sure you set it to either `dm` or `server`! Ex:w!lvlmsg dm");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("ServerLogging"), Alias("Sl", "logging")]
        [Summary("Enables server logging (such as bans, message edits, deletions, kicks, channel additions, etc)")]
        public async Task SetServerLoggingChannel(bool isEnabled)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var chnl = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
                if (chnl == null)
                {
                    var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                    var perms = new OverwritePermissions(
                        viewChannel: PermValue.Deny
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
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("AdminRole")]
        [Summary("Sets the server Admin role")]
        public async Task SetServerAdminRole(string roleName)
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
                    embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("ModRole")]
        [Summary("Sets the server Moderator role")]
        public async Task SetServerModRole(string roleName)
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
                    embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("HelperRole")]
        [Summary("Sets the server Moderator role")]
        public async Task SetServerHelperRole(string roleName)
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
                    embed.WithDescription($"The role `{roleName}` doesn't exist on this server. Remember that this command is cAsE sEnSiTiVe.");
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
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("SelfRoleAdd"), Alias("SRA")]
        [Summary("Adds a role a user can add themselves with w!Iam or w!Iamnot")]
        public async Task AddStringToList([Remainder]string role)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255)
                    .WithDescription($"Added the {role} to the Config.");
                config.SelfRoles.Add(role);
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("SelfRoleRem"), Alias("SRR")]
        [Summary("Removes a Self Role. Users can add a role themselves with w!Iam or w!Iamnot")]
        public async Task RemoveStringFromList([Remainder]string role)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
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
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("SelfRoleClear"), Alias("SRC")]
        [Summary("Clears all Self Roles. Users can add a role themselves with w!Iam or w!Iamnot")]
        public async Task ClearListFromConfig()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
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

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("SelfRoleList"), Summary("Shows all currently set Self Roles")]
        public async Task SelfRoleList()
        {
            var sr = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).SelfRoles;
            var embB = new EmbedBuilder().WithTitle("No Self Roles set yet..");
            if (sr.Count > 0) embB.WithTitle("All Self Roles:");

            for (var i = 0; i < sr.Count; i++)
            {
                embB.AddField($"Self Role #{i + 1}:", sr[i], true);
            }
            await ReplyAsync("", false, embB.Build());
        }

        [Command("syncguild")]
        public async Task SyncGuild()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var info = System.IO.Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.ServerUserAccountsFolder));
                ulong In = Context.Guild.Id;
                string Out = Convert.ToString(In);
                if (!Directory.Exists(Out))
                    Directory.CreateDirectory(Path.Combine(Constants.ServerUserAccountsFolder, Out));

                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.GuildOwnerId = Context.Guild.Owner.Id;
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync($"Successfully synced the Guild's owner to <@{Context.Guild.OwnerId}>!");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            }
        }

        [Command("Leveling"), Alias("L")]
        [Summary("Enables or disables leveling for the server. Use w!leveling <true or false>")]
        public async Task Leveling(bool arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription(arg ? "Enabled leveling for this server." : "Disabled leveling for this server.");
                config.Leveling = arg;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("unflip"), Alias("uf")]
        [Summary("Enables or disables unflipping reactions for the server. Use w!uf <true or false>")]
        public async Task Unflip(bool arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription(arg ? "I'll maintain your anger! **(Enabled unflipping for this server)**" : "You may freely rampage at your own will. **(Disabled unflipping for this server)**");
                config.Unflip = arg;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("AutoRole")]
        [Summary("Adds a role that new members will recieve automatically")]
        public async Task AutoRoleRoleAdd([Remainder]string arg = "")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.Autorole = arg;
                GlobalGuildAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithDescription($"AutoroleCommandText : {arg}");
                embed.WithThumbnailUrl(Context.Guild.IconUrl);
                embed.WithColor(37, 152, 255);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }
    }
}