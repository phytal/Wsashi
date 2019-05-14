using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;
using System.Linq;
using Wsashi.Helpers;

namespace Wsashi.Modules.Management.Commands
{
    public class ServerLogging : WsashiModule
    {
        [Command("ServerLogging"), Alias("Sl", "logging")]
        [Summary("Enables server logging (such as bans, message edits, deletions, kicks, channel additions, etc)")]
        [Remarks("w!sl <on/off> Ex: w!sl on")]
        [Cooldown(5)]
        public async Task SetServerLoggingChannel(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool setting = result.Item2;
                    var chnl = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
                    if (chnl == null)
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
                        var perms = new OverwritePermissions(
                            viewChannel: PermValue.Deny
                            );
                        var channell = await Context.Guild.CreateTextChannelAsync("logs");
                        await channell.AddPermissionOverwriteAsync(role, perms);
                    }
                    var chnl1 = Context.Guild.TextChannels.FirstOrDefault(r => r.Name == "logs");
                    var channel = chnl1 as SocketTextChannel;
                    string lol;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    if (setting) { lol = "Enabled server logging"; } else { lol = "Disabled server logging"; }
                    ulong id = channel.Id;
                    config.IsServerLoggingEnabled = setting;
                    config.ServerLoggingChannel = id;
                    GlobalGuildAccounts.SaveAccounts();
                    var embed = MiscHelpers.CreateEmbed(Context, "Server Logging", $"{lol}, and set the channel to <#{channel.Id}>.").WithColor(37, 152, 255);
                    await MiscHelpers.SendMessage(Context, embed);
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `w!sl <on/off>`");
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
    }
}
