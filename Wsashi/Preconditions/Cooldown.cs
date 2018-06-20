﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Wsashi.Preconditions
{
    public class Cooldown : PreconditionAttribute
    {
        TimeSpan CooldownLength { get; set; }
        bool AdminsAreLimited { get; set; }
        readonly ConcurrentDictionary<CooldownInfo, DateTime> _cooldowns = new ConcurrentDictionary<CooldownInfo, DateTime>();

        /// <summary>
        /// Sets the cooldown for a user to use this command
        /// </summary>
        /// <param name="seconds">Sets the cooldown in seconds.</param>
        /// <param name="adminsAreLimited">Set whether admins should have cooldowns between commands use.</param>
        public Cooldown(int seconds, bool adminsAreLimited = false)
        {
            CooldownLength = TimeSpan.FromSeconds(5);
            AdminsAreLimited = adminsAreLimited;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if the user is administrator and if it needs to apply cooldown for him.
            if (!AdminsAreLimited && context.User is IGuildUser user && user.GuildPermissions.Administrator)
                return Task.FromResult(PreconditionResult.FromSuccess());

            var key = new CooldownInfo(context.User.Id, command.GetHashCode());
            // Check if message with the same hash code is already in dictionary 
            if (_cooldowns.TryGetValue(key, out DateTime endsAt))
            {
                // Calculate the difference between current time and the time cooldown should end
                var difference = endsAt.Subtract(DateTime.UtcNow);
                var timeSpanString = string.Format("{0:%s} seconds", difference);
                // Display message if command is on cooldown
                if (difference.Ticks > 0)
                {
                    return Task.FromResult(PreconditionResult.FromError($"You can use this command in {timeSpanString}"));
                }
                // Update cooldown time
                var time = DateTime.UtcNow.Add(CooldownLength);
                _cooldowns.TryUpdate(key, time, endsAt);
            }
            else
            {
                _cooldowns.TryAdd(key, DateTime.UtcNow.Add(CooldownLength));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
        public struct CooldownInfo
        {
            public ulong UserId { get; }
            public int CommandHashCode { get; }

            public CooldownInfo(ulong userId, int commandHashCode)
            {
                UserId = userId;
                CommandHashCode = commandHashCode;
            }
        }
    }
}
