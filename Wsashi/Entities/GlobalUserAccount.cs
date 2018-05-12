using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Entities
{
    public class GlobalUserAccount : IGlobalAccount
    {
    
        public ulong Id { get; set; }

        public uint Points { get; set; }

        public uint XP { get; set; }

        public ulong Money { get; set; } = 1;

        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);

        public DateTime LastMessage { get; set; } = DateTime.UtcNow;

        public List<ReminderEntry> Reminders { get; internal set; } = new List<ReminderEntry>();
        /* Add more values to store */
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 200);
            }
        }

        public bool IsMuted { get; set; }

        public uint NumberOfWarnings { get; set; }
    }
    public struct ReminderEntry
    {
        public DateTime DueDate;
        public string Description;

        public ReminderEntry(DateTime dueDate, string description)
        {
            DueDate = dueDate;
            Description = description;
        }
    }
}