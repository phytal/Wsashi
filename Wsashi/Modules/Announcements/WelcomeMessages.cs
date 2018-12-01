using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Announcements
{
    public class WelcomeMessages : WsashiModule
    {
        [Command("welcome channel"), Alias("Wc")]
        [Summary("Set where you want welcome messages to be displayed")]
        [Remarks("w!welcome channel <channel where welcome messages are> Ex: w!welcome channel #general")]
        public async Task SetIdIntoConfig(SocketGuildChannel chnl)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($"Set this guild's welcome channel to #{chnl}.");
                config.WelcomeChannel = chnl.Id;
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("welcome add")]
        [Summary("Add a welcome message for people that join!")]
        [Remarks("w!welcome add <welcome message> Ex: `welcome add <usermention>, welcome to **<guildname>**! " +
                 "Try using ```@<botname>#<botdiscriminator> help``` for all the commands of <botmention>!`\n" +
                 "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                 "`<botname>`, `<botdiscriminator>`, `<botmention>` ")]
        public async Task AddWelcomeMessage([Remainder] string message)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var response = $"Failed to add this Welcome Message...";
                if (guildAcc.WelcomeMessages.Contains(message) == false)
                {
                    guildAcc.WelcomeMessages.Add(message);
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    response = $"Successfully added ```\n{message}\n``` as Welcome Message!";
                }
                await Context.Channel.SendMessageAsync(response);
            }

            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("welcome remove"), Summary("Removes a Welcome Message from the ones avaliable")]
        [Remarks("w!welcome remove <welcome messag number (can be shown with w!welcome list) Ex: w!welcome remove 1")]
        public async Task RemoveWelcomeMessage(int messageIndex)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
                var response = $"Failed to remove this Welcome Message... Use the number shown in `welcome list` next to the `#` sign!";
                if (messages.Count > messageIndex - 1)
                {
                    messages.RemoveAt(messageIndex - 1);
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
                }

                await ReplyAsync(response);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }

        [Command("welcome list"), Summary("Shows all currently set Welcome Messages")]
        [Remarks("w!welcome list")]
        public async Task ListWelcomeMessages()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var welcomeMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
                var embB = new EmbedBuilder().WithTitle("No Welcome Messages set yet... add some if you want to greet incoming people!");
                if (welcomeMessages.Count > 0) embB.WithTitle("Possible Welcome Messages:");

                for (var i = 0; i < welcomeMessages.Count; i++)
                {
                    embB.AddField($"Message #{i + 1}:", welcomeMessages[i], true);
                }
                await ReplyAsync("", false, embB.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await Task.Delay(5000);
                await use.DeleteAsync();
            }
        }
    }
}
