using System.Threading.Tasks;
using Wsashi.Features.GlobalAccounts;
using Discord;
using Discord.Commands;
using Wsashi.Handlers;
using Wsashi.Entities;

namespace Wsashi.Modules
{
    [Group("Prefix"), Alias("Prefixes"), Summary("Setting for the Bots prefix on this server")]
    [RequireContext(ContextType.Guild)]
    public class Prefix : ModuleBase<SocketCommandContext>
    {
        [Command("add"), Alias("set"), RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Adds a prefix to the list of prefixes")]
        public async Task AddPrefix([Remainder] string prefix)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Failed to add the Prefix... Was `{prefix}` already a prefix?";
            if (guildAcc.Prefixes.Contains(prefix) == false)
            {
                guildAcc.Prefixes.Add(prefix);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully added `{prefix}` as prefix!";
            }

            await ReplyAsync(response);
        }

        [Command("remove")] 
        [Summary("Removes a prefix from the list of prefixes")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemovePrefix([Remainder] string prefix)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Failed to remove the Prefix... Was `{prefix}` really a prefix?";
            if (guildAcc.Prefixes.Contains(prefix))
            {
                guildAcc.Prefixes.Remove(prefix);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully removed `{prefix}` as possible prefix!";
            }

            await ReplyAsync(response);
        }

        [Command("list")]
        [Summary("Show all possible prefixes for this server")]
        public async Task ListPrefixes()
        {
            var prefixes = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).Prefixes;
            var response = "No Prefix set yet... just mention me to use commands!";
            if (prefixes.Count != 0) response = "Usable Prefixes are:\n`" + string.Join("`, `", prefixes) + "`\nOr just mention me! :grin:";
            await ReplyAsync(response);
        }
    }
}