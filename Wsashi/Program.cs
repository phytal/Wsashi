using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Wsashi.Handlers;
using Watchdog.Modules;
using Wsashi.Features.Trivia;

namespace Wsashi
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;
        CommandService _service;

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _client.Log += Logger.Log;
            _client.ReactionAdded += OnReactionAdded;
            _client.MessageReceived += MessageRewardHandler.HandleMessageRewards;
            _client.MessageReceived += Filter.Filtering;
            _client.MessageReceived -= Filter.Filtering;
            _client.MessageReceived += Filter.LinkFiltering;

            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await _client.SetGameAsync("w!help | Wsashi");
            await Task.Delay(-1);
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (!reaction.User.Value.IsBot)
            {
                var msgList = Global.MessagesIdToTrack ?? new Dictionary<ulong, string>();
                if (msgList.ContainsKey(reaction.MessageId))
                {
                    if (reaction.Emote.Name == "➕")
                    {
                        var item = msgList.FirstOrDefault(k => k.Key == reaction.MessageId);
                        var embed = BlogHandler.SubscribeToBlog(reaction.User.Value.Id, item.Value);
                    }
                }
                // Checks if the rection is associated with a running game and if it is 
                // from the same user who ran the command - if so it handles it
                await TriviaGames.HandleReactionAdded(cache, reaction);
            }
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message ?? msg.Exception.ToString());
        }

        private async Task AttemptLogin()
        {
            try
            {
                await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            }
            catch (Exception e)
            {
                Console.WriteLine("The BOT Token is most likely incorrect.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        //private static IAudioClient _audioclient;

        //private async Task AttemptJoin()
        //{
            //_service.Log += Log;
            
            //try
            //{
                //IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
                //audioclient = await channel.ConnectAsync();

            //}
            //catch (Exception )
            //{
                //Console.WriteLine("Failed Joining");
                //Environment.Exit(0);
            //}
        //}
    }
}

