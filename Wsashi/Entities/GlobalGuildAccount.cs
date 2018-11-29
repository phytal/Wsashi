using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Entities
{
    public class GlobalGuildAccount : IGlobalAccount
    {
        #region JSONValueDeclaration
        public GlobalGuildAccount()
        {
            AntilinkIgnoredChannels = new List<ulong>();
            SelfRoles = new List<string>();

        }

        public ulong Id { get; set; }

        public ulong AnnouncementChannelId { get; set; }

        //public List<string> Prefixes { get; set; } = new List<string>();

        public List<string> WelcomeMessages { get; set; } = new List<string> { };

        public List<string> LeaveMessages { get; set; } = new List<string>();

        public bool Filter { get; set; }

        public bool Antilink { get; set; }

        public bool Unflip { get; set; }

        public string LevelingMsgs { get; set; }

        public List<ulong> AntilinkIgnoredChannels { get; set; }

        public List<ulong> FilterIgnoredChannels { get; set; }

        public string HelperRoleName { get; set; }

        public ulong HelperRole { get; set; }

        public ulong ModRole { get; set; }

        public ulong AdminRole { get; set; }

        public string ModRoleName { get; set; }

        public string AdminRoleName { get; set; }

        public string CommandPrefix { get; set; }

        public bool Leveling { get; set; }

        public ulong WelcomeChannel { get; set; }

        public ulong LeaveChannel { get; set; }

        public string WelcomeMessage { get; set; }

        public string LeavingMessage { get; set; }

        public bool MassPingChecks { get; set; }

        public List<string> SelfRoles { get; set; }

        public ulong GuildOwnerId { get; set; }

        public string Autorole { get; set; }

        public bool VerifiedGuild { get; set; }

        public ulong ServerLoggingChannel { get; set; }

        public bool IsServerLoggingEnabled { get; set; }

        public ulong SlowModeCooldown { get; set; }

        public bool IsSlowModeEnabled { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
        /* Add more values to store */
        #endregion
    }
}