using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Wsashi.Features;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi
{
    public class WasagotchiTimer
    {
        private static Timer loopingtimer;
        private readonly DiscordShardedClient _client;

        internal Task StartTimer()
        {
            var fourHoursInMiliSeconds = 14400000;
            loopingtimer = new Timer()
            {
                Interval = fourHoursInMiliSeconds,
                AutoReset = true,
                Enabled = true
            };
            loopingtimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Initialized Mission - Cripple Wasagotchi"); //lmao such cringe
            return Task.CompletedTask;
        }
        //try to get something so that all pets will experience soemthing
        public async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = GlobalWasagotchiUserAccounts.GetAllWasagotchiAccount();
            foreach (var userAcc in config)
            {
                if (userAcc.Have == true)
                {
                    if (userAcc.Hunger > 0)
                        userAcc.Hunger = userAcc.Hunger - 1;
                    else userAcc.Hunger = 0;
                    if (userAcc.Waste < 20)
                        userAcc.Waste = userAcc.Waste + 1;
                    else userAcc.Waste = 20;
                    if (userAcc.Attention > 0)
                        userAcc.Attention = userAcc.Attention - 1;
                    else userAcc.Attention = 0;

                    var user = _client.GetUser(userAcc.Id);
                    var message = await user.GetOrCreateDMChannelAsync();
                    if (userAcc.Waste >= 15)
                    {
                        userAcc.Sick = true;
                        await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAcc.Name}** is sick! Treat her right with medicine with w!buy! ");
                    }
                    if ((userAcc.Waste == 20) && (userAcc.Hunger <= 5) && (userAcc.Attention <= 5))
                    {
                        userAcc.XP = 0;
                        userAcc.Name = null;
                        userAcc.Have = false;
                        userAcc.pfp = null;
                        userAcc.RanAway = true;
                        await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAcc.Name}** ran away! The living conditions you provided were too low... Maybe try to pay more attention to your Wasagotchi next time! ");
                    }
                    Console.WriteLine("Successfully executed pet crippling effects.");
                    GlobalWasagotchiUserAccounts.SaveAccounts();
                    return;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
