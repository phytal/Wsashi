using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Entities
{
    public class GlobalGuildUserAccount : IGlobalAccount
    {
        public string UniqueId { get; set; }

        public ulong Id { get; set; }

        public uint Reputation { get; set; }

        public uint XP { get; set; }
        //duels stuff
        public int Health { get; set; }

        public int Meds { get; set; }

        public bool Blocking { get; set; }

        public bool Deflecting { get; set; }

        public string whosTurn { get; set; }

        public string whoWaits { get; set; }

        public string placeHolder { get; set; }

        public bool Fighting { get; set; }

        public ulong OpponentId { get; set; }
        public string OpponentName { get; set; }
        //no more duels stuff
        public DateTime LastRep { get; set; } = DateTime.UtcNow.AddDays(-2);

        public DateTime LastXPMessage { get; set; } = DateTime.UtcNow;

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 200);
            }
        }

        public uint NumberOfWarnings { get; set; }

        public List<string> Warnings { get; private set; } = new List<string>();
    }
}