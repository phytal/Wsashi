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
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;

            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            int argPos = 0;
            if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || CheckPrefix(ref argPos, context))
                {
                    var cmdSearchResult = _service.Search(context, argPos);
                    if (cmdSearchResult.Commands.Count == 0) return;

                    var executionTask = _service.ExecuteAsync(context, argPos);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    executionTask.ContinueWith(task =>
                    {
                        if (!task.Result.IsSuccess && task.Result.Error != CommandError.UnknownCommand)
                        {
                            string errTemplate = "{0}, Error: {1}.";
                            string errMessage = String.Format(errTemplate, context.User.Mention, task.Result.ErrorReason);
                            context.Channel.SendMessageAsync(errMessage);
                        }
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

        private static bool CheckPrefix(ref int argPos, SocketCommandContext context)
        {
            var prefixes = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id).Prefixes;
            var tmpArgPos = 0;
            var success = prefixes.Any(pre =>
            {
                if (context.Message.Content.StartsWith(pre))
                {
                    tmpArgPos = pre.Length + 1;
                    return true;
                }
                return false;
            });
            argPos = tmpArgPos;
            return success;
        }

        private async Task ReactionWasAdded(global::Discord.Cacheable<global::Discord.IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (Timeouts.HasCommandTimeout(reaction.UserId, "REACTION", 1)) return;
            G1024ReactionInput.HandleReaction(reaction);
        }

        private async Task _client_UserJoined(SocketGuildUser user)
        {
            var possibleMessages = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id).WelcomeMessages;
            var channel = _client.GetChannel(414989014551232512) as SocketTextChannel;
            var member = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MEMBER").FirstOrDefault() as SocketRole;

            await user.AddRoleAsync(member);
            await channel.SendMessageAsync($"Welcome **{user.Username}** to **{user.Guild.Name}**! Have fun!");
        }

        private async Task _client_UserLeft(SocketGuildUser user)
        {
            if (user.Guild.Name == "Phytal's Public Discord")
            {
                var channel = _client.GetChannel(414989014551232512) as SocketTextChannel;
                await channel.SendMessageAsync($"**{user.Username}** has left **{user.Guild.Name}**.. :cry: :wave: ");
            }
        }
    }
}
