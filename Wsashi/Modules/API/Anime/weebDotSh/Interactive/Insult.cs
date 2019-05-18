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
    public class Insult : WsashiModule
    {
        [Command("insult")]
        [Summary("Displays an image of an anime insult gif")]
        [Remarks("Usage: w!insult <user you want to insult (or can be left empty)> Ex: w!insult @Phytal")]
        [Cooldown(5)]
        public async Task InsultUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("insult", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Insult!");
                embed.WithDescription(
                    $"{Context.User.Mention} insulted themselves! Why the pessimism? \n**(Include a user with your command! Example: w!insult <person you want to insult>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Insult!");
                embed.WithDescription($"{Context.User.Username} insulted {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
