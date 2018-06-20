using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules
{
    public class Dming : WsashiModule
    {
        [Command("dm")]
        [Summary("DMs a specified user.")]
        [Remarks("w!dm <person you want to dm> <your dm message> Ex: w!dm @Phytal Your bot is cool")]
        [Cooldown(10, true)]
        public async Task Dm(IGuildUser user, [Remainder] string dm)
        {
            var rep = user.Id;

            var application = await Context.Client.GetApplicationInfoAsync();
            var message = await user.GetOrCreateDMChannelAsync();

            var embed = new EmbedBuilder()
            {
                Color = new Color(37, 152, 255)
            };

            embed.WithTitle($":mailbox_with_mail:  | You have recieved a DM from {Context.User.Username}!");
            embed.Description = $"{dm}";
            embed.WithFooter(new EmbedFooterBuilder().WithText($"Guild: {Context.Guild.Name}"));
            await message.SendMessageAsync("", embed: embed.Build());
            embed.Description = $":e_mail: | You have sent a message to {user.Username}, they will read the message soon.";

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}
