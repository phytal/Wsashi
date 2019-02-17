using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Wsashi.Core.Providers;
using Wsashi.Core.Features.Games;
using Discord;
using System.Threading.Tasks;

namespace Wsashi.Modules.Games
{
    public static class G2048ReactionInput
    {
        public static async Task HandleReaction(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
        {
            if (!G2048Provider.UserIsPlaying(reaction.UserId)) return;

            var msg = await cache.GetOrDownloadAsync();
            // Immediatly remove reaction so user is able to use it as input 
            // Check permissions first
            if (reaction.UserId != msg.Author.Id)
            {
                var user = reaction.User.GetValueOrDefault(null) ?? Global.Client.GetUser(reaction.UserId);
                try
                {
                    await msg.RemoveReactionAsync(reaction.Emote, user);
                }
                catch (Exception e)

                {
                    await Logger.Log(new LogMessage(LogSeverity.Warning, $"Discord | Missing Permissions to remove reaction in {msg.Channel}", e.Message, e.InnerException));
                }
            }

            if (reaction.Emote.Name == "⬆")
            {
                G2048Provider.MakeMove(reaction.UserId, Game2048.MoveDirection.Up);
            }
            else if (reaction.Emote.Name == "⬇")
            {
                G2048Provider.MakeMove(reaction.UserId, Game2048.MoveDirection.Down);
            }
            else if (reaction.Emote.Name == "⬅")
            {
                G2048Provider.MakeMove(reaction.UserId, Game2048.MoveDirection.Left);
            }
            else if (reaction.Emote.Name == "➡")
            {
                G2048Provider.MakeMove(reaction.UserId, Game2048.MoveDirection.Right);
            }
            else if (reaction.Emote.Name == "❌")
            {
                G2048Provider.EndGame(reaction.UserId);
                return;
            }

            
            /*
             *("⬆"));
        await msg.AddReactionAsync(new Emoji("⬇"));
        await msg.AddReactionAsync(new Emoji("⬅"));
        await msg.AddReactionAsync(new Emoji("➡"));
             *
             */
        }
    }
}
