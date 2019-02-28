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
    public class LevelingRewards
    {
        public static Task CheckDuelRewards(SocketGuildUser user)
        {
            var userAccount = GlobalGuildUserAccounts.GetUserID(user);
            var config = GlobalUserAccounts.GetUserAccount(user);
            int wins = config.Wins;

            if (wins == 10)
            {
                if (config.Attacks.Contains("Bash"))
                    config.Attacks.Add("Bash");
            }
            if (wins == 30)
            {
                if (config.Attacks.Contains("Fireball"))
                    config.Attacks.Add("Fireball");
            }
            if (wins == 50)
            {
                if (config.Attacks.Contains("Meditate"))
                    config.Attacks.Add("Meditate");
            }
            if (wins == 70)
            {
                if (config.Attacks.Contains("Earth Shatter"))
                    config.Attacks.Add("Earth Shatter");
            }
            GlobalUserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }
        public static async Task CheckDuelLootboxes(SocketGuildUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var channel = await user.GetOrCreateDMChannelAsync();
            int wins = (int)config.Wins;

            var uc = wins / 10;
            var rare = wins / 20;
            var epic = wins / 35;
            var legendary = wins / 50;
            if (legendary == config.LegendaryLB)
            {
                config.LegendaryLBD += 1;
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **LEGENDARY** lootbox for reaching {config.Wins} wins!");
                return;
            }
            if (epic == config.EpicLB)
            {
                config.EpicLBD += 1;
                config.LootBoxEpic += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **EPIC** lootbox for reaching {config.Wins} wins!");
                return;
            }
            if (rare == config.RareLB)
            {
                config.RareLBD += 1;
                config.LootBoxRare += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **RARE** lootbox for reaching {config.Wins} wins!");
                return;
            }
            if (uc == config.UncommonLB)
            {
                config.UncommonLBD += 1;
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **UNCOMMON** lootbox for reaching {config.Wins} wins!");
                return;
            }
            else
            {
                config.LootBoxCommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **COMMON** lootbox for reaching {config.Wins} wins!");
                return;
            }
        }

            public static async Task CheckLootBoxRewards(SocketGuildUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var channel = await user.GetOrCreateDMChannelAsync();
            int level = (int)config.LevelNumber;

            var uc = level / 5;
            var rare = level / 10;
            var epic = level / 15;
            var legendary = level / 20;
            if (legendary == config.LegendaryLB)
            {
                config.LegendaryLB += 1;
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **LEGENDARY** lootbox for reaching level {config.LevelNumber}");
                return;
            }
            if (epic == config.EpicLB)
            {
                config.EpicLB += 1;
                config.LootBoxEpic += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **EPIC** lootbox for reaching level {config.LevelNumber}");
                return;
            }
            if (rare == config.RareLB)
            {
                config.RareLB += 1;
                config.LootBoxRare += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **RARE** lootbox for reaching level {config.LevelNumber}");
                return;
            }
            if (uc == config.UncommonLB)
            {
                config.UncommonLB += 1;
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **UNCOMMON** lootbox for reaching level {config.LevelNumber}");
                return;
            }
            else
            {
                config.LootBoxCommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have recieved a **COMMON** lootbox for reaching level {config.LevelNumber}");
                return;
            }
        }
    }
}
