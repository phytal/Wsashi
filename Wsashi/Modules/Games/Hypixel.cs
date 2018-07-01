using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Modules.Games
{
    public class Hypixel : ModuleBase
    {
        [Command("party")]
        [Summary("Gives you the 'Party' rank (access to party channels)")]
        [Alias("p")]
        public async Task Party()
        {
            var party = Context.Guild.Roles.Where(input => input.Name.ToUpper() == "PARTY").FirstOrDefault() as SocketRole;
            var userlist = await Context.Guild.GetUsersAsync();
            var user = userlist.Where(input => input.Username == Context.Message.Author.Username).FirstOrDefault() as SocketGuildUser;

            if (user.Roles.Contains(party))
            {
                await user.RemoveRoleAsync(party);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":white_check_mark:  Removed Party role.");
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                await user.AddRoleAsync(party);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":white_check_mark:  Added Party role");
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
