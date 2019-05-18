using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;
using Wsashi.Helpers;

namespace Wsashi.Modules.Management.Commands
{
    public class CustomCurrency : WsashiModule
    {
        [Command("customcurrency"), Alias("cc")]
        [Summary("Make a custom currency for the server! (Defaulted to Potatoes)")]
        [Remarks("w!cc <name of your custom currency> Ex: w!cc Credits")]
        [Cooldown(5)]
        public async Task CustomCurrencyCMD([Remainder]string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($"The server's currency is now set to the **{arg}**!");
                    config.Currency = arg;
                    GlobalGuildAccounts.SaveAccounts();

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                if (arg == string.Empty)
                {
                    await Context.Channel.SendMessageAsync($"The server currency is now set to the default **Potato** To change this, you can use `w!cc <name of your custom currency>`");
                    config.Currency = "Potatoes";
                    GlobalGuildAccounts.SaveAccounts();
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
