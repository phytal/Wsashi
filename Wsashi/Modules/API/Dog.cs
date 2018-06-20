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
    public class Dog : ModuleBase<SocketCommandContext>
    {
        [Command("dog")]
        [Alias("doggo")]
        [Summary("Displays an image of a dog")]
        [Remarks("Ex: w!dog")]
        [Cooldown(5)]
        public async Task GetRandomDog()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://dog.ceo/api/breeds/image/random");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string link = dataObject.message.ToString();

            string result = dataObject.status.ToString();

            if(result != "success") return;

            var embed = new EmbedBuilder();
            embed.WithTitle(":dog: | Here's a random dog!");
            embed.WithImageUrl(link);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}