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
        internal static async Task Level(SocketGuildUser user, SocketTextChannel channel)
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
            if (oldLevel != newLevel)
            {
                if (config.LevelingMsgs == "server")
                {
                    await channel.SendMessageAsync($"Level Up! {user.Username}, you just advanced to level {newLevel}!");
                    return;
                }
                if (config.LevelingMsgs == "dm")
                {
                    await dmchannel.SendMessageAsync($"Level Up! {user.Username}, you just advanced to level {newLevel}!");
                    return;
                }
            }
            else return;
        }
        internal static async Task WsashiLevel(SocketGuildUser user, SocketTextChannel channel)
        {
            var userAccount = GlobalUserAccounts.GetUserAccount(user);
            DateTime now = DateTime.UtcNow;

            if (now < userAccount.LastXPMessage.AddSeconds(Constants.MessageXPCooldown))
            {
                return;
            }

            userAccount.LastXPMessage = now;

            uint oldLevel = userAccount.LevelNumber;
            userAccount.XP += 7;
            GlobalUserAccounts.SaveAccounts();
            uint newLevel = userAccount.LevelNumber;
            if (oldLevel != newLevel)
            {
                await LevelingRewards.CheckLootBoxRewards(user);
            }
            return;
        }
        internal static async Task MessageRewards(SocketGuildUser user, SocketTextChannel channel, SocketMessage msg)
        {
            if (msg == null) return;
            if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;
            if (msg.Author.IsBot) return;

            var userAccount = GlobalUserAccounts.GetUserAccount(user);
            DateTime now = DateTime.UtcNow;

            if (now < userAccount.LastMessage.AddSeconds(Constants.MessageRewardCooldown) || msg.Content.Length < Constants.MessageRewardMinLenght)
            {
                return;
            }

            // Generate a randomized reward in the configured boundries
            userAccount.Money += (ulong)Global.Rng.Next(Constants.MessagRewardMinMax.Item1, Constants.MessagRewardMinMax.Item2 + 1);
            userAccount.LastMessage = now;

            GlobalUserAccounts.SaveAccounts();

            return;
        }
    }
}
