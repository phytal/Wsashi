using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Net;

namespace Watchdog.Modules.API
{
    public class Dog : ModuleBase<SocketCommandContext>
    {
        [Command("dog")]
        [Alias("doggo")]
        [Summary("Displays an image of a dog")]
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

            await Context.Channel.SendMessageAsync(link);
        }
    }
}