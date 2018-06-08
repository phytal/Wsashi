using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Wasagotchi
{
    internal static class Auto
    {
        internal static async void AutomaticActions(SocketGuildUser user)
        {
            var userAccount = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(user);
            var message = await user.GetOrCreateDMChannelAsync();
            if (userAccount.Waste >= 15)
            {
                userAccount.Sick = true;
                GlobalWasagotchiUserAccounts.SaveAccounts();
                await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAccount.Name}** is sick! Treat her right away and buy her some medicine with w!buy! ");
            }
            if ((userAccount.Waste == 20) && (userAccount.Hunger <= 5) && (userAccount.Attention <= 5))
            {
                userAccount.RanAway = true;
                GlobalWasagotchiUserAccounts.SaveAccounts();
                await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAccount.Name}** ran away! The living conditions you provided were too low... Maybe try to pay more attention to your Wasagotchi next time! ");
            }
        }
    }
}