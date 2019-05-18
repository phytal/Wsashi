using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Anime.weebDotSh
{
    public class HoldHand : WsashiModule
    {
        [Command("holdHand")]
        [Summary("Displays an image of an anime hand holding gif")]
        [Remarks("Usage: w!holdHand <user you want to holdHand (or can be left empty)> Ex: w!holdHand @Phytal")]
        [Cooldown(5)]
        public async Task HandHoldUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("handholding", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("kawaii-ne!");
                embed.WithDescription(
                    $"{Context.User.Mention} held their own hand! Hey, I thought you loved ***me**, {Context.User.Username}! \n**(Include a user with your command! Example: w!holdHand <person you want to hold hands with>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("kawaii-ne!");
                embed.WithDescription($"{Context.User.Username} is holding hands with {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
