using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Management
{
    public class SlowMode : WsashiModule
    {
        private static DiscordSocketClient _client = Program._client;

        public static async Task HandleSlowMode(SocketMessage s)
        {
            _ = Slowmode(s);
        }

        public static async Task Slowmode(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new SocketCommandContext(_client, msg);
            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            if (config.IsSlowModeEnabled == true)
            {
                if (context.User is IGuildUser user && user.GuildPermissions.ManageChannels) return;
                if (msg == null) return;
                if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;
                if (msg.Author.IsBot) return;

                var userAcc = GlobalUserAccounts.GetUserAccount(msg.Author.Id);

                DateTime now = DateTime.UtcNow;

                if (now < userAcc.LastMessage.AddSeconds(config.SlowModeCooldown))
                {
                    var difference1 = now - userAcc.LastMessage;
                    var time = new TimeSpan((long)config.SlowModeCooldown*10000000);
                    var difference = time - difference1;
                    var timeSpanString = string.Format("{0:%s} seconds", difference);
                    await msg.DeleteAsync();
                    var dm = await context.User.GetOrCreateDMChannelAsync();
                    await dm.SendMessageAsync($"Slow down! You can send a message in **{timeSpanString}** in **{context.Guild.Name}**.");
                    return;
                }
                else
                {
                    userAcc.LastMessage = now;
                    return;
                }
            }
            else return;
        }
    }
}
