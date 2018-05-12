using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace Wsashi.Modules
{
    public class Calculator : ModuleBase
    {
        [Command("Addition")]
        [Summary("Adds 2 numbers together.")]
        [Alias("Add")]
        public async Task AddAsync(int num1, int num2)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"The Answer To That Is: {num1 + num2}");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Subtract")]
        [Summary("Subtracts 2 numbers.")]
        [Alias("Minus")]
        public async Task SubstractAsync(int num1, int num2)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"The Answer To That Is: {num1 - num2}");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Multiply")]
        [Summary("Multiplys 2 Numbers.")]
        [Alias("Times")]
        public async Task MultiplyAsync(int num1, int num2)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"The Answer To That Is {num1 * num2}");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Divide")]
        [Summary("Divides 2 Numbers.")]
        public async Task DivideAsync(int num1, int num2)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"The Answer To That Is: {num1 / num2}");
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
