using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Wsashi.Preconditions;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Modules.Fun.Dueling;
using Wsashi.Core.Modules;
using Discord.Rest;
using System.Collections.Generic;
using System.Linq;

namespace Wsashi.Modules
{
    public struct PendingDuelBase
    {
        public ulong PlayerId { get; set; }
        public ulong Requester { get; set; }
        public RestUserMessage Message { get; set; }
    }
    public static class PendingDuelProvider
    {
        public static List<PendingDuelBase> games;

        static PendingDuelProvider()
        {
            games = new List<PendingDuelBase>();
        }

        public static bool UserIsPlaying(ulong userId)
        {
            return games.Any(g => g.PlayerId == userId);
        }

        public static ulong RequestUser(ulong userId)
        {
            var game = games.FirstOrDefault(g => g.PlayerId == userId);
            return game.Requester;
        }

        public static void CreateNewGame(ulong userId, ulong req, RestUserMessage message)
        {
            var game = new PendingDuelBase()
            {
                PlayerId = userId,
                Message = message,
                Requester = req 
            };

            games.Add(game);
        }
    }
    public class Duel : WsashiModule
    {
        [Command("duelhelp")]
        [Alias("duelshelp", "helpduel")]
        [Summary("Shows all possible commands for dueling")]
        [Remarks("Ex: w!duel help")]
        public async Task DuelHelp()
        {
            string[] footers = new string[]
{
                "If you want to gain health but still want to do damage, use w!absorb!",
                "You have a maximum of 6 Med Kits you can use in battle, use them wisely!",
                "If you are low on health, use your Med Kits and heal up!",
                                "If you want to deflect some damage from your opponent's next attack, use w!deflect!",
                                "Each duel command has a cooldown of 3 seconds.",
                                "Absorbing is powerful, but it is rare that it can hit the target."
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle(":crossed_swords:  Duel Command List / Help");
            embed.AddField("w!duels help", "Brings up the help commmand (lol)", true);
            embed.AddField("w!duel", "Starts a duel with the confirmation specified user!", true);
            embed.AddField("w!engage", "Enables you to attack, use an item, or give up in a duel!", true);
            embed.AddField("w!duelsBuy", "Opens the duel shop interface.", true);
            embed.AddField("w!duelsInventory", "View all of your items for duels.", true);
            embed.AddField("w!duelsLeaderboard", "Views the dueling leaderboard for your guild.", true);
            embed.AddField("w!globalDuelsLeaderboard", "Views the dueling leaderboard for the world.", true);
            embed.AddField("w!duelStats", "View yours or someone else's stats for duels.", true);
            embed.AddField("w!attacks", "Views your 4 attacks.", true);
            embed.AddField("w!replaceattack", "Replaces an attack.", true);
            embed.AddField("w!blessings", "Shows all of your possessed blessings.", true);
            embed.AddField("w!activeblessing", "Set your active blessing.", true);
            embed.WithFooter(text);
            embed.WithColor(37, 152, 255);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("duel")]
        [Alias("Duel", "dual")]
        [Summary("Starts a duel with the specified user!")]
        [Remarks("w!duel <user you want to duel> Ex: w!duel @Phytal")]
        public async Task Pvp(SocketGuildUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(user.Id);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            if (config.Fighting == false && configg.Fighting == false)
            {
                if (PendingDuelProvider.UserIsPlaying(Context.User.Id))
                {
                    await ReplyAsync("You already sent a duel request to someone. Cancel it with `w!duelcancel` or wait until your opponent accepts your duel request.");
                    return;
                }
                var msg = await Context.Channel.SendMessageAsync($"{Context.User.Mention} challenges {user.Mention} to a duel! {user.Username}, do you accept? (react with the emojis)");
                var emote = Emote.Parse("<:no:453716729525174273>");
                await msg.AddReactionAsync(new Emoji("✅"));
                await msg.AddReactionAsync(emote);
                PendingDuelProvider.CreateNewGame(user.Id, Context.User.Id, msg);
            }
            else
            {
                await ReplyAsync(":expressionless:  | " + Context.User.Mention + ", sorry, either you or your opponent are currently fighting or you just tried to fight yourself...");
            }
        }

        [Command("duelStats")]
        [Alias("ds")]
        [Summary("Gets yours or someone else's duel stats!")]
        [Remarks("w!duelStats <target user (will be defaulted to you if left empty)> Ex: w!duelStats @Phytal")]
        public async Task DuelStats([Remainder] string user = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(target);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"{Context.User.Username}'s Duel Stats :crossed_swords: ");
            embed.AddField("**Wins**", config.Wins);
            embed.AddField("**Losses**", config.Losses);
            embed.AddField("**Win Streak**", config.WinStreak);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        public static async Task StartDuel(ISocketMessageChannel channel, SocketGuildUser user, SocketUser req)
        {
            var config = GlobalUserAccounts.GetUserAccount(req);
            var player2 = user.Guild.GetUser(user.Id);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            configg.OpponentId = req.Id;
            config.OpponentId = user.Id;
            configg.OpponentName = req.Username;
            config.OpponentName = user.Username;
            config.Fighting = true;
            configg.Fighting = true;


            string[] whoStarts = new string[]
            {
                    req.Mention,
                    user.Mention
            };

            Random rand = new Random();

            int randomIndex = rand.Next(whoStarts.Length);
            string text = whoStarts[randomIndex];

            config.whosTurn = text;
            configg.whosTurn = text;
            if (text == req.Mention)
            {
                config.whoWaits = user.Mention;
                configg.whoWaits = user.Mention;
            }
            else
            {
                config.whoWaits = req.Mention;
                configg.whoWaits = req.Mention;
            }
            GlobalUserAccounts.SaveAccounts();
            await channel.SendMessageAsync($":crossed_swords:  | {req.Mention} challenged {user.Mention} to a duel!\n\n**{configg.OpponentName}** has **{config.Health}** health!\n**{config.OpponentName}** has **{configg.Health}** health!\n\n{text}, you go first!");
        }

        [Command("duelsLeaderboard"), Alias("dlb")]
        [Summary("Shows a leaderboard sorted by duel wins. Pageable to see lower ranked users.")]
        [Remarks("w!dlb <page number (if left empty it will default to 1)> Ex: w!dlb 2")]
        [Cooldown(15)]
        public async Task DLB(int page = 1)
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
            // Sort the accounts descending by Duel wins
            var ordered = accounts.OrderByDescending(acc => acc.Wins).ToList();

            var embed = new EmbedBuilder()
                .WithTitle($"Duels Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embed.WithColor(37, 152, 255);
                embed.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Wins} Wins", true);
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("globalDuelsLeaderboard"), Alias("gdlb")]
        [Summary("Shows a global leaderboard sorted by duel wins. Pageable to see lower ranked users.")]
        [Remarks("w!gdlb <page number (if left empty it will default to 1)> Ex: w!gdlb 2")]
        [Cooldown(15)]
        public async Task GDLB(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? ***REALLY?***");
                return;
            }

