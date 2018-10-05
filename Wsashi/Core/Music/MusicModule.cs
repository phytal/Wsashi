using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Wsashi.Core.Music
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {

        private static IAudioClient _audioclient;

        [Command("join")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        /*{
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await ReplyAsync("Error: Couldn't find channel to join");
                return;
            }
                var audioClient = await channel.ConnectAsync();
        }*/
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("You must be in a voice channel.");
                return;
            }

            Console.WriteLine("Channel: " + channel.Name + " - " + channel.Id);
            try
            {
                var audioClient = await channel.ConnectAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
        private void SendAsync(IAudioClient audioClient)
        {
            var ffmpeg = CreateStream("tune.mp3");
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = audioClient.CreatePCMStream(AudioApplication.Mixed);
            output.CopyToAsync(discord);
            discord.FlushAsync();
        }


        [Command("leave")]
        public async Task StopAsync(IVoiceChannel channel = null)
        {
            if (channel == null)
            {
                await ReplyAsync("Error: Couldn't find channel to leave");
                return;
            }
            await _audioclient.StopAsync();
        }

        [Command("play")]
        public async Task PlaySong([Remainder] string url)
        {
            await ReplyAsync("**Song:** " + (url) + "Has now started.");

            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(url);
            var output = ffmpeg.StandardOutput.BaseStream;
            var stream = _audioclient.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(stream);
            await stream.FlushAsync();

        }

        private Process CreateStream(string url)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C youtube-dl -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }

    }
}


