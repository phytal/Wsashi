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

namespace Wsashi.Modules.API.Nekos.life.Anime
{
    public class Kiss : ModuleBase<SocketCommandContext>
    {
        [Command("kiss", RunMode = RunMode.Async)]
        [Summary("Kiss someone! :3")]
        public async Task GetRandomNeko(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/kiss");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embedd = new EmbedBuilder();
                embedd.WithColor(37, 152, 255);
                embedd.WithTitle("Kiss!");
                embedd.WithDescription($"{Context.User.Mention} you can't really kiss yourself... Don't worry how about a kiss from me?... \n **(Include a user with your command! Example: w!kiss <person you want to kiss>)**");
                embedd.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", false, embedd);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Kiss!");
                embed.WithDescription($":heart:  |  {Context.User.Username} kissed {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }
    }
}
