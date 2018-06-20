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

namespace Watchdog.Modules.API
{
    public class Cat : ModuleBase<SocketCommandContext>
    {
        [Command("cat")]
        [Summary("Displays an image of a cute cuddly cat")]
        [Remarks("Ex: w!cat")]
        [Cooldown(5)]
        public async Task GetRandomCat()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://aws.random.cat/meow");
            }

            var ReplyObject = JsonConvert.DeserializeObject<CatReply>(json);

            var catImageUrl = ReplyObject.file;

            var embed = new EmbedBuilder();
            embed.WithTitle(":cat: | Here's a random cat!");
            embed.WithImageUrl(catImageUrl);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        public class CatReply
        {
            public String file { get; set; }
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
