using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Features.GlobalAccounts;

namespace Watchdog.Modules
{
    public class Prefix2 : ModuleBase<SocketCommandContext>
    {
        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Prefix(string prefix)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $":x:  | Failed to add the Prefix... Was `{prefix}` already a prefix?";
            if (guildAcc.Prefixes.Contains(prefix) == false)
            {
                guildAcc.Prefixes = prefix;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $":white_check_mark:  | Successfully changed prefix to {prefix}!";
            }
            await ReplyAsync(response);
        }
    }
}
