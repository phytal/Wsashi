using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.weebDotSh
{
    public class Kill : WsashiModule
    {
        public async Task<RandomData> GetTypesAsync()
        {
            WeebClient weebClient = new WeebClient("Wsashi", Config.bot.Version);
            var tags = new [] { "kill" };
            var result = await weebClient.GetRandomAsync("kill", tags, FileType.Gif, false, NsfwSearch.False); //hidden and nsfw are always defaulted to false

            if (result == null)
            {
                return null;
            }
            return result;
        }

        [Command("kill")]
        [Summary("Displays an image of a cute cuddly cat gif")]
        [Remarks("Ex: w!catgif")]
        [Cooldown(5)]
        public async Task KillUser(IGuildUser user = null)
        {

            Kill kill = new Kill();
            RandomData result = await kill.GetTypesAsync();
            string url = result.Url;

            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Tickle!");
                embed.WithDescription($"{Context.User.Mention} tickled themselves... I'll stay out of this for now... \n **(Include a user with your command! Example: w!tickle <person you want to tickle>)**");
                embed.WithImageUrl(url);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Tickle!");
                embed.WithDescription($"{Context.User.Username} tickled {user.Mention}!");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
