using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;
using Wsashi.Helpers;

namespace Wsashi.Modules.Management.Commands
{
    public class AntiLink : WsashiModule
    {
        [Command("Antilink"), Alias("Al")]
        [Summary("Turns on or off the link filter.")]
        [Remarks("w!al <on/off> Ex: w!al on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfig(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool setting = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    config.Antilink = setting;
                    GlobalGuildAccounts.SaveAccounts();
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(setting ? "Enabled Antilink for this server." : "Disabled Antilink for this server.");
                    await ReplyAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!al <on/off>`");
                    return;
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

        [Command("AntilinkIgnore"), Alias("Ali")]
        [Summary("Sets a channel that if Antilink is turned on, it will be disabled there")]
        [Remarks("w!ali <channel you want anti-link to be ignored> Ex: w!ali #links-only")]
        [Cooldown(5)]
        public async Task SetChannelToBeIgnored(string type, SocketGuildChannel chnl = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                switch (type)
                {
                    case "add":
                    case "Add":
                        config.AntilinkIgnoredChannels.Add(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Antilink.");
                        break;
                    case "rem":
                    case "Rem":
                        config.AntilinkIgnoredChannels.Remove(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Antilink.");
                        break;
                    case "clear":
                    case "Clear":
                        config.AntilinkIgnoredChannels.Clear();
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription("List of channels to be ignored by Antilink has been cleared.");
                        break;
                    default:
                        embed.WithDescription($"Valid types are `add`, `rem`, and `clear`. Syntax: `w!ali {{add/rem/clear}} [channelMention]`");
                        break;
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
