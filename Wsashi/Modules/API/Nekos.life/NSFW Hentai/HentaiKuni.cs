using System;
using Discord.Commands;
using Discord;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Nekos.life.NSFW_Hentai
{
    public class HentaiKuni : ModuleBase<SocketCommandContext>
    {
        [Command("kuni", RunMode = RunMode.Async)]
        [Summary("Displays a hentai kuni")]
        [Remarks("Ex: w!kuni")]
        [Cooldown(5)]
        public async Task GetRandomNekoKuni()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/kuni");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                var embed = new EmbedBuilder();
                embed.WithTitle("Randomly generated hentai kunai just for you <3!");
                embed.WithImageUrl(nekolink);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need to use this command in a NSFW channel, {Context.User.Username}!";
                var use = await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }
    }
}
