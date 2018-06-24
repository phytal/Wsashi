using Discord;
using Discord.Commands;
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
    public class Bird : ModuleBase<SocketCommandContext>
    {
        [Command("bird")]
        [Summary("Displays an image of a bird (not a meme lol)")]
        [Remarks("Ex: w!bird")]
        [Cooldown(10)]
        public async Task GetRandomShiba()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://shibe.online/api/birds?count=1&urls=true&httpsUrls=false");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string link = dataObject[0].ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle(":bird: | Here's a random bird!");
            embed.WithImageUrl(link);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}
