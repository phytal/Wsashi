using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Net;
using Discord;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API
{
    public class Cat : ModuleBase<SocketCommandContext>
    {
        [Command("catgif", RunMode = RunMode.Async)]
        [Summary("Displays an image of a cute cuddly cat gif")]
        [Remarks("Ex: w!catgif")]
        [Cooldown(5)]
        public async Task GetRandomCatGif()
        {
            string url = @"http://thecatapi.com/api/images/get?format=src&type=gif";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "cat.gif");
            }
        }

        [Command("cat", RunMode = RunMode.Async)]
        [Summary("Displays an image of a cute cuddly cat")]
        [Remarks("Ex: w!cat")]
        [Cooldown(5)]
        public async Task GetRandomCat()
        {
            Random rand = new Random();
            int link = rand.Next(1, 3);
            if (link == 1)
            {
                string url = @"http://thecatapi.com/api/images/get?format=src&type=png";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    await Context.Channel.SendFileAsync(stream, "cat.png");
                }
            }
            if (link == 2)
            {
                string url = @"http://thecatapi.com/api/images/get?format=src&type=jpg";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    await Context.Channel.SendFileAsync(stream, "cat.jpg");
                }
            }
        }

        [Command("catfact")]
        [Summary("Displays a random cat fact")]
        [Remarks("Ex: w!cat fact")]
        [Cooldown(3)]
        public async Task RandomCatFact()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://catfact.ninja/fact");
            }

            var ReplyObject = JsonConvert.DeserializeObject<CatFactReply>(json);

            var CatFact = ReplyObject.fact;

            var embed = new EmbedBuilder();
            embed.WithTitle("Random Cat Fact");
            embed.WithDescription(CatFact);
            embed.WithColor(new Color(19, 100, 140));

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        public class CatFactReply
        {
            public String fact { get; set; }
        }
    }
}
