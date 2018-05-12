using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Modules.API
{
    public class Shiba : ModuleBase<SocketCommandContext>
    {
        [Command("shiba")]
        [Alias("shibe")]
        [Summary("Displays an image of a Shiba Inu :3")]
        public async Task GetRandomShiba()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://shibe.online/api/shibes?count=1&urls=true&httpsUrls=false");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string link = dataObject[0].ToString();

            await Context.Channel.SendMessageAsync(link);
        }
    }
}
