using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord.Rest;
using Discord.WebSocket;
using Wsashi.Core.Features.Games;

namespace Wsahsi.Core.Providers
{
    public struct G1024Game
    {
        public ulong PlayerId { get; set; }
        public RestUserMessage Message { get; set; }
        public int[][] Grid { get; set; }
        public int Score { get; set; }
        public Game1024.GameState State { get; set; }
        public int Move { get; set; }
    }

    public static class G1024Provider
    {
        public static List<G1024Game> games;

        static G1024Provider()
        {
            games = new List<G1024Game>();
        }

        public static bool UserIsPlaying(ulong userId)
        {
            return games.Any(g => g.PlayerId == userId);
        }

        public static void CreateNewGame(ulong userId, RestUserMessage message)
        {
            var game = new G1024Game()
            {
                Grid = Game1024.GetNewGameBoard(),
                Score = 0,
                State = Game1024.GameState.Playing,
                PlayerId = userId,
                Message = message,
                Move = 0
            };

            games.Add(game);
            UpdateMessage(game);
        }

        public static void MakeMove(ulong userId, Game1024.MoveDirection direction)
        {
            if (!UserIsPlaying(userId)) return;


            var game = games.FirstOrDefault(g => g.PlayerId == userId);
            var gamesID = games.IndexOf(game);

            var result = Game1024.MakeMove(game.Grid, direction);

            game.Score += result.GainedScore;
            game.State = result.State;
            game.Grid = result.Board;
            game.Move++;

            games[gamesID] = game;

            UpdateMessage(game);
        }

        public static async void UpdateMessage(G1024Game game)
        {
            var builder = new StringBuilder();
            builder.Append("Score: ");
            builder.Append(game.Score);
            builder.Append("\nMove: ");
            builder.Append(game.Move);
            builder.Append("\n```cpp\n");
            builder.Append(FormatBoard(game.Grid));

            if (game.State == Game1024.GameState.Lost)
            {
                builder.Clear();
                builder.Append("YOU LOST\n");
                builder.Append("Score: ");
                builder.Append(game.Score);

                games.Remove(game);
            }
            else if (game.State == Game1024.GameState.Won)
            {
                builder.Clear();
                builder.Append("**YOU WON**\n");
                builder.Append("Score: ");
                builder.Append(game.Score);

                games.Remove(game);
            }

            builder.Append("\n```");

            await game.Message.ModifyAsync(m => m.Content = builder.ToString());
        }

        public static string FormatBoard(int[][] board)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    var paddingCount = 4 - (board[i][j].ToString().Length);
                    builder.Append(board[i][j]);
                    for (int m = 0; m < paddingCount; m++) builder.Append(" ");
                    builder.Append(" | ");
                }

                builder.Append("\n");
            }

            return builder.ToString();
        }

        public static void PrintGridToConsole(int[][] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(grid[i][j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}