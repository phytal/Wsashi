using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.API.Overwatch
{
    public class SetAccount : WsashiModule
    {
        [Command("owaccount")]
        [Summary("Set your Battle.net username and ID")]
        public async Task OwAccount([Remainder] string usercred)
        {
            usercred = usercred.Replace('#', '-');
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);

            if (usercred == null)
            {
                embed.WithDescription($"Make sure that you put in your Battle.net Account Username and ID in! Ex: Username#1234");
            }
            else
            {
                embed.WithDescription($"Successfully set {usercred} to your default Battle.net credentials.");
                config.OW = usercred;
                GlobalUserAccounts.SaveAccounts();
            }

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
