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

namespace Wsashi.Modules.API.Nekos.life.Anime
{
    public class Hug : ModuleBase<SocketCommandContext>
    {
        [Command("Hug")]
        [Summary("Hug someone!")]
        [Remarks("w!hug <user you want to hug (if left empty you will hug yourself)> Ex: w!hug @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/hug");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Hug!");
                embed.WithDescription($"{Context.User.Mention} hugged themselves... Aw, don't be sad, you can hug me! \n **(Include a user with your command! Example: w!hug <person you want to hug>)**");
                embed.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Hug!");
                embed.WithDescription($":heart:  |  {Context.User.Username} hugged {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
