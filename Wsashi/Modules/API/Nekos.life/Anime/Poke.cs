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
    public class Poke : ModuleBase<SocketCommandContext>
    {
        [Command("poke", RunMode = RunMode.Async)]
        [Summary("Poke someone! :3")]
        public async Task GetRandomNeko(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/poke");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embedd = new EmbedBuilder();
                embedd.WithColor(37, 152, 255);
                embedd.WithTitle("Poke!");
                embedd.WithDescription($"{Context.User.Mention} poked themselves... I guess you can poke yourself if you're lonely... \n **(Include a user with your command! Example: w!poke <person you want to poke>)**");
                embedd.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", false, embedd);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Poke!");
                embed.WithDescription($":point_right:  |  {Context.User.Username} poked {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }
    }
}
