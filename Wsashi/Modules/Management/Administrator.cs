using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Helpers;
using System.IO;
using Wsashi.Preconditions;

namespace Wsashi.Core.Modules
{
    public class Administrator : WsashiModule
    {
        private static readonly OverwritePermissions denyOverwrite = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, attachFiles: PermValue.Deny);
        DiscordSocketClient _client;

        //(37, 152, 255) is the color code

        [Command("ban")]
        [Summary("Bans a specified user")]
        [Remarks("w!ban <user you want to ban> Ex: w!ban @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task BanAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                try
                {
                    var kb = (Context.Client as DiscordShardedClient).GetChannel(config.ServerLoggingChannel) as SocketTextChannel;
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{user.Username}** was banned";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";

                    await gld.AddBanAsync(user);
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user that has a low enough rank to be banned.");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                return;
            }
        }

        [Command("unban")]
        [Summary("Unban A User")]
        [Remarks("w!unban <user you want to unban> Ex: w!unban @Phytal#8213")]
        [Cooldown(5)]
        public async Task Unban([Remainder]string user2)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.BanMembers)
            {

                var bans = await Context.Guild.GetBansAsync();
                var theUser = bans.FirstOrDefault(x => x.User.ToString().ToLowerInvariant() == user2.ToLowerInvariant());

                await Context.Guild.RemoveBanAsync(theUser.User).ConfigureAwait(false);
                await Context.Channel.SendMessageAsync($":white_check_mark:  | Unbanned {user2}.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Softban"), Alias("Sb")]
        [Summary("Bans then unbans a user.")]
        [Remarks("w!softban <user you want to soft ban> Ex: w!softban @Phytal")]
        [Cooldown(5)]
        public async Task BanThenUnbanUser(SocketGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.BanMembers)
            {
                try
                {
                    var embed = MiscHelpers.CreateEmbed(Context, "Softban", $"{Context.User.Mention} softbanned <@{user.Id}>, deleting the last 7 days of messages from that user.");
                    await MiscHelpers.SendMessage(Context, embed);
                    await Context.Guild.AddBanAsync(user, 7);
                    await Context.Guild.RemoveBanAsync(user);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("IdBan")]
        [Summary("Ban a user by their ID")]
        [Remarks("w!idban <user you want to idban> Ex: w!idban 264897146837270529")]
        [Cooldown(5)]
        public async Task BanUserById(ulong userid, [Remainder]string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.BanMembers)
            {
                try
                {
                    await Context.Guild.AddBanAsync(userid, 7, reason);
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{userid}** was banned";
                    embed.Description = $"**Username: **{userid}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";
                    await MiscHelpers.SendMessage(Context, embed);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must enter a valid user-id");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("kick")]
        [Summary("Kicks @Username")]
        [Remarks("w!kick <user you want to kick> Ex: w!kick @Phytal")]
        [Cooldown(5)]
        public async Task KickAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.KickMembers)
            {
                try
                {
                    var kb = (Context.Client as DiscordShardedClient).GetChannel(config.ServerLoggingChannel) as SocketTextChannel;
                    await user.KickAsync();
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $" {user.Username} has been kicked from {user.Guild.Name}";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Kicked by: **{Context.User.Mention}\n**Reason: **{reason}";
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
                catch
                {
                    await ReplyAndDeleteAsync(":hand_splayed:  | You must mention a valid user", timeout: TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Kick Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("mute")]
        [Summary("Mutes @Username")]
        [Remarks("w!mute <user you want to mute> <reason> Ex: w!mute @Phytal spammed in the no spam channel")]
        [Cooldown(5)]
        public async Task MuteAsync(SocketGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
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
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("unmute")]
        [Summary("Unmutes @Username")]
        [Remarks("w!unmute <user you want to unmute> Ex: w!unmute @Phytal")]
        [Cooldown(5)]
        public async Task UnmuteAsync(SocketGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                try
                {
                    try { await user.ModifyAsync(x => x.Mute = false).ConfigureAwait(false); } catch { }
                    try { await user.RemoveRoleAsync(await GetMuteRole(user.Guild)).ConfigureAwait(false); } catch { }
                    var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                    await ReplyAsync(":white_check_mark:  | " + Context.User.Mention + " unmuted " + user.Username);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user that is muted");
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

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Purges A User's Last 100 Messages")]
        [Remarks("w!clear <user whose messages you want to clear> Ex: w!clear @Phytal")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Clears *x* amount of messages")]
        [Remarks("w!clear <amount of messages you want to clear> Ex: w!clear 10")]
        [Cooldown(5)]
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
                        if (num == 1) await ReplyAndDeleteAsync(":white_check_mark:  | Deleted 1 message."); 
                        else await ReplyAndDeleteAsync(":white_check_mark:  | Cleared " + num + " messages.", timeout: TimeSpan.FromSeconds(5));
                    }
                    else
                    {
                        var embed = new EmbedBuilder();
                        embed.WithColor(37, 152, 255);
                        embed.Title = ":x:  | You cannot delete more than 100 messages at once!";
                        await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                    }
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                    await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
        }

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
        [Remarks("w!warn <user you want to warn> <reason> Ex: w!warn @Phytal bullied my brother")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task WarnUser(IGuildUser user, [Remainder]string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                try
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
                catch
                {
                    if (user == null)
                    {
                        var embed = new EmbedBuilder();
                        embed.WithColor(37, 152, 255);
                        embed.WithTitle(":hand_splayed:  | Please say who you want to warn and a reason for their warning. Ex: w!warn @Phytal bullied my brother");
                        await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                    }
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Warnings")]
        [Summary("Shows all of a user's warnings")]
        [Remarks("w!warnings <user whose warnings you want to look at> Ex: w!warnings @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ClearWarnings")]
        [Summary("Clears all of a user's warnings")]
        [Alias("cw")]
        [Remarks("w!cw <user whose warnings you want to clear> Ex: w!cw @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("say")]
        [Summary("Lets you speak for the bot anonymously")]
        [Remarks("w!say <your message> Ex: w!say whats up my doots")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
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
        [Alias("poll")]
        [Summary("Creates a voting poll")]
        [Remarks("w!vote <what you want to vote on> Ex: w!vote is Phytal good at overwatch")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Filter"), Alias("blacklist", "bl", "fil")]
        [Summary("Turns on or off filter. Usage: w!filter true/false")]
        [Remarks("w!filter <on/off> Ex: w!filter on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfigFilter(string setting)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(setting);
                if (result.Item1 == true)
                {
                    var argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    config.Filter = argg;
                    GlobalGuildAccounts.SaveAccounts();
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? ":white_check_mark:  | Filter successfully turned on. Stay safe!" : ":white_check_mark:  | Filter successfully turned off. Daredevil!");
                    await ReplyAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!filter <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ServerName")]
        [Summary("Changes the name of the server")]
        [Remarks("w!servername <new name of the server> Ex: w!servername roblox is best")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("PingChecks"), Alias("Pc")]
        [Summary("Turns on or off mass ping checks.")]
        [Remarks("w!pc <on/off> Ex: w!pc on")]
        [Cooldown(5)]
        public async Task SetBoolToJson(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg
                        ? "Enabled mass ping checks for this server."
                        : "Disabled mass ping checks for this server.");
                    config.MassPingChecks = argg;
                    GlobalGuildAccounts.SaveAccounts();
                    await ReplyAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!pc <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Antilink"), Alias("Al")]
        [Summary("Turns on or off the link filter.")]
        [Remarks("w!al <on/off> Ex: w!al on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfig(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool setting = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    config.Antilink = setting;
                    GlobalGuildAccounts.SaveAccounts();
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(setting ? "Enabled Antilink for this server." : "Disabled Antilink for this server.");
                    await ReplyAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!al <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("AntilinkIgnore"), Alias("Ali")]
        [Summary("Sets a channel that if Antilink is turned on, it will be disabled there")]
        [Remarks("w!ali <channel you want anti-link to be ignored> Ex: w!ali #links-only")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("FilterIgnore"), Alias("Fi")]
        [Summary("Sets a channel that if Filter is turned on, it will be disabled there")]
        [Remarks("w!fi <channel you want filter to be ignored> Ex: w!fi #nsfw")]
        [Cooldown(5)]
        public async Task SetChannelToBeIgnoredByFilter(string type, SocketGuildChannel chnl = null)
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
                        config.NoFilterChannels.Add(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Filter.");
                        break;
                    case "rem":
                    case "Rem":
                        config.NoFilterChannels.Remove(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Filter.");
                        break;
                    case "clear":
                    case "Clear":
                        config.NoFilterChannels.Clear();
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription("List of channels to be ignored by Filter has been cleared.");
                        break;
                    default:
                        embed.WithDescription($"Valid types are `add`, `rem`, and `clear`. Syntax: `w!fi {{add/rem/clear}} [channelMention]`");
                        break;
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

        [Command("Rename")]
        [Alias("Nick")]
        [Summary("Changes a user's nickname")]
        [Remarks("w!rename <user you want to rename> <desired nickname> Ex: w!rename @Phytal dumb kid")]
        [Cooldown(5)]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder]string nick)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                await user.ModifyAsync(x => x.Nickname = nick);
                var embed = MiscHelpers.CreateEmbed(Context, "User Nicked" , $"Set <@{user.Id}>'s nickname on this server to **{nick}**!").WithColor(37, 152, 255);
                await MiscHelpers.SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

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

        [Command("levelingmsg")]
        [Alias("lvlmsg")]
        [Summary("Sets the way leveling messages are sent")]
        [Remarks("w!lvlmsg <dm/server> Ex: w!lvlmsg dm")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("lockchannel"), Alias("lc")]
        [Summary("Locks the current channel (users will be unable to send messages, only admins)")]
        [Remarks("w!lc")]
        [Cooldown(5)]
        public async Task LockChannel()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var chnl = Context.Channel as ITextChannel;
                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                var perms = new OverwritePermissions(
                    sendMessages: PermValue.Deny
                    );
                await chnl.AddPermissionOverwriteAsync(role, perms);

                var embed = MiscHelpers.CreateEmbed(Context, "Channel Locked", $":lock: Locked {Context.Channel.Name}.");
                await MiscHelpers.SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("unlockchannel"), Alias("ulc")]
        [Summary("Unlocks the current channel (users can send messages again)")]
        [Remarks("w!ulc")]
        [Cooldown(5)]
        public async Task UnlockChannel()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var chnl = Context.Channel as ITextChannel;
                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                var perms = new OverwritePermissions(
                    sendMessages: PermValue.Allow
                    );
                await chnl.AddPermissionOverwriteAsync(role, perms);

                var embed = MiscHelpers.CreateEmbed(Context, "Channel Unlocked", $":unlock: Unlocked {Context.Channel.Name}.").WithColor(37, 152, 255);
                await MiscHelpers.SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ServerLogging"), Alias("Sl", "logging")]
        [Summary("Enables server logging (such as bans, message edits, deletions, kicks, channel additions, etc)")]
        [Remarks("w!sl <on/off> Ex: w!sl on")]
        [Cooldown(5)]
        public async Task SetServerLoggingChannel(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool setting = result.Item2;
                    var chnl = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
                    if (chnl == null)
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                        var perms = new OverwritePermissions(
                            viewChannel: PermValue.Deny
                            );
                        var channell = await Context.Guild.CreateTextChannelAsync("logs");
                        await channell.AddPermissionOverwriteAsync(role, perms);
                    }
                    var chnl1 = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
                    var channel = chnl1 as SocketTextChannel;
                    string lol;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    if (setting) { lol = "Enabled server logging"; } else { lol = "Disabled server logging"; }
                    ulong id = channel.Id;
                    config.IsServerLoggingEnabled = setting;
                    config.ServerLoggingChannel = id;
                    GlobalGuildAccounts.SaveAccounts();
                    var embed = MiscHelpers.CreateEmbed(Context, "Server Logging", $"{lol}, and set the channel to <#{channel.Id}>.").WithColor(37, 152, 255);
                    await MiscHelpers.SendMessage(Context, embed);
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!sl <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ModRole")]
        [Summary("Sets the server Moderator role")]
        [Remarks("w!ModRole <mod role name> Ex: w!ModRole Moderator")]
        [Cooldown(5)]
        public async Task SetServerModRole([Remainder]string roleName)
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("HelperRole")]
        [Summary("Sets the server Moderator role")]
        [Remarks("w!HelperRole <helper role name> Ex: w!HelperRole Helper")]
        [Cooldown(5)]
        public async Task SetServerHelperRole([Remainder]string roleName)
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SelfRoleAdd"), Alias("SRA")]
        [Summary("Adds a role a user can add themselves with w!Iam or w!Iamnot")]
        [Remarks("w!sra <role you want to be available> Ex: w!sra Member")]
        [Cooldown(5)]
        public async Task AddStringToList([Remainder]string role)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255)
                    .WithDescription($"Added the {role} to the Config.");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
                config.SelfRoles.Add(role);
                GlobalGuildAccounts.SaveAccounts();
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SelfRoleRem"), Alias("SRR")]
        [Summary("Removes a Self Role. Users can add a role themselves with w!Iam or w!Iamnot")]
        [Remarks("w!srr <self role you want to be removed> Ex: w!srr Member")]
        [Cooldown(5)]
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
                    GlobalGuildAccounts.SaveAccounts();
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SelfRoleClear"), Alias("SRC")]
        [Summary("Clears all Self Roles. Users can add a role themselves with w!Iam or w!Iamnot")]
        [Remarks("w!src")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("syncguild")]
        [Summary("Syncs the current guild information with the database")]
        [Remarks("w!syncguild")]
        [Cooldown(5)]
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
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Leveling"), Alias("L")]
        [Summary("Enables or disables leveling for the server.")]
        [Remarks("w!leveling <on/off> Ex: w!leveling on")]
        [Cooldown(5)]
        public async Task Leveling(string arg)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? "Enabled leveling for this server." : "Disabled leveling for this server.");
                    config.Leveling = argg;
                    GlobalGuildAccounts.SaveAccounts();

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!leveling <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("unflip"), Alias("uf")]
        [Summary("Enables or disables unflipping reactions for the server.")]
        [Remarks("w!uf <on/off> Ex: w!uf on")]
        [Cooldown(5)]
        public async Task Unflip(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? "I'll maintain your anger! **(Enabled unflipping for this server)**" : "You may freely rampage at your own will. **(Disabled unflipping for this server)**");
                    config.Unflip = argg;
                    GlobalGuildAccounts.SaveAccounts();

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!uf <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("AutoRole")]
        [Summary("Adds a role that new members will recieve automatically")]
        [Remarks("w!autorole <role name> Ex: w!autorole Member")]
        [Cooldown(5)]
        public async Task AutoRoleRoleAdd(string arg = "")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                if (arg == null) await ReplyAndDeleteAsync("Please include the name of the role you want to autorole");
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.Autorole = arg;
                GlobalGuildAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithDescription($"Added the **{arg}** role to Autorole!");
                embed.WithColor(37, 152, 255);
                embed.WithFooter("Make sure that Wsashi has a higher role than the autoroled role!");

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

        [Command("SlowMode")]
        [Summary("Adds a slowmode to the entire server (usually for large servers)")]
        [Remarks("w!slowmode <length between messages> Ex: w!slowmode 5")]
        [Cooldown(5)]
        public async Task SlowMode(ulong length)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.IsSlowModeEnabled = true;
                config.SlowModeCooldown = length;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":snail:  | Successfully turned on Slowmode for **{length}** seconds.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SlowModeOff")]
        [Summary("Disables Slowmode")]
        [Remarks("w!slowmodeoff")] //        [Remarks("w! <> Ex: w!")]
        [Cooldown(5)]
        public async Task SlowModeOff(ulong length)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.IsSlowModeEnabled = false;
                config.SlowModeCooldown = 0;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":snail:  | Successfully turned off Slowmode.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("BlacklistAdd")]
        [Alias("Bladd")]
        [Summary("Add a word to the filter")]
        [Remarks("w!bladd <word you want to add> Ex: w!bladd gay")]
        [Cooldown(5)]
        public async Task AddStringToBl([Remainder]string word)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:  | Added **{word}** to the Blacklist.");

                config.CustomFilter.Add(word);
                GlobalGuildAccounts.SaveAccounts();
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("BlacklistRemove")]
        [Alias("Blrem")]
        [Summary("Remove a custom word from filter")]
        [Remarks("w!blrem <word you want to remove> Ex: w!blrem gay")]
        [Cooldown(5)]
        public async Task RemoveStringFromBl([Remainder] string bl)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                if (!config.CustomFilter.Contains(bl))
                {
                    embed.WithDescription($"`{bl}` isn't present in the Blacklist.");
                }
                else
                {
                    embed.WithDescription($"Removed {bl} from the Blacklist.");
                    config.CustomFilter.Remove(bl);
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

        [Command("BlacklistClear")]
        [Alias("Blcl")]
        [Summary("Clears the custom words in the filter")]
        [Remarks("w!blcl")]
        [Cooldown(5)]
        public async Task ClearBlacklist()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.CustomFilter.Clear();
                GlobalGuildAccounts.SaveAccounts();
                var embed = new EmbedBuilder();
                embed.WithDescription("Cleared the Blacklist for this server.");
                embed.WithColor(37, 152, 255);

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

        [Command("Blacklistlist")]
        [Alias("Bll")]
        [Summary("Lists the custom words in the filter")]
        [Remarks("w!bll")]
        [Cooldown(5)]
        public async Task ListBlacklist()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                string list = string.Empty;
                foreach(var word in config.CustomFilter)
                {
                    list += $"{word}  ";
                }
                var embed = new EmbedBuilder();
                embed.WithTitle($"Blacklisted words in {Context.Guild.Name}");
                embed.WithDescription(list);
                embed.WithColor(37, 152, 255);

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

        [Command("CustomCommandAdd")]
        [Alias("Cca")]
        [Summary("Add a custom command")]
        [Remarks("w!cca <command name> <bot response> Ex: w!cca whatsup hey man")]
        [Cooldown(5)]
        public async Task AddCustomCommand(string commandName, [Remainder] string commandValue)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.CustomCommands.Add(commandName, commandValue);
                GlobalGuildAccounts.SaveAccounts();
                var embed = new EmbedBuilder()
                    .WithTitle("Custom Command Added!")
                    .AddField("Command Name", $"__{commandName}__")
                    .AddField("Bot Response", $"**{commandValue}**")
                    .WithColor(37, 152, 255);

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

        [Command("CustomCommandRemove")]
        [Alias("Ccr")]
        [Summary("Remove a custom command")]
        [Remarks("w!ccr <custom command name you want to remove> Ex: w!ccr whatsup")]
        public async Task RemCustomCommand(string commandName)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255);
                if (config.CustomCommands.Keys.Contains(commandName))
                {
                    embed.WithDescription($"Removed **{commandName}** as a command!");
                    config.CustomCommands.Remove(commandName);
                    GlobalGuildAccounts.SaveAccounts();
                }
                else
                {
                    embed.WithDescription($"**{commandName}** isn't a command on this server.");
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

        [Command("CustomCommandList")]
        [Alias("Ccl")]
        [Summary("Lists all custom commands and its outputs")]
        [Remarks("w!ccr <custom command name you want to remove> Ex: w!ccr whatsup")]
        public async Task CustomCommandList()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var cmds = config.CustomCommands;
            var embed = new EmbedBuilder().WithTitle("No custom commands set up yet... add some!");
            if (cmds.Count > 0) embed.WithTitle("Here are all available custom commands:");

            foreach (var cmd in cmds)
            {
                embed.AddField(cmd.Key, cmd.Value, true);
            }

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}