using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Anime.weebDotSh
{
    public class Lick : WsashiModule
    {
        [Command("lick")]
        [Summary("Displays an image of an anime lick gif")]
        [Remarks("Usage: w!lick <user you want to lick (or can be left empty)> Ex: w!lick @Phytal")]
        [Cooldown(5)]
        public async Task LickUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("lick", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Lick!");
                embed.WithDescription(
                    $"{Context.User.Mention} licked themselves... I'll stay out of this for now... \n**(Include a user with your command! Example: w!lick <person you want to lick>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Lick!");
                embed.WithDescription($"{Context.User.Username} licked {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
