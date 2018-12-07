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
using System.Net;
using System.IO;
using Wsashi.Handlers;

namespace Wsashi.Core.Modules.Management
{
    public class Owner : WsashiModule
    {

        public void KillProgram() => Kill(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.

        public void Kill() => KillProgram(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.

        [Command("Shutdown")]
        [Summary("Shuts down Wsashi :((")]
        [RequireOwner]
        public async Task Shutdown()
        {
            var client = Program._client;

            await client.LogoutAsync();
            await client.StopAsync();
            KillProgram();

        }

        [Command("VerifyGuild"), Alias("Verify")]
        [Summary("Verifys the current guild or by ID")]
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
            await ReplyAsync("", embed: embed.Build());
        }

        [Command("Stream")]
        [Summary("Sets what Wsashi is streaming")]
        [RequireOwner]
        public async Task SetBotStream(string streamer, [Remainder]string streamName)
        {
            await Program._client.SetGameAsync(streamName, $"https://twitch.tv/{streamer}", ActivityType.Streaming);
            var embed = MiscHelpers.CreateEmbed(Context, $"Set the stream name to **{streamName}**, and set the streamer to <https://twitch.tv/{streamer}>!").WithColor(37, 152, 255);
            await MiscHelpers.SendMessage(Context, embed);
        }


        [Command("Game")]
        [Summary("Sets the game Wsashi is playing")]
        [RequireOwner]
        public async Task SetBotGame([Remainder] string game)
        {
            var client = Program._client;

            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's game to {game}");
            embed.WithColor(37, 152, 255);
            await client.SetGameAsync(game);
            await ReplyAsync("", embed: embed.Build());
        }

        [Command("setVersion")]
        [Summary("Set Wsashi's version")]
        [RequireOwner]
        public async Task SetBotVersion([Remainder] string version)
        {
            Config.bot.Version = version;
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's version to {version}");
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", embed: embed.Build());
        }

        [Command("Status")]
        [Summary("Sets Wsashi's user status")]
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

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("LeaveServer")]
        [Summary("Make's Wsashi leave the server")]
        [RequireOwner]
        public async Task LeaveServer()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", embed: embed.Build());
            await Context.Guild.LeaveAsync();
        }

        [Command("ServerCount"), Alias("Sc")]
        [Summary("Sets Wsashi's game/stream the number of guilds in")]
        [RequireOwner]
        public async Task ServerCountStream()
        {
            var client = Program._client;
            var guilds = client.Guilds.Count;
            var embed = new EmbedBuilder();
            embed.WithDescription($"Done. In {guilds}");
            embed.WithColor(37, 152, 255);
            await ReplyAsync("", embed: embed.Build());
            await client.SetGameAsync($"w!help | in {guilds} servers!", $"https://twitch.tv/{Config.bot.TwitchStreamer}", ActivityType.Streaming);

        }

        [Command("setAvatar"), Remarks("Sets the bots Avatar")]
        [RequireOwner]
        public async Task SetAvatar(string link)
        {
            var s = Context.Message.DeleteAsync();

            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(link);

                var stream = new MemoryStream(imageBytes);

                var image = new Image(stream);
                await Context.Client.CurrentUser.ModifyAsync(k => k.Avatar = image);
            }
            catch (Exception)
            {
                var embed = EmbedHandler.CreateEmbed("Avatar", "Coult not set the avatar!", EmbedHandler.EmbedMessageType.Exception);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }
    }
}