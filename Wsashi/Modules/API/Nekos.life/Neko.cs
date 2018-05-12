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

namespace Watchdog.Modules.API
{
    public class Neko : ModuleBase<SocketCommandContext>
    {
        [Command("neko", RunMode = RunMode.Async)]
        [Summary("Displays an random neko :3")]
        public async Task GetRandomNeko()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/neko");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            await Context.Channel.SendMessageAsync(nekolink);
        }
    }
}
