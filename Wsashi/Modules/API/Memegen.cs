using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API
{
    public class Memegen : ModuleBase
    {
        [Command("meme")]
        [Summary("Create a meme!")]
        [Alias("memecreate")]
        [Remarks("w!meme <top text>/<bottom text> (Note that there is no space between top and bottom text from the slash) Ex: w!meme hi/lol")]
        [Cooldown(10)]
        public async Task Define([Remainder] string message)
        {
            message= message.Replace(' ', '_');
            var user = Context.User as SocketGuildUser;
            var aurl = (Context.User.GetAvatarUrl());
            string url = "https://memegen.link/custom/" + message + ".jpg?alt=" + aurl; //+ "?font=Verdana";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "meme.jpg");
            }
        }
        [Command("gif", RunMode = RunMode.Async)]
        public async Task Image()
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://www.reddit.com/r/meme/random/.json");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string image = dataObject[0].data.children[0].data.preview.images[0].source.url.ToString();
            string posttitle = dataObject[0].data.children[0].data.title.ToString();

            var embed = new EmbedBuilder()
                .WithTitle(posttitle)
                .WithImageUrl(image)
                .WithFooter("via r/meme");
            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }
}
