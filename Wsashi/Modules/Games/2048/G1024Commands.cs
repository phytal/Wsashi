using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Wsahsi.Core.Providers;

namespace Wsashu.Modules.Games
{
    public class G1024Commands : ModuleBase<SocketCommandContext>
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

            G1024Provider.CreateNewGame(Context.User.Id, msg);
        }
    }
}
