using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API
{
    public class Slap : ModuleBase<SocketCommandContext>
    {
        [Command("slap")]
        [Summary("Slap someone!")]
        [Remarks("w!slap <user you want to slap (if left empty you will slap yourself)> Ex: w!slap @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/slap");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Slap!");
                embed.WithDescription($"{Context.User.Mention} slapped themselves... Don't do this to yourself! \n **(Include a user with your command! Example: w!slap <person you want to slap>)**");
                embed.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Slap!");
                embed.WithDescription($"{Context.User.Username} slapped {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
