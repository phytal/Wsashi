using Discord.Commands;
using System;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Watchdog.Modules
{
    public class Gambling : ModuleBase
    {
        string[] CoinSides = new string[]
        {
                "Heads",
                "Tails"
        };

        [Command("coinflip")]
        [Summary("Flips a coin")]
        [Alias("Coin", "flip")]
        public async Task CoinFlip()
        {
            Random rand = new Random();
            int randomIndex = rand.Next(CoinSides.Length);
            string text = CoinSides[randomIndex];
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(":dvd:  | " + Context.User.Username + ", " + text + ".");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("roll")]
        [Summary("Rolls a Dice")]
        [Alias("dice", "dice roll")]
        public async Task RollDice()
        {

            Random random = new Random();
            int randomNumber = random.Next(1, 7);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(":game_die:  | You Rolled **" + randomNumber + "**");
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
