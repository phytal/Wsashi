using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace Wsashi.Modules
{
    public class Calculator : ModuleBase
    {
        [Command("Calculator"), Alias("Calc")]
        public async Task Calculate(string oper, float val1, float val2 = 0) //this code is fucking nasty, i will fix it in the future.
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Calculator");

            int result;
            double result2;

            switch (oper)
            {
                case "add":
                    embed.WithDescription($"The answer is `{val1 + val2}`");
                    break;
                case "sub":
                    embed.WithDescription($"The answer is `{val1 - val2}`");
                    break;
                case "mult":
                    embed.WithDescription($"The answer is `{val1 * val2}`");
                    break;
                case "div":
                    embed.WithDescription($"The answer is `{val1 / val2}`");
                    break;
                case "sqrt":
                    result2 = Math.Sqrt(val1);
                    embed.WithDescription($"The answer is `{result2}`");
                    break;
                case "power":
                    result2 = Math.Pow(val1, val2);
                    embed.WithDescription($"The answer is `{result2}`");
                    break;
                default:
                    embed.WithDescription("You didn't specify a valid operation. Valid operations are `add`, `sub`, `mult`, `div`, `power`, and `sqrt`.");
                    break;


            }
            await ReplyAsync("", false, embed);
        }
    }
}
