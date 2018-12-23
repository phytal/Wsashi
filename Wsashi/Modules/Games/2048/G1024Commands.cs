using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wsashi.Core.Providers;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.Games
{
    public class G1024Commands : WsashiModule
    {
        [Command("2048 start")]
        [Summary("Starts a 2048 game")]
        [Alias("2048", "1024", "1024 start")]
        public async Task Start1024Game()
        {
            if (G1024Provider.UserIsPlaying(Context.User.Id))
            {
                await ReplyAsync("You are already playing.\nYou must end the game first.");
                return;
            }

            var msg = await Context.Channel.SendMessageAsync("**YOUR GAME IS BEING PREPARED**\nPlease wait until all 4 emojis are added and the board changes.");

            await msg.AddReactionAsync(new Emoji("⬆"));
            await msg.AddReactionAsync(new Emoji("⬇"));
            await msg.AddReactionAsync(new Emoji("⬅"));
            await msg.AddReactionAsync(new Emoji("➡"));
            await msg.AddReactionAsync(new Emoji("❌"));

            G1024Provider.CreateNewGame(Context.User.Id, msg);
        }

        [Command("end2048")]
        [Alias("finish2048", "e2048", "e2")]
        public async Task End2048Gmae()
        {
            G1024Provider.EndGame(Context.User.Id);
            await Task.CompletedTask;
        }
    }
}
