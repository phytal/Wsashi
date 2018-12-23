using System.Threading.Tasks;
using Discord.Commands;
using System.Net;
using Newtonsoft.Json;
using Discord;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.API.Nekos.life.Anime
{
    public class Kiss : WsashiModule
    {
        [Command("kiss")]
        [Summary("Kiss someone! :3")]
        [Remarks("w!kiss <user you want to kiss (if left empty you will kiss yourself)> Ex: w!kiss @Phytal")]
        [Cooldown(10)]
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
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Kiss!");
                embed.WithDescription($"{Context.User.Mention} you can't really kiss yourself... Don't worry how about a kiss from me?... \n **(Include a user with your command! Example: w!kiss <person you want to kiss>)**");
                embed.WithImageUrl(nekolink);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(nekolink);
                embed.WithTitle("Kiss!");
                embed.WithDescription($":heart:  |  {Context.User.Username} kissed {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
