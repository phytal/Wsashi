using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Management.Commands
{
    public class Kick : WsashiModule
    {
        [Command("kick")]
        [Summary("Kicks @Username")]
        [Remarks("w!kick <user you want to kick> Ex: w!kick @Phytal")]
        [Cooldown(5)]
        public async Task KickAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.KickMembers)
            {
                try
                {
                    var kb = (Context.Client as DiscordShardedClient).GetChannel(config.ServerLoggingChannel) as SocketTextChannel;
                    await user.KickAsync();
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $" {user.Username} has been kicked from {user.Guild.Name}";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Kicked by: **{Context.User.Mention}\n**Reason: **{reason}";
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
                catch
                {
                    await ReplyAndDeleteAsync(":hand_splayed:  | You must mention a valid user", timeout: TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Kick Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
