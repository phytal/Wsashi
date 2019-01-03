using System.Net;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.API.Nekos.life.NSFW_Hentai
{
    public class OverwatchHentai : WsashiModule
    {
        [Command("overwatchnsfw")]
        [Summary("Generates a picture of NSFW Overwatch from r/OverwatchNSFW")]
        [Alias("ownsfw")]
        [Remarks("Ex: w!ownsfw")]
        [Cooldown(5)]
        public async Task GetRandomOWHentai()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                string json;
                using (var client = new WebClient())
                {
                    json = client.DownloadString("https://www.reddit.com/r/OverwatchNSFW/random/.json");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
                string image = dataObject[0].data.children[0].data.url.ToString();
                string posttitle = dataObject[0].data.children[0].data.title.ToString();
                string link = dataObject[0].data.children[0].data.permalink.ToString();
                string ups = dataObject[0].data.children[0].data.ups.ToString();
                string comments = dataObject[0].data.children[0].data.num_comments.ToString();

                var embed = new EmbedBuilder()
                    .WithTitle(posttitle)
                    .WithImageUrl(image)
                    .WithFooter($"👍 {ups} | 💬 {comments}")
                    .WithUrl($"https://www.reddit.com{link}")
                    .WithColor(37, 152, 255);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need to use this command in a NSFW channel, {Context.User.Username}!";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }
    }
}
