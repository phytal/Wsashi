using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Core.LevelingSystem
{
    internal static class Leveling
    {
        internal static async Task UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            var userAccount = GlobalGuildUserAccounts.GetUserID(user);
            var dmchannel = await user.GetOrCreateDMChannelAsync();
            DateTime now = DateTime.UtcNow;

            if (now < userAccount.LastXPMessage.AddSeconds(Constants.MessageXPCooldown))
            {
                return;
            }

            userAccount.LastXPMessage = now;

            uint oldLevel = userAccount.LevelNumber;
            userAccount.XP += 13;
            GlobalGuildUserAccounts.SaveAccounts();
            uint newLevel = userAccount.LevelNumber;
            //var requiredXp = (Math.Pow(newLevel + 1, 2) * 50) - userAccount.XP;
            if (newLevel > oldLevel)
            {
                if (config.LevelingMsgs == "server")
                {
                    await channel.SendMessageAsync($"Level Up! {user.Username}, you just advanced to level {newLevel}!");
                    return;
                }
                if (config.LevelingMsgs == "dm")
                {
                    await channel.SendMessageAsync($"Level Up! {user.Username}, you just advanced to level {newLevel}!");
                    return;
                }
            }
            else return;
        }
    }
}
