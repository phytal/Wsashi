using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wsashi.Economy;
using Wsashi.Entities;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;
using DiscordBotsList.Api;
using Wsashi.Core.Modules;

namespace Wsashi.Core.LevelingSystem
{
    public class Economy : WsashiModule
    {
        [Command("Daily")]
        [Alias("GetDaily", "ClaimDaily")]
        [Summary("Claims your daily Potatoes!")]
        [Remarks("Ex: w!daily")]
        [Cooldown(60)]
        public async Task GetDaily()
        {
            var result = Daily.GetDaily(Context.User.Id);

            if (result.Success)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($":potato:  | Here's **{Constants.DailyMoneyGain}** Potatoes, {Context.User.Mention}! Come back tomorrow for more!");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var timeSpanString = string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", result.RefreshTimeSpan);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($":potato:  | **You have already claimed your free daily Potatoes, {Context.User.Mention}.\nCome back in {timeSpanString}.**");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        [Command("Reputation")]
        [Alias("rep")]
        [Summary("Gives a mentioned user reputation points, you can only use this once every 24 hours.")]
        [Remarks("w!rep <person you want to rep> Ex: w!rep @Phytal")]
        [Cooldown(30)]
        public async Task GetRep(SocketGuildUser userB)
        {
            if (Context.User.Id == userB.Id)
            {
                await Context.Channel.SendMessageAsync("You can't use this command on yourself");
            }
            else
            {
                var result = Daily.GetRep((SocketGuildUser)Context.User);

                if (result.Success)
                {
                    var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
                    var mentionedaccount = GlobalGuildUserAccounts.GetUserID(userB);
                    mentionedaccount.Reputation += 1;
                    GlobalGuildUserAccounts.SaveAccounts();
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($":diamond_shape_with_a_dot_inside:   | {Context.User.Mention} gave {userB.Mention} a reputation point!");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var timeSpanString = string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", result.RefreshTimeSpan);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($":diamond_shape_with_a_dot_inside::arrows_counterclockwise:  | **You already gave someone reputation points recently, {Context.User.Mention}.\nCome back in {timeSpanString}.**");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }

        [Command("gift")]
        [Alias("grant", "pay")]
        [Summary("Gifts/Pays Potatoes to a selected user (of course taken from your balance) Ex: w!gift <amount of Potatoes> @user")]
        [Remarks("w!gift <amount> <user you want to gift to> Ex: w!")]
        [Cooldown(10)]
        public async Task Gift(uint Money, IGuildUser userB, [Remainder]string arg = "")
        {
            var giveaccount = GlobalUserAccounts.GetUserAccount(Context.User);

            if (giveaccount.Money < Money)
            {
                await ReplyAsync(":angry:  | Stop trying to gift an amount of Potatoes over your account balance! ");
            }
            else
            {
                if (userB == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle(":hand_splayed:  | Please say who you want to gift Potatoes to. Ex: w!gift <amount of Potatoes> @user");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    SocketUser target = null;
                    var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
                    target = mentionedUser ?? Context.User;

                    var minusaccount = GlobalUserAccounts.GetUserAccount((SocketUser)userB);

                    giveaccount.Money -= Money;
                    minusaccount.Money += Money;
                    GlobalUserAccounts.SaveAccounts();

                    await Context.Channel.SendMessageAsync($":white_check_mark:  | " + Context.User.Mention + "has gifted " + userB.Mention + Money + " " + "Potatoes. How generous.");
                }
            }
        }

        [Command("addPotatos")]
        [Summary("Grants Potatoes to selected user")]
        [Alias("giveppotatos")]
        [RequireOwner]
        public async Task AddPotatos(uint Money, IGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalUserAccounts.GetUserAccount((SocketUser)user);

            userAccount.Money += Money;
            GlobalUserAccounts.SaveAccounts();

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":white_check_mark:  | **{Money}** Potatoes were added to " + target.Username + "'s account.");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("levels")]
        [Summary("Shows a user list of the sorted by Potatoes. Pageable to see lower ranked users.")]
        [Alias("Top", "Top10", "richest", "rank")]
        [Remarks("w!level <page number (if left empty it will default to 1)> Ex: w!levels 2")]
        [Cooldown(15)]
        public async Task ShowRichesPeople(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? ***REALLY?***");
                return;
            }

            var guildUserIds = Context.Guild.Users.Select(user => user.Id);
            var accounts = GlobalUserAccounts.GetFilteredAccounts(acc => guildUserIds.Contains(acc.Id));

            const int usersPerPage = 9;
            // Calculate the highest accepted page number => amount of pages we need to be able to fit all users in them
            // (amount of users) / (how many to show per page + 1) results in +1 page more every time we exceed our usersPerPage  
            var lastPageNumber = 1 + (accounts.Count / (usersPerPage + 1));
            if (page > lastPageNumber)
            {
                await ReplyAsync($"There are not that many pages...\nPage {lastPageNumber} is the last one...");
                return;
            }
            // Sort the accounts descending by Potatoes
            var ordered = accounts.OrderByDescending(acc => acc.Money).ToList();

            var embB = new EmbedBuilder()
                .WithTitle($"Potato Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embB.WithColor(37, 152, 255);
                embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Money} Potatoes", true);
            }

            await ReplyAsync("", false, embB.Build());
        }

        [Command("balance")]
        [Alias("Cash", "Money", "bal")]
        [Summary("Checks the balance for your, or an mentioned account")]
        [Remarks("w!bal <person you want to check (will default to you if left empty)> Ex: w!bal @Phytal")]
        [Cooldown(10)]
        public async Task CheckPotatos([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = GlobalUserAccounts.GetUserAccount(target);
            await ReplyAsync(GetPotatosReport(account.Money, target.Username, target.Mention));
        }

        public string GetPotatosReport(ulong Potatoes, string mention, string mentionn)
        {
            return $":potato:  | {mention} has **{Potatoes} Potatoes**! {GetPotatoCountReaction(Potatoes, mention)}";
        }

        private string GetPotatoCountReaction(ulong value, string mention)
        {

            if (value > 100000)
            {
                return $"Holy sh!t, **{mention}**! You're either cheating or you're really dedicated.";
            }
            else if (value > 50000)
            {
                return $"Damn, you must be here often, **{mention}**. Do you have a crush on me or something?";
            }
            else if (value > 20000)
            {
                return $"That's enough to buy a house... In Wsashi land... \n\nIt's a real place, shut up, **{mention}**!";
            }
            else if (value > 10000)
            {
                return $"**{mention}** is kinda getting rich. Do we rob them or what?";
            }
            else if (value > 5000)
            {
                return $"Is it just me or is **{mention}** taking this economy a little too seriously?";
            }
            else if (value > 2500)
            {
                return $"Great, **{mention}!** Now you can give all those Potatoes to your superior mistress, ME.";
            }
            else if (value > 1100)
            {
                return $"**{mention}** is showing their wealth on the internet again.";
            }
            else if (value > 800)
            {
                return $"Alright, **{mention}**. Put the Potatoes in the bag and nobody gets hurt.";
            }
            else if (value > 550)
            {
                return $"I like how **{mention}** thinks that's impressive.";
            }
            else if (value > 200)
            {
                return $"Ouch, **{mention}**! If I knew that is all you've got, I would've just DM'd you the amount! Embarrassing!";
            }
            else if (value == 0)
            {
                return $"Yeah, **{mention}** is broke. What a surprise.";
            }

            return $"The whole concept of Potatoes is fake. I hope you know that";
        }
    }
}