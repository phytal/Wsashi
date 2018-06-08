using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Modules.Games
{
    public class Fortnite : ModuleBase
    {
        [Command("fortnite")]
        [Summary("Gives you the 'Fortnite' rank (access to Fortnite channels)")]
        public async Task FortniteR()
        {
            var fortnite = Context.Guild.Roles.Where(input => input.Name.ToUpper() == "FORTNITE").FirstOrDefault() as SocketRole;
            var userlist = await Context.Guild.GetUsersAsync();
            var user = userlist.Where(input => input.Username == Context.Message.Author.Username).FirstOrDefault() as SocketGuildUser;

            if (user.Roles.Contains(fortnite))
            {
                await user.RemoveRoleAsync(fortnite);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":white_check_mark:  | Removed Fortnite role.");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                await user.AddRoleAsync(fortnite);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":white_check_mark:  | Added Fortnite role");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
