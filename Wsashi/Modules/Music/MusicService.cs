using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Handlers;

namespace Wsashi.Modules.Music
{
    public class MusicService
    {
        private readonly DiscordShardedClient _client;
        private readonly LavaShardClient _lavaShardClient;
        private readonly LavaRestClient _lavaRestClient;
        private LavaPlayer _lavaPlayer;

        public MusicService(LavaRestClient lavaRestClient, DiscordShardedClient client, LavaShardClient lavaShardClient)
        {
            _client = client;
            _lavaRestClient = lavaRestClient;
            _lavaShardClient = lavaShardClient;
        }

        private readonly Lazy<ConcurrentDictionary<ulong, AudioOptions>> _lazyOptions
            = new Lazy<ConcurrentDictionary<ulong, AudioOptions>>();

        private ConcurrentDictionary<ulong, AudioOptions> Options
            => _lazyOptions.Value;

        public async Task<Embed> JoinOrPlayAsync(SocketGuildUser user, IMessageChannel textChannel, ulong guildId, string query = null)
        {
            if (user.VoiceChannel == null)
                return await EmbedHandler.CreateErrorEmbed("Music Join/Play", "You must first join a voice channel!");

            if(Options.TryGetValue(user.Guild.Id, out var options) && options.Summoner.Id != user.Id)
                return await EmbedHandler.CreateErrorEmbed("Music, Join/Play", $"I can't join another voice channel until {options.Summoner} disconnects me.");
            try
            {
                _lavaPlayer = _lavaShardClient.GetPlayer(guildId);
                if (_lavaPlayer == null)
                {

                    await _lavaShardClient.ConnectAsync(user.VoiceChannel);
                    Options.TryAdd(user.Guild.Id, new AudioOptions
                    {
                        Summoner = user
                    });
                    _lavaPlayer = _lavaShardClient.GetPlayer(guildId);
                }

                LavaTrack track;
                var search = await _lavaRestClient.SearchYouTubeAsync(query);

                if (search.LoadType == LoadType.NoMatches && query != null)
                    return await EmbedHandler.CreateErrorEmbed("Music", $"I wasn't able to find anything for {query}.");
                if (search.LoadType == LoadType.LoadFailed && query != null)
                    return await EmbedHandler.CreateErrorEmbed("Music", $"I failed to load {query}.");
                
                track = search.Tracks.FirstOrDefault();

                if (_lavaPlayer.CurrentTrack != null && _lavaPlayer.IsPlaying || _lavaPlayer.IsPaused)
                {
                    _lavaPlayer.Queue.Enqueue(track);
                    return await EmbedHandler.CreateBasicEmbed("Music", $"{track.Title} has been added to queue.");
                }
                await _lavaPlayer.PlayAsync(track);
                return await EmbedHandler.CreateMusicEmbed("Music", $"Now Playing: {track.Title}\nUrl: {track.Uri}");
            }
            catch (Exception e)
            {
                return await EmbedHandler.CreateErrorEmbed("Music, Join/Play", e.Message);
            }
        }

        public async Task<Embed> LeaveAsync(SocketGuildUser user, ulong guildId)
        {
            try
            {
                var player = _lavaShardClient.GetPlayer(guildId);

                if (player.IsPlaying)
                    await player.StopAsync();

                var channelName = player.VoiceChannel.Name;
                await _lavaShardClient.DisconnectAsync(user.VoiceChannel);
                return await EmbedHandler.CreateBasicEmbed("Music", $"Disconnected from {channelName}.");
            }

            catch (InvalidOperationException e)
            {
                return await EmbedHandler.CreateErrorEmbed("Leaving Music Channel", e.Message);
            }
        }
       
