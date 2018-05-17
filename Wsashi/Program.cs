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
using Wsashi.Entities;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi
{
    internal class Program
    {
        public static DiscordSocketClient _client;
        CommandHandler _handler;
        CommandService _service;

        private static void Main()
        {
            Console.Title = "Wsashi";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            new Program().StartAsync().GetAwaiter().GetResult();
        }


        private async Task StartAsync()
        {


            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _client.Log += Logger.Log;
            _client.ReactionAdded += OnReactionAdded;
            _client.MessageReceived += MessageRewardHandler.HandleMessageRewards;
            _client.UserJoined += Events.Welcome;
            _client.UserJoined += Events.Autorole;
            _client.JoinedGuild += Events.GuildUtils;
            _client.UserLeft += Events.Goodbye;
            _client.UserBanned += Logging.HandleBans;
            _client.ChannelCreated += Logging.HandleChannelCreate;
            _client.ChannelDestroyed += Logging.HandleChannelDelete;
            _client.GuildUpdated += Logging.HandleServerUpdate;
            _client.MessageDeleted += Logging.HandleMessageDelete;
            _client.MessageUpdated += Logging.HandleMessageUpdate;
            _client.UserUpdated += Logging.HandleUserUpdate;
            _client.RoleCreated += Logging.HandleRoleCreation;
            _client.RoleUpdated += Logging.HandleRoleUpdate;
            _client.RoleDeleted += Logging.HandleRoleDelete;

            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await _client.SetGameAsync(Config.bot.BotGameToSet, $"https://twitch.tv/{Config.bot.TwitchStreamer}", StreamType.Twitch);
            await _client.SetStatusAsync(UserStatus.Online);
            //await _client.SetGameAsync("w!help | Wsashi");
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

