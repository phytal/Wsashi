using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Modules.API
{
    public class Memegen : ModuleBase
    {
        [Command("meme")]
        [Summary("Create a meme! Usage: w!meme top text/bottom text (Note that there is no space between top and bottom text from the slash.)")]
        [Alias("memecreate")]
        public async Task Define([Remainder] string message)
        {
            message= message.Replace(' ', '_');
            var user = Context.User as SocketGuildUser;
            var aurl = (Context.User.GetAvatarUrl());
            string url = "https://memegen.link/custom/" + message + ".jpg?alt=" + aurl; //+ "?font=Verdana";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "meme.jpg");
            }
        }
    }
}
