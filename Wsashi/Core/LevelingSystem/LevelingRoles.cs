using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Core.LevelingSystem
{
    internal static class LevelingRoles
    {
        internal static async void UserSentMessage(SocketGuildUser user)
        {
            if (user.Guild.Id == 389508749821345806)
            {
                var userAccount = GlobalGuildUserAccounts.GetUserID(user);
                uint oldLevel = userAccount.LevelNumber;

                if (oldLevel != 10)
                {
                    var mplus = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MEMBER+").FirstOrDefault() as SocketRole;
                    var m = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MEMBER").FirstOrDefault() as SocketRole;
                    await user.AddRoleAsync(mplus);
                    await user.RemoveRoleAsync(m);
                }
                if (oldLevel != 30)
                {
                    var mplus = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MEMBER++").FirstOrDefault() as SocketRole;
                    var m = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MEMBER+").FirstOrDefault() as SocketRole;
                    await user.AddRoleAsync(mplus);
                    await user.RemoveRoleAsync(m);
                }
            }
            else return;
        }
    }
}
