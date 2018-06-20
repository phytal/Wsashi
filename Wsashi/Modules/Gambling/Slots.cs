using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Wsashi.Features.Economy;
using System;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules
{
    public class Slots : ModuleBase<SocketCommandContext>
    {
        [Command("newslot")]
        [Alias("newslots")]
        [Summary("Generates a new slot machine (duh)")]
        [Remarks("Ex: w!newslot")]
        [Cooldown(10)]
        public async Task NewSlot(int amount = 0)
        {
            Global.slot = new Slot(amount);
            await ReplyAsync(":white_check_mark:  | A new slotmachine got generated!");
        }

        [Command("slots")]
        [Alias("slot")]
        [Summary("Play a game of slots! Ex: w!slots <amount you bet on>")]
        [Remarks("w!slots <amount you want to gamble> Ex: w!slots 50")]
        [Cooldown(10)]
        public async Task SpinSlot(uint amount)
        {
            if (amount < 1)
            {
                await ReplyAsync($":x:  | You can't spin for that amount of Potatos.");
                return;
            }
            var account = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            if (account.Money < amount)
            {
                await ReplyAsync($":hand_splayed:  | Sorry but it seems like you don't have enough Potatos... You only have {account.Money}.");
                return;
            }

            account.Money -= amount;
            GlobalUserAccounts.SaveAccounts();

            string slotEmojis = Global.slot.Spin();
            var payoutAndFlavour = Global.slot.GetPayoutAndFlavourText(amount);

            if (payoutAndFlavour.Item1 > 0)
            {
                account.Money += payoutAndFlavour.Item1;
                GlobalUserAccounts.SaveAccounts();
            }

            IUserMessage msg = await ReplyAsync(slotEmojis);
            await Task.Delay(1000);
            await ReplyAsync(payoutAndFlavour.Item2);

        }

        [Command("showslots")]
        [Alias("showslot")]
        [Summary("Shows the slots wheel (don't worry it gets randomized everytime :stuck_out_tongue: ")]
        [Remarks("Ex: w!showslots")]
        [Cooldown(10)]
        public async Task ShowSlot()
        {
            await ReplyAsync(String.Join("\n", Global.slot.GetCylinderEmojis(true)));
        }
    }
}
