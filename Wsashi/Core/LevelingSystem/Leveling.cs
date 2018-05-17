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
        internal static async void UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
            var userAccount = GlobalUserAccounts.GetUserAccount(user);
            uint oldLevel = userAccount.LevelNumber;
            userAccount.XP += 13;
            GlobalUserAccounts.SaveAccounts();
            uint newLevel = userAccount.LevelNumber;
            
                if (oldLevel != newLevel)
                {
                    var dmChannel = await user.GetOrCreateDMChannelAsync();
                    var embed = new EmbedBuilder();

                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("LEVEL UP!");
                    embed.WithDescription(user.Username + " has just leveled up!");
                    embed.AddInlineField("Level", newLevel);
                    embed.AddInlineField("XP", userAccount.XP);

                    var msg = await channel.SendMessageAsync("", embed: embed);
                    //await dmChannel.SendMessageAsync("", embed: embed);
                    await msg.DeleteAsync();
            }
            
        }
    }
}
