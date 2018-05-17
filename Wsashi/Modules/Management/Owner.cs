using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using Wsashi.Helpers;
using Wsashi.Core.Modules;
using Wsashi;
using Wsashi.Features.GlobalAccounts;

namespace SIVA.Core.Modules.Management
{
    public class Owner : WsashiModule
    {

        public void KillProgram() => Kill(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.

        public void Kill() => KillProgram(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.

        [Command("Shutdown")]
        [RequireOwner]
        public async Task Shutdown()
        {
            var client = Program._client;

            await client.LogoutAsync();
            await client.StopAsync();
            KillProgram();

        }

        [Command("VerifyGuild"), Alias("Verify")]
        [RequireOwner]
        public async Task VerifyGuildById(ulong guildId = 0)
        {
            var id = guildId;
            if (id == 0) id = Context.Guild.Id;
            var config = GlobalGuildAccounts.GetGuildAccount(id);
            config.VerifiedGuild = true;
            var embed = new EmbedBuilder()
                .WithDescription("Successfully verified this server.")
                .WithColor(37, 152, 255);
            await ReplyAsync("", false, embed);
        }

        [Command("Stream")]
        [RequireOwner]
        public async Task SetBotStream(string streamer, [Remainder]string streamName)
        {
            await Program._client.SetGameAsync(streamName, $"https://twitch.tv/{streamer}", StreamType.Twitch);
            var embed = MiscHelpers.CreateEmbed(Context, $"Set the stream name to **{streamName}**, and set the streamer to <https://twitch.tv/{streamer}>!");
            await MiscHelpers.SendMessage(Context, embed);
        }


        [Command("Game")]
        [RequireOwner]
        public async Task SetBotGame([Remainder] string game)
        {
            var client = Program._client;

            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's game to {game}");
            embed.WithColor(37, 152, 255);
            await client.SetGameAsync(game);
            await ReplyAsync("", false, embed);
        }

        [Command("setVersion")]
        [RequireOwner]
        public async Task SetBotVersion([Remainder] string version)
        {
            Config.bot.Version = version;
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's version to {version}");
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", false, embed);
        }

        [Command("Status")]
        [RequireOwner]
        public async Task SetBotStatus(string status)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the status to {status}.");

            embed.WithColor(37, 152, 255);

            var client = Program._client;

            switch (status)
            {
                case "dnd":
                    await client.SetStatusAsync(UserStatus.DoNotDisturb);
                    break;
                case "idle":
                    await client.SetStatusAsync(UserStatus.Idle);
                    break;
                case "online":
                    await client.SetStatusAsync(UserStatus.Online);
                    break;
                case "offline":
                    await client.SetStatusAsync(UserStatus.Invisible);
                    break;
            }

            await ReplyAsync("", false, embed);
        }

        [Command("LeaveServer")]
        [RequireOwner]
        public async Task LeaveServer()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", false, embed);
            await Context.Guild.LeaveAsync();
        }

        [Command("ServerCount"), Alias("Sc")]
        [RequireOwner]
        public async Task ServerCountStream()
        {
            var client = Program._client;
            var guilds = client.Guilds.Count;
            var embed = new EmbedBuilder();
            embed.WithDescription("Done.");
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", false, embed);
            await client.SetGameAsync($"in {guilds} servers!", $"https://twitch.tv/{Config.bot.TwitchStreamer}", StreamType.Twitch);

        }
    }
}