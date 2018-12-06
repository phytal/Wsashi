using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Overwatch
{
    public class SetAccount : WsashiModule
    {
        [Command("owaccount")]
        [Summary("Set your Overwatch username and ID")]
        [Remarks("w!owaccount <username> Ex: w!owaccount Username#1234")]
        [Cooldown(10)]
        public async Task OwAccount(string user, string platform, string region)
        {
            user = user.Replace('#', '-');
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Overwatch Credentials");
            embed.AddField("Username", user);
            embed.AddField("Platform", platform);
            embed.AddField("Region", region);
            embed.WithDescription($"Successfully set your default Battle.net credentials.");

            config.OW.Add("username", user);
            config.OW.Add("platform", platform);
            config.OW.Add("region", region);
            GlobalUserAccounts.SaveAccounts();


            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("owaccount")]
        [Summary("View your Overwatch username and ID")]
        [Remarks("w!owaccountinfo")]
        [Cooldown(10)]
        public async Task GetOwAccount()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            var values = config.OW;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Here are your Overwatch credentials");
            foreach (var value in values)
            {
                embed.AddField(value.Key, value.Value, true);
            }

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}
