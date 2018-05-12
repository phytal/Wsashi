using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Modules
{
    public static class Filter
    {
        public static async Task Filtering(SocketMessage msg)
        {
            string[] predictionsTexts = new string[]
    {
        "This is a Christian Minecraft Server!",
        "Watch your language buddy!",
        "I think you touched the stove on accident!",
        "You're starting to bug me.."
    };
            Random rand = new Random();
            List<string> bannedWords = new List<string>
                {
                     "fuck", "bitch", "gay", "shit", "pussy", "penis", "vagina", "nigger", "nigga", "suck", "eat my balls", "make me wet", "nude", "naked"," ass","asshole", "-ass", "cock", "dick", "cunt", "arse", "damn", "hell", "kill urslef", "kys", "slut", "hoe", "whore","retard", "gay", "autis", "screw you", "kill"
                };
            if (msg.Author.IsBot) return;
            if (bannedWords.Any(msg.Content.ToLower().Contains))
            {
                int randomIndex = rand.Next(predictionsTexts.Length);
                string text = predictionsTexts[randomIndex];
                await msg.DeleteAsync();
                var use = await msg.Channel.SendMessageAsync(":warning:  | " + text + " (Inappropiate language)");
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        public static async Task LinkFiltering(SocketMessage msg)
        {
            List<string> bannedWords = new List<string>
                {
                     "www.", ".com", ".org"
                };
            if (msg.Author.IsBot) return;
            if (bannedWords.Any(msg.Content.ToLower().Contains))
            {
                await msg.DeleteAsync();
                await msg.Channel.SendMessageAsync(":warning:  | Don't you dare post your filthy links here!");
            }
        }
    }
}

