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
    public class Filtering : WsashiModule
    {
        [Command("Filter"), Alias("blacklist", "bl", "fil")]
        [Summary("Turns on or off filter. Usage: w!filter true/false")]
        [Remarks("w!filter <on/off> Ex: w!filter on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfigFilter(string setting)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(setting);
                if (result.Item1 == true)
                {
                    var argg = result.Item2;
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? ":white_check_mark:  | Filter successfully turned on. Stay safe!" : ":white_check_mark:  | Filter successfully turned off. Daredevil!");
                    await ReplyAsync("", embed: embed.Build());
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    config.Filter = argg;
                    GlobalGuildAccounts.SaveAccounts();
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!filter <on/off>`");
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

        [Command("FilterIgnore"), Alias("Fi")]
        [Summary("Sets a channel that if Filter is turned on, it will be disabled there")]
        [Remarks("w!fi <channel you want filter to be ignored> Ex: w!fi #nsfw")]
        [Cooldown(5)]
        public async Task SetChannelToBeIgnoredByFilter(string type, SocketGuildChannel chnl = null)
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
                        config.NoFilterChannels.Add(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Filter.");
                        break;
                    case "rem":
                    case "Rem":
                        config.NoFilterChannels.Remove(chnl.Id);
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Filter.");
                        break;
                    case "clear":
                    case "Clear":
                        config.NoFilterChannels.Clear();
                        GlobalGuildAccounts.SaveAccounts();
                        embed.WithDescription("List of channels to be ignored by Filter has been cleared.");
                        break;
                    default:
                        embed.WithDescription($"Valid types are `add`, `rem`, and `clear`. Syntax: `w!fi {{add/rem/clear}} [channelMention]`");
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

        [Command("BlacklistAdd")]
        [Alias("Bladd")]
        [Summary("Add a word to the filter")]
        [Remarks("w!bladd <word you want to add> Ex: w!bladd gay")]
        [Cooldown(5)]
        public async Task AddStringToBl([Remainder]string word)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:  | Added **{word}** to the Blacklist.");

                config.CustomFilter.Add(word);
                GlobalGuildAccounts.SaveAccounts();
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("BlacklistRemove")]
        [Alias("Blrem")]
        [Summary("Remove a custom word from filter")]
        [Remarks("w!blrem <word you want to remove> Ex: w!blrem gay")]
        [Cooldown(5)]
        public async Task RemoveStringFromBl([Remainder] string bl)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                if (!config.CustomFilter.Contains(bl))
                {
                    embed.WithDescription($"`{bl}` isn't present in the Blacklist.");
                }
                else
                {
                    embed.WithDescription($"Removed {bl} from the Blacklist.");
                    config.CustomFilter.Remove(bl);
                    GlobalGuildAccounts.SaveAccounts();
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

        [Command("BlacklistClear")]
        [Alias("Blcl")]
        [Summary("Clears the custom words in the filter")]
        [Remarks("w!blcl")]
        [Cooldown(5)]
        public async Task ClearBlacklist()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.CustomFilter.Clear();
                GlobalGuildAccounts.SaveAccounts();
                var embed = new EmbedBuilder();
                embed.WithDescription("Cleared the Blacklist for this server.");
                embed.WithColor(37, 152, 255);

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

        [Command("BlacklistList")]
        [Alias("Bll")]
        [Summary("Lists the custom words in the filter")]
        [Remarks("w!bll")]
        [Cooldown(5)]
        public async Task ListBlacklist()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                string list = string.Empty;
                foreach (var word in config.CustomFilter)
                {
                    list += $"{word}  ";
                }
                var embed = new EmbedBuilder();
                embed.WithTitle($"Blacklisted words in {Context.Guild.Name}");
                embed.WithDescription(list);
                embed.WithColor(37, 152, 255);

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
