using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Net;
using Discord;

namespace Watchdog.Modules.API
{
    public class Cat : ModuleBase<SocketCommandContext>
    {
        [Command("cat")]
        [Summary("Displays an image of a cute cuddly cat")]
        public async Task GetRandomCat()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://aws.random.cat/meow");
            }

            var ReplyObject = JsonConvert.DeserializeObject<CatReply>(json);

            var catImageUrl = ReplyObject.file;

            await Context.Channel.SendMessageAsync(catImageUrl);
        }

        public class CatReply
        {
            public String file { get; set; }
        }

        [Command("catfact")]
        [Summary("Displays a random cat fact")]
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

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        public class CatFactReply
        {
            public String fact { get; set; }
        }
    }
}
