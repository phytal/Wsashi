using System.Threading.Tasks;
using Discord.Commands;
using System.Net;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.API
{
    public class birb : WsashiModule
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
