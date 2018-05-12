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
    public class Hug : ModuleBase<SocketCommandContext>
    {
        [Command("Hug", RunMode = RunMode.Async)]
        [Summary("Hug someone!")]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/hug");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embedd = new EmbedBuilder();
                embedd.WithColor(37, 152, 255);
                embedd.WithTitle("Hug!");
                embedd.WithDescription($"{Context.User.Mention} hugged themselves... Aw, don't be sad, you can hug me! \n **(Include a user with your command! Example: w!hug <person you want to hug>)**");
                embedd.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", false, embedd);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Hug!");
                embed.WithDescription($":heart:  |  {Context.User.Username} hugged {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }
    }
}