            var accounts = GlobalUserAccounts.GetAllAccounts();

            const int usersPerPage = 9;
            // Calculate the highest accepted page number => amount of pages we need to be able to fit all users in them
            // (amount of users) / (how many to show per page + 1) results in +1 page more every time we exceed our usersPerPage  
            var lastPageNumber = 1 + (accounts.Count / (usersPerPage + 1));
            if (page > lastPageNumber)
            {
                await ReplyAsync($"There are not that many pages...\nPage {lastPageNumber} is the last one...");
                return;
            }
            // Sort the accounts descending by duel wins
            var ordered = accounts.OrderByDescending(acc => acc.Wins).ToList();

            var embed = new EmbedBuilder()
                .WithTitle($"Global Duels Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embed.WithColor(37, 152, 255);
                embed.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Wins} Wins", true);
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("endfight")]
        [Summary("Ends the fight between users (DEBUG PURPOSES ONLY)")]
        [Remarks("w!endfight")]
        [Cooldown(15)]
        public async Task Endfight()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            var configg = GlobalUserAccounts.GetUserAccount(config.OpponentId);
            await ReplyAsync(":flag_white:  | " + Context.User.Mention + " ended the fight.");
            config.Fighting = false;
            configg.Fighting = false;
            config.Health = 100;
            configg.Health = 100;
            config.OpponentId = 0;
            configg.OpponentId = 0;
            config.OpponentName = null;
            configg.OpponentName = null;
            config.whosTurn = null;
            config.whoWaits = null;
            config.placeHolder = null;
            configg.whosTurn = null;
            configg.whoWaits = null;
            configg.placeHolder = null;
            config.Meds = 6;
            configg.Meds = 6;
            config.Blocking = false;
            configg.Blocking = false;
            configg.Deflecting = false;
            config.Deflecting = false;
            config.Wins += 1;
            configg.Losses += 1;
            GlobalUserAccounts.SaveAccounts();
        }
    }
}

