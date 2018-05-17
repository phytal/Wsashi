using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Wsashi.Core.LevelingSystem;
using Wsashi.Modules.Games;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi
{
    internal class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            Global.Client = client;
            _client.UserJoined += _client_UserJoined;
            _client.UserLeft += _client_UserLeft;
            _client.ReactionAdded += ReactionWasAdded;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Channel is SocketDMChannel) return;

            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            var prefix = config.CommandPrefix ?? Config.bot.cmdPrefix;

            var argPos = 0;
            if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || msg.HasStringPrefix(prefix, ref argPos))//|| CheckPrefix(ref argPos, context))
            {
                var cmdSearchResult = _service.Search(context, argPos);
                if (cmdSearchResult.Commands.Count == 0) return;

                var executionTask = _service.ExecuteAsync(context, argPos);

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

            // Mute check
            var userAccount = GlobalUserAccounts.GetUserAccount(context.User);
            if (userAccount.IsMuted)
            {
                await context.Message.DeleteAsync();
                return;
            }

            // Leveling up
            Leveling.UserSentMessage((SocketGuildUser)context.User, (SocketTextChannel)context.Channel);

            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }

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
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.WelcomeChannel == 0) return;
            if (!(_client.GetChannel(guildAcc.WelcomeChannel) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.LeaveMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
        }
    }
}
