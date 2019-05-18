using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Anime.weebDotSh
{
    public class Baka : WsashiModule
    {
        [Command("baka")]
        [Summary("Displays an image of an anime baka gif")]
        [Remarks("Usage: w!baka <user you want to call a baka (or can be left empty)> Ex: w!baka @Phytal")]
        [Cooldown(5)]
        public async Task bakaUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("baka", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Baka!");
                embed.WithDescription(
                    $"{Context.User.Mention} called themselves a baka! I kind of agree with that.. \n**(Include a user with your command! Example: w!baka <person you want to call a baka>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Baka!");
                embed.WithDescription($"{Context.User.Username} called {user.Mention} a baka!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
