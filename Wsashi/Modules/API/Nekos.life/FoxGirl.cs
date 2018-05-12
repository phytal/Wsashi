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
    public class FoxGirl : ModuleBase<SocketCommandContext>
    {
        [Command("foxgirl", RunMode = RunMode.Async)]
        [Summary("Displays an random fox girl :3")]
        public async Task GetRandomFoxGirl()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/fox_girl");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            await Context.Channel.SendMessageAsync(nekolink);
        }
    }
}