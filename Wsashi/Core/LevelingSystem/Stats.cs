using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Core.LevelingSystem
{
    public class StatsModule : ModuleBase<SocketCommandContext>
    {
        [Command("stats")]
        [Summary("Checks your stats (level, xp, reputation)")]
        [Alias("userstats")]
        [Remarks("w!stats <person you want to check(will default to you if left empty)> Ex: w!stats @Phytal")]
        [Cooldown(10)]
        public async Task Stats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)target);
            uint Level = userAccount.LevelNumber;
            GlobalGuildUserAccounts.SaveAccounts();
            var xp = userAccount.XP;
            var requiredXp = (Math.Pow(userAccount.LevelNumber + 1, 2) * 50);

            var thumbnailurl = target.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()
            {
                Name = target.Username,
                IconUrl = thumbnailurl,
            };

            var embed = new EmbedBuilder()
            {
                Author = auth
            };

            embed.WithColor(37, 152, 255);
            embed.AddField("Lvl.", Level, true);
            embed.AddField("Exp.", $"{xp}/{requiredXp} (tot. {userAccount.XP})", true);
            embed.AddField("Reputation Points", userAccount.Reputation, true);

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }


}
