using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Wsahsi.Core.Providers;
using Wsashi.Core.Features.Games;

namespace Wsashi.Modules.Games
{
    public static class G1024ReactionInput
    {
        public static void HandleReaction(SocketReaction reaction)
        {
            if (!G1024Provider.UserIsPlaying(reaction.UserId)) return;

            if (reaction.Emote.Name == "⬆")
            {
                G1024Provider.MakeMove(reaction.UserId, Game1024.MoveDirection.Up);
            }
            else if (reaction.Emote.Name == "⬇")
            {
                G1024Provider.MakeMove(reaction.UserId, Game1024.MoveDirection.Down);
            }
            else if (reaction.Emote.Name == "⬅")
            {
                G1024Provider.MakeMove(reaction.UserId, Game1024.MoveDirection.Left);
            }
            else if (reaction.Emote.Name == "➡")
            {
                G1024Provider.MakeMove(reaction.UserId, Game1024.MoveDirection.Right);
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
