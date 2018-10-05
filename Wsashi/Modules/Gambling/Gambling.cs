using Discord.Commands;
using System;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Wsashi.Preconditions;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class Gambling : ModuleBase
    {
        [Command("coinflip")]
        [Summary("Flips a coin and if you win you earn x2 of the Potatoes you betted. If lost you lose your Potatoes.")]
        [Alias("Coin", "flip", "cf")]
        [Remarks("w!cf <side (heads/tails)> <amount of Potatoes you want to flip for(you will earn nothing if left empty)> Ex: w!cf tails 20")]
        [Cooldown(5)]
        public async Task CoinFlip(string side, uint amount = 0)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            config.Money = config.Money - amount;
            GlobalUserAccounts.SaveAccounts();
            Random rand = new Random();
            string sid = side.ToLower();
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"<:coin:459944364546981909> Coin Flip for {amount} Potatoes on the Side of {sid}.");
            if (sid == "tails")
            {
                int randomNumber = rand.Next(1, 4);//you know gambling is dangerous right lol you have a low chance of winning :)
                if (randomNumber == 1)//win
                {
                    config.Money = config.Money + ((ulong)amount * 2);
                    GlobalUserAccounts.SaveAccounts();
                    embed.WithDescription($"I guess heads. And the coin landed on tails! Alright, you beat me. Here's **{amount * 2}** Potatoes!");
                }
                else
                {
                    embed.WithDescription($"I guess heads. And the coin landed on heads! Sorry **{Context.User.Username}**, but your **{amount}** Potatoes are mine now!");
                }
            }
            if (sid == "heads")
            {
                int randomNumber = rand.Next(1, 4);//you know gambling is dangerous right lol
                if (randomNumber == 1)//win
                {
                    config.Money = config.Money + ((ulong)amount * 2);
                    GlobalUserAccounts.SaveAccounts();
                    embed.WithDescription($"I guess tails. And the coin landed on heads! Alright, you beat me. Here's **{amount * 2}** Potatoes!");
                }
                else
                {
                    embed.WithDescription($"I guess tails. And the coin landed on tails! Sorry **{Context.User.Username}**, but your **{amount}** Potatoes are mine now!");
                }
            }
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("roll")]
        [Summary("Rolls a Dice")]
        [Alias("dice", "dice roll")]
        [Remarks("Ex: w!roll")]
        [Cooldown(5)]
        public async Task RollDice()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 7);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(":game_die:  | You Rolled **" + randomNumber + "**");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
