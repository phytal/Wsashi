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
                // Check if the user is 

                if (context.User is IGuildUser user && user.GuildPermissions.ManageChannels) return;
                if (msg == null) return;
                if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;
                if (msg.Author.IsBot) return;

                var userAcc = GlobalUserAccounts.GetUserAccount(msg.Author.Id);

                DateTime now = DateTime.UtcNow;

                // Check if the coolown is up - if not, return
                if (now < userAcc.LastMessage.AddSeconds(config.SlowModeCooldown))
                {
                    var difference = now - userAcc.LastMessage;
                    var timeSpanString = string.Format("{0:%s} seconds", difference);
                    var dm = await context.User.GetOrCreateDMChannelAsync();
                    await dm.SendMessageAsync($"Slow down! You can send a message in **{timeSpanString}** in **{context.Guild.Name}**.");
                    await msg.DeleteAsync();
                    return;
                }
                return;
            }
            else return;
        }
    }
}
