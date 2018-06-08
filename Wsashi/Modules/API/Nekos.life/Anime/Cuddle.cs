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

namespace Wsashi.Modules.API
{
    public class Cuddle : ModuleBase<SocketCommandContext>
    {
        [Command("cuddle", RunMode = RunMode.Async)]
        [Summary("Displays an random cuddle picture!")]
        public async Task GetRandomNekoCuddle(IGuildUser user = null)
        {

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/cuddle");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Cuddle!");
                embed.WithDescription($"{Context.User.Mention} cuddled with themselves... Maybe you can cuddle with a friend? \n **(Include a user with your command! Example: w!cuddle <person you want to cuddle with>)**");
                embed.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Cuddle!");
                embed.WithDescription($"{Context.User.Username} cuddled with {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }
    }
}