        public async Task<Embed> ListAsync(ulong guildId) 
        {
            var config = GlobalGuildAccounts.GetGuildAccount(guildId);
            var cmdPrefix = config.CommandPrefix;
            try
            {
                var descriptionBuilder = new StringBuilder();

                var player = _lavaShardClient.GetPlayer(guildId);
                if (player == null)
                    return await EmbedHandler.CreateErrorEmbed("Music Queue", $"Could not aquire music player.\nAre you using the music service right now? See `{cmdPrefix}h m` for proper usage.");

                if (player.IsPlaying)
                {

                    if (player.Queue.Count < 1 && player.CurrentTrack != null)
                    {
                        return await EmbedHandler.CreateBasicEmbed($"Now Playing: {player.CurrentTrack.Title}", "There are no other items in the queue.");
                    }
                    else
                    {
                        var trackNum = 2;
                        foreach (LavaTrack track in player.Queue.Items)
                        {
                            if (trackNum == 2) { descriptionBuilder.Append($"Up Next: [{track.Title}]({track.Uri})\n"); trackNum++; }
                            else { descriptionBuilder.Append($"#{trackNum}: [{track.Title}]({track.Uri})\n"); trackNum++; }
                        }
                        return await EmbedHandler.CreateBasicEmbed("Music Playlist", $"Now Playing: [{player.CurrentTrack.Title}]({player.CurrentTrack.Uri})\n{descriptionBuilder.ToString()}");
                    }
                }
                else
                {
                    return await EmbedHandler.CreateErrorEmbed("Music Queue", "Player doesn't seem to be playing anything right now. If this is an error, Please contact Stage in the Kaguya support server.");
                }
            }
            catch (Exception ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Music, List", ex.Message);
            }

        }

        public async Task<Embed> SkipTrackAsync(ulong guildId)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(guildId);
            var cmdPrefix = config.CommandPrefix;

            try
            {
                var player = _lavaShardClient.GetPlayer(guildId);
                if (player == null)
                    return await EmbedHandler.CreateErrorEmbed("Music, List", $"Could not aquire player.\nAre you using the bot right now? check{cmdPrefix}Help for info on how to use the bot.");
                if (player.Queue.Count == 1)
                    return await EmbedHandler.CreateMusicEmbed("Music Skipping", "This is the last song in the queue, so I have stopped playing."); await player.StopAsync();
                if (player.Queue.Count == 0)
                    return await EmbedHandler.CreateErrorEmbed("Music Skipping", "There are no songs to skip!");
                else
                {
                    try
                    {
                        var currentTrack = player.CurrentTrack;
                        await player.SkipAsync();
                        return await EmbedHandler.CreateBasicEmbed("Music Skip", $"Successfully skipped {currentTrack.Title}");
                    }
                    catch (Exception ex)
                    {
                        return await EmbedHandler.CreateErrorEmbed("Music Skipping Exception:", ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Music Skip", ex.ToString());
            }
        }

        public async Task<Embed> VolumeAsync(ulong guildId, int volume)
        {
            if (volume >= 150 || volume <= 0)
            {
                return await EmbedHandler.CreateErrorEmbed($"Music Volume", $"Volume must be between 1 and 149.");
            }
            try
            {
                var player = _lavaShardClient.GetPlayer(guildId);
                await player.SetVolumeAsync(volume);
                return await EmbedHandler.CreateBasicEmbed($"🔊 Music Volume", $"Volume has been set to {volume}.");
            }
            catch (InvalidOperationException ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Music Volume", $"{ex.Message}", "Please contact Stage in the support server if this is a recurring issue.");
            }
        }

        public async Task<Embed> Pause(ulong guildId)
        {
            try
            {
                var player = _lavaShardClient.GetPlayer(guildId);
                if (player.IsPaused)
                {
                    await player.ResumeAsync();
                    return await EmbedHandler.CreateMusicEmbed("▶️ Music", $"**Resumed:** Now Playing {player.CurrentTrack.Title}");
                }
                else
                {
                    await player.PauseAsync();
                    return await EmbedHandler.CreateMusicEmbed("⏸️ Music", $"**Paused:** {player.CurrentTrack.Title}");
                }
            }
            catch (InvalidOperationException e)
            {
                return await EmbedHandler.CreateErrorEmbed("Music Play/Pause", e.Message);
            }
        }

        public async Task OnTrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext())
                return;

            if (!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                await player.TextChannel?.SendMessageAsync($"There are no more songs left in queue.");
                return;
            }

            await player.PlayAsync(nextTrack);

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithDescription($"**Finished Playing: `{track.Title}`\nNow Playing: `{nextTrack.Title}`**");
            //embed.WithColor();
            await player.TextChannel.SendMessageAsync("", false, embed.Build());
            await player.TextChannel.SendMessageAsync(player.ToString());
        }

        public Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
