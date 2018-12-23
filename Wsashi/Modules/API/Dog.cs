using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Net;
using Discord;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.API
{
    public class Dog : WsashiModule
    {
        [Command("dog")]
        [Alias("doggo")]
        [Summary("Displays an image of a dog")]
        [Remarks("Ex: w!dog")]
        [Cooldown(5)]
        public async Task GetRandomDog()
        {
            Random rand = new Random();
            int link = rand.Next(1, 4);
            if (link == 1)
            {
                string url = @"https://api.thedogapi.com/v1/images/search?format=src&mime_types=image/png";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    await Context.Channel.SendFileAsync(stream, "dog.png");
                }
            }
            if (link == 2)
            {
                string url = @"https://api.thedogapi.com/v1/images/search?format=src&mime_types=image/jpg";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    await Context.Channel.SendFileAsync(stream, "dog.jpg");
                }
            }
            if (link == 3)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://dog.ceo/api/breeds/image/random");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string img = dataObject.message.ToString();

                string result = dataObject.status.ToString();

                if (result != "success") return;

                var embed = new EmbedBuilder();
                embed.WithTitle(":dog: | Here's a random dog!");
                embed.WithImageUrl(img);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        [Command("dogg")]
        [Alias("doggif")]
        [Summary("Displays an gif of a dog")]
        [Remarks("Ex: w!doggif")]
        [Cooldown(5)]
        public async Task GetRandomDogGif()
        {
            string url = @"https://api.thedogapi.com/v1/images/search?format=src&mime_types=image/gif";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "doggo.gif");
            }
        }
    }
}