using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class XPModule : ModuleBase<SocketCommandContext>
    {
        [Command("addXP")]
        [Summary("Grants XP/Exp to selected user")]
        [Alias("givexp", "giveexp", "addexp")]
        [RequireOwner]
        public async Task AddXP(uint xp, IGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user);

            userAccount.XP += xp;
            GlobalGuildUserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($":white_check_mark:  | **{xp}** Exp were added to " + target.Username + "'s account.");
        }

        [Command("addrep")]
        [Summary("Grants reputation points to selected user")]
        [Alias("givepoints")]
        [RequireOwner]
        public async Task AddPoints(uint Points, SocketGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalGuildUserAccounts.GetUserID(user);

            userAccount.Reputation += Points;
            GlobalGuildUserAccounts.SaveAccounts();

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":white_check_mark:  | **{Points}** reputation points were added to " + target.Username + "'s account.");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("xp")]
        [Alias("exp")]
        [Summary("Views the xp of you or an mentioned user Ex: /xp @user or /xp")]
        public async Task Experience([Remainder]string arg = "")
        {
            var user = Context.User as SocketGuildUser;
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            //var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault(u => !u.IsBot);
            target = mentionedUser ?? Context.User;

            var account = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)target);
            var requiredXp = (Math.Pow(account.LevelNumber + 1, 2) * 50);
            var levelXp = (Math.Pow(account.LevelNumber, 2) * 50);

            var thumbnailurl = user.GetAvatarUrl();

            var auth = new EmbedAuthorBuilder()
            {
                Name = user.Username,
                IconUrl = thumbnailurl,
            };

            var embed = new EmbedBuilder()
            {
                Author = auth
            };
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"{target.Username} has {requiredXp - levelXp}/{requiredXp} XP");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("levelsxp")]
        [Summary("Shows a user list of the sorted by XP. Pageable to see lower ranked users.")]
        [Alias("Topxp", "Top10xp", "experienced", "rankxp")]
        public async Task ShowTopXP(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? You know it won't work ");
                return;
            }

            var guildUserIds = Context.Guild.Users.Select(user => user.Id);
            var accounts = GlobalGuildUserAccounts.GetFilteredAccounts(acc => guildUserIds.Contains(acc.Id));

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
            var ordered = accounts.OrderByDescending(acc => acc.XP).ToList();

            var embB = new EmbedBuilder()
                .WithTitle($"Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embB.WithColor(37, 152, 255);
                embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.XP} XP", true);
            }

            await ReplyAsync("", false, embB.Build());
        }

        [Command("level")]
        [Summary("Shows what level you are")]
        public async Task WhatLevelIs([Remainder]string arg = "")
        {
            var user = Context.User as SocketGuildUser;
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)target);
            var application = await Context.Client.GetApplicationInfoAsync();
            var thumbnailurl = user.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()

            {

                Name = user.Username,
                IconUrl = thumbnailurl,

            };

            uint level = (uint)Math.Sqrt(account.XP / 50);

            var embed = new EmbedBuilder
            {
                Color = new Color(37, 152, 255),
                Author = auth
            };

            embed.WithTitle("You are level " + level);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("data")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data Has " + DataStorage.GetPairsCount() + " pairs.");
        }
    }
}

