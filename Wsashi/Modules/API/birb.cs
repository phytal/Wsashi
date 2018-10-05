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
using Wsashi.Preconditions;

namespace Wsashi.Modules.API
{
    public class birb : ModuleBase<SocketCommandContext>
    {
        [Command("birb")]
        [Alias("birdmeme")]
        [Summary("Displays an random birb meme")]
        [Remarks("Ex: w!birb")]
        [Cooldown(5)]
        public async Task GetRandomBirb()
        {
            string url = @"https://random.birb.pw/tweet/random";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "yeaah.jpg");
            }
        }
    }
}
