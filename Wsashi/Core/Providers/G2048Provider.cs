using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Wsashi.Core.Features.Games;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Core.Providers
{
    public struct G2048Game
    {
        public ulong PlayerId { get; set; }
        public RestUserMessage Message { get; set; }
        public int[][] Grid { get; set; }
        public int Score { get; set; }
        public Game2048.GameState State { get; set; }
        public int Move { get; set; }
    }

    public static class G2048Provider
    {
        public static List<G2048Game> games;

        static G2048Provider()
        {
            games = new List<G2048Game>();
        }

        public static bool UserIsPlaying(ulong userId)
        {
            return games.Any(g => g.PlayerId == userId);
        }

        public static void CreateNewGame(ulong userId, RestUserMessage message)
        {
            var game = new G2048Game()
            {
                Grid = Game2048.GetNewGameBoard(),
                Score = 0,
                State = Game2048.GameState.Playing,
                PlayerId = userId,
                Message = message,
                Move = 0
            };

            games.Add(game);
            UpdateMessage(game, userId);
        }

        public static void MakeMove(ulong userId, Game2048.MoveDirection direction)
        {
            if (!UserIsPlaying(userId)) return;


            var game = games.FirstOrDefault(g => g.PlayerId == userId);
            var gamesID = games.IndexOf(game);

            var result = Game2048.MakeMove(game.Grid, direction);

            game.Score += result.GainedScore;
            game.State = result.State;
            game.Grid = result.Board;
            game.Move++;

            games[gamesID] = game;

            UpdateMessage(game, userId);
        }

        public static async void EndGame(ulong userId)
        {
            try
            {
                var game = games.FirstOrDefault(g => g.PlayerId == userId);
                games.Remove(game);

                var builder = new StringBuilder();
                builder.Append($":video_game: **Game Ended**");

                await game.Message.ModifyAsync(m => m.Content = builder.ToString());
                await game.Message.RemoveAllReactionsAsync();
            }
            catch
            {
                // ignored
            }

        }

        public static async void UpdateMessage(G2048Game game, ulong userId)
        {
            try
            {
                var globalAccount = Global.Client.GetUser(userId);
                var chanelGuil = game.Message.Channel as IGuildChannel;
                var account = GlobalUserAccounts.GetUserAccount(globalAccount);
                if (game.Score > account.Best2048Score)
                {
                    account.Best2048Score = game.Score;
                    GlobalUserAccounts.SaveAccounts(chanelGuil.Guild.Id);
                }
                var builder = new StringBuilder();
                builder.Append("Score: ");
                builder.Append(game.Score);
                builder.Append("\nMove: ");
                builder.Append(game.Move);
                builder.Append("\n```cpp\n");
                builder.Append(FormatBoard(game.Grid));

                if (game.State == Game2048.GameState.Lost)
                {
                    builder.Clear();
                    builder.Append("YOU LOST\n");
                    builder.Append("Score: ");
                    builder.Append(game.Score);

                    games.Remove(game);
                }
                else if (game.State == Game2048.GameState.Won)
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
            catch
            {
                // ignored
            }
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