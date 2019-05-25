using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Modules.Music;

namespace Wsashi.Modules.Music
{
    public class Music : WsashiModule
    {
        private MusicService _musicService;

        public Music(MusicService musicService)
        {
            _musicService = musicService;
        }

        [Command("join")]
        public async Task MusicJoin()
            => await ReplyAsync("", false, await _musicService.JoinOrPlayAsync((SocketGuildUser)Context.User, Context.Channel, Context.Guild.Id));

        [Command("play")]
        public async Task MusicPlay([Remainder]string search) 
            => await ReplyAsync("", false, await _musicService.JoinOrPlayAsync((SocketGuildUser)Context.User, Context.Channel, Context.Guild.Id, search));

        [Command("leave")]
        public async Task MusicLeave()
            => await ReplyAsync("", false, await _musicService.LeaveAsync((SocketGuildUser)Context.User, Context.Guild.Id));

        [Command("queue")]
        public async Task MusicQueue()
            => await ReplyAsync("", false, await _musicService.ListAsync(Context.Guild.Id));

        [Command("skip")]
        public async Task SkipTrack()
            => await ReplyAsync("", false, await _musicService.SkipTrackAsync(Context.Guild.Id));

        [Command("volume")]
        public async Task Volume(int volume)
            => await ReplyAsync("", false, await _musicService.VolumeAsync(Context.Guild.Id, volume));

        [Command("Pause")]
        public async Task Pause()
           => await ReplyAsync("", false, await _musicService.Pause(Context.Guild.Id));

        [Command("Resume")]
        public async Task Resume()
            => await ReplyAsync("", false, await _musicService.Pause(Context.Guild.Id));
    }
}
