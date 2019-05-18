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

            var uc = wins % 10;
            var rare = wins % 20;
            var epic = wins % 35;
            var legendary = wins % 50;
            if (legendary == 0)
            {
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **LEGENDARY** lootbox for reaching {config.Wins} wins!");
            }
            else if (epic == 0)
            {
                config.LootBoxEpic += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **EPIC** lootbox for reaching {config.Wins} wins!");
            }
            else if (rare == 0)
            {
                config.LootBoxRare += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **RARE** lootbox for reaching {config.Wins} wins!");
            }
            else if (uc == 0)
            {
                config.LootBoxUncommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **UNCOMMON** lootbox for reaching {config.Wins} wins!");
            }
            else
            {
                config.LootBoxCommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **COMMON** lootbox for reaching {config.Wins} wins!");
            }
        }

            public static async Task CheckLootBoxRewards(SocketGuildUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var channel = await user.GetOrCreateDMChannelAsync();
            int level = (int)config.LevelNumber;

            int uc = level % 5;
            int rare = level % 10;
            int epic = level % 15;
            int legendary = level % 20;
            if (legendary == 0)
            {
                config.LootBoxLegendary += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **LEGENDARY** lootbox for reaching level {config.LevelNumber}");
            }
            else if (epic == 0)
            {
                config.LootBoxEpic += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **EPIC** lootbox for reaching level {config.LevelNumber}");
            }
            else if (rare == 0)
            {
                config.LootBoxRare += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **RARE** lootbox for reaching level {config.LevelNumber}");
            }
            else if (uc == 0)
            {
                config.LootBoxUncommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **UNCOMMON** lootbox for reaching level {config.LevelNumber}");
            }
            else
            {
                config.LootBoxCommon += 1;
                GlobalUserAccounts.SaveAccounts();
                await channel.SendMessageAsync($"**{user.Username}**, you have received a **COMMON** lootbox for reaching level {config.LevelNumber}");
            }
        }
    }
}
