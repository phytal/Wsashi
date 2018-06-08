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
            var userAccount = GlobalUserAccounts.GetUserAccount((SocketUser)user);

            userAccount.XP += xp;
            GlobalUserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($":white_check_mark:  | **{xp}** Exp were added to " + target.Username + "'s account.");
        }

        [Command("addrep")]
        [Summary("Grants reputation points to selected user")]
        [Alias("givepoints")]
        [RequireOwner]
        public async Task AddPoints(uint Points, IGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalUserAccounts.GetUserAccount((SocketUser)user);

            userAccount.Reputation += Points;
            GlobalUserAccounts.SaveAccounts();

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":white_check_mark:  | **{Points}** reputation points were added to " + target.Username + "'s account.");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("xp")]
        [Summary("Views the xp of you or an mentioned user Ex: /xp @user or /xp")]
        public async Task Experience([Remainder]string arg = "")
        {
            var user = Context.User as SocketGuildUser;
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            //var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault(u => !u.IsBot);
            target = mentionedUser ?? Context.User;

            var account = GlobalUserAccounts.GetUserAccount(target);

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
            embed.WithTitle($"{target.Username} has {account.XP} XP");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("level")]
        [Summary("Shows what level you are")]
        public async Task WhatLevelIs()
        {
            var user = Context.User as SocketGuildUser;
            var account = GlobalUserAccounts.GetUserAccount(Context.User);
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
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("data")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data Has " + DataStorage.GetPairsCount() + " pairs.");
        }
    }
}

