using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
//using Discord.Audio;
using Discord.Commands;
using Wsashi.Handlers;
using Wsashi.Features.Trivia;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Modules.Games;
using Wsashi.Core.LevelingSystem;

namespace Wsashi
{
    class Program
    {
        public static DiscordSocketClient _client;
        //CommandHandler _handler;
        CommandService _service;


        private static void Main()
        {
            Console.Title = "Wsashi";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        private IServiceProvider services;

        private async Task StartAsync()
        {
            //_client = new DiscordSocketClient();
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 100
            });

            _client.Log += Logger.Log;
            _client.Ready += Timers.StartTimer;
            _client.ReactionAdded += OnReactionAdded;
            //_client.MessageReceived += MessageRewardHandler.MessageRewards;
            //_client.UserJoined += Events.Welcome;
            _client.UserJoined += Events.Autorole;
            _client.JoinedGuild += Events.GuildUtils;
            //_client.LeftGuild += Events.LeaveServer;
            //_client.UserLeft += Events.Goodbye;
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

            services = new ServiceCollection()
    .AddSingleton(_client)
    .AddSingleton<InteractiveService>()
    //.AddSingleton<Interactive>()
    .BuildServiceProvider();

            _service = new CommandService();
            await _service.AddModulesAsync(
                                assembly: Assembly.GetEntryAssembly(),
                services: services);

            //_handler = new CommandHandler();
            Initialize(_client);
            await _client.SetGameAsync(Config.bot.BotGameToSet, $"https://twitch.tv/{Config.bot.TwitchStreamer}", ActivityType.Streaming);
            await _client.SetStatusAsync(UserStatus.Online);
            //await _client.SetGameAsync("w!help | Wsashi");
            await Task.Delay(-1);

        }
        public void Initialize(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            var cmdConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            };
            _service.AddModulesAsync(
                                assembly: Assembly.GetEntryAssembly(),
                services: services);

            _client.MessageReceived += CommandHandler;
            Global.Client = client;
            _client.UserJoined += _client_UserJoined;
            _client.UserLeft += _client_UserLeft;
            _client.ReactionAdded += ReactionWasAdded;
        }

        public async Task CommandHandler(SocketMessage s)
        {
            _ = HandleCommandAsync(s);
        }
        private async Task HandleCommandAsync(SocketMessage s)
        {
            _ =  Events.FilterUnflip(s);
            _ = Modules.Management.SlowMode.HandleSlowMode(s);

            if (!(s is SocketUserMessage msg)) return;
            if (msg.Channel is SocketDMChannel) return;

            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            var prefix = config.CommandPrefix ?? Config.bot.cmdPrefix;

            var argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos) && (context.Guild == null || context.Guild.Id != 264445053596991498 || context.Guild.Id != 396440418507816960) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos) && (context.Guild == null || context.Guild.Id != 264445053596991498 || context.Guild.Id != 396440418507816960))//|| CheckPrefix(ref argPos, context))
            {
                var cmdSearchResult = _service.Search(context, argPos);
                if (cmdSearchResult.Commands.Count == 0) return;

                var executionTask = _service.ExecuteAsync(context, argPos, services);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                executionTask.ContinueWith(task =>
                {
                    if (task.Result.IsSuccess || task.Result.Error == CommandError.UnknownCommand) return;
                    const string errTemplate = "{0}, Error: {1}.";
                    var errMessage = string.Format(errTemplate, context.User.Mention, task.Result.ErrorReason);
                    context.Channel.SendMessageAsync(errMessage);
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            // Leveling up
            if (config.Leveling)
            {
                await Leveling.UserSentMessage((SocketGuildUser)context.User, (SocketTextChannel)context.Channel);
            }
        }

        private async Task ReactionWasAdded(global::Discord.Cacheable<global::Discord.IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (Timeouts.HasCommandTimeout(reaction.UserId, "REACTION", 1)) return;
            G1024ReactionInput.HandleReaction(reaction);
        }

        /*private static bool CheckPrefix(ref int argPos, SocketCommandContext context)
        {
            var prefixes = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id).Prefixes;
            var tmpArgPos = 0;
            var success = prefixes.Any(pre =>
            {
                if (!context.Message.Content.StartsWith(pre)) return false;
                tmpArgPos = pre.Length + 1;
                return true;
            });
            argPos = tmpArgPos;
            return success;
        }
        */
        private async Task _client_UserJoined(SocketGuildUser user)
        {
            _ = UserJoined(user);
        }
        private async Task UserJoined(SocketGuildUser user)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.WelcomeChannel == 0) return;
            if (!(_client.GetChannel(guildAcc.WelcomeChannel) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.WelcomeMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
        }

        private async Task _client_UserLeft(SocketGuildUser user)
        {
            _ = UserLeft(user);
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.LeaveChannel == 0) return;
            if (!(_client.GetChannel(guildAcc.LeaveChannel) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.LeaveMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
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
            catch
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

