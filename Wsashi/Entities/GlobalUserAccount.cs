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

        public Dictionary<string, string> OverwatchInfo { get; set; } = new Dictionary<string, string>();

        public ulong Money { get; set; } = 1;

        public int Best2048Score { get; set; }

        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);

        public DateTime LastMessage { get; set; } = DateTime.UtcNow;

        public List<ReminderEntry> Reminders { get; internal set; } = new List<ReminderEntry>();
        /* Add more values to store */
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
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