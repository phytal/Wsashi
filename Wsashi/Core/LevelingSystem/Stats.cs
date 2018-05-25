using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Core.LevelingSystem
{
    public class StatsModule : ModuleBase<SocketCommandContext>
    {
        [Command("stats")]
        [Summary("Checks your stats (level, xp, reputation)")]
        [Alias("givepoints")]
        public async Task Stats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var userAccount = GlobalUserAccounts.GetUserAccount(target);
            var account = GlobalUserAccounts.GetUserAccount(target);
            uint oldLevel = userAccount.LevelNumber;
            GlobalUserAccounts.SaveAccounts();
            uint newLevel = userAccount.LevelNumber;

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(target.Username);
            embed.AddInlineField("Level", oldLevel);
            embed.AddInlineField("Exp", userAccount.XP);
            embed.AddInlineField("Reputation Points", userAccount.Reputation);

            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }


}
