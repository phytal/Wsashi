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

namespace Wsashi.Modules.API.Nekos.life.Anime
{
    public class Pat : ModuleBase<SocketCommandContext>
    {
        [Command("pat", RunMode = RunMode.Async)]
        [Summary("Pat someone! :3")]
        public async Task GetRandomNekoPat(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/pat");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Pat!");
                embed.WithDescription($"{Context.User.Mention} patted thin air... You can pat me if you would like! \n **(Include a user with your command! Example: w!pat <person you want to pat>)**");
                embed.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Pat!");
                embed.WithDescription($"{Context.User.Username} patted {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }
    }
}
