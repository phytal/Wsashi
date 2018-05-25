using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;

namespace Wsashi.Modules
{
    public class Dming : WsashiModule
    {
        [Command("dm")]
        [Summary("DMs a specified user.")]
        public async Task Dm(IGuildUser user, [Remainder] string dm)
        {
            var rep = user.Id;

            var application = await Context.Client.GetApplicationInfoAsync();
            var message = await application.Owner.GetOrCreateDMChannelAsync();

            var embed = new EmbedBuilder()
            {
                Color = new Color(37, 152, 255)
            };

            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Message from: {Context.User.Username} | Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", false, embed);
            embed.Description = $"You have sent a message to {user.Username}, they will read the message soon.";

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
