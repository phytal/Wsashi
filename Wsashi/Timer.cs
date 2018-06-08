using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Wsashi.Features;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi
{
    internal static class Timers
    {
        private static DiscordSocketClient _client;
        private static Timer loopingtimer;
        private static SocketTextChannel channel;
        private static SocketGuildUser user;

        internal static Task StartTimer()
        {
            var fourHoursInMiliSeconds = 14400000;
            loopingtimer = new Timer()
            {
                Interval = fourHoursInMiliSeconds,
                AutoReset = true,
                Enabled = true
            };
            loopingtimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Initialized Mission - Cripple Wasagotchi");
            return Task.CompletedTask;
        }
        //try to get something so that all pets will experience soemthing
        private static void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = GlobalWasagotchiUserAccounts.GetFilteredAccounts(usrAcc => true);
            foreach (var userAcc in config)
            {
                if (userAcc.Have == true)
                {
                    if (userAcc.Hunger >= 20)
                        userAcc.Hunger = userAcc.Hunger - 1;
                    else userAcc.Hunger = 0;
                    if (userAcc.Waste <= 20)
                        userAcc.Waste = userAcc.Waste + 1;
                    else userAcc.Waste = 20;
                    if (userAcc.Attention >= 20)
                        userAcc.Attention = userAcc.Attention - 1;
                    else userAcc.Attention = 0;
                    GlobalWasagotchiUserAccounts.SaveAccounts();
                }
                else
                {
                    return;
                }
            }
            Console.WriteLine("Successfully executed pet crippling effects.");
        }
    }
}
