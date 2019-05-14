using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Entities
{
    public class GlobalUserAccount : IGlobalAccount
    {
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

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int WinStreak { get; set; }

        public bool bookWM { get; set; }
        public bool bookPE { get; set; }
        public bool bookSD { get; set; }
        public bool bookDR { get; set; }
        public string weapon { get; set; }
        public string armour { get; set; }

        public int HasStrengthPots { get; set; }//stacks up to 25%
        public bool HasSpeedPots { get; set; }
        public int HasDebuffPots { get; set; }//stacks up to 25%
        public bool HasBookWM { get; set; }
        public bool HasBookPE { get; set; }
        public bool HasBookSD { get; set; }
        public bool HasBookDR { get; set; }
        public bool HasBasicTreatment { get; set; }
        public bool HasDivineShield { get; set; }
        public bool HasPoisonedWeapon { get; set; }
        public bool IsMeditate { get; set; }

        public bool blessingProtection { get; set; } //start off with free basic treatment
        public bool blessingSwiftness { get; set; } //small chance to attack twice each turn
        public bool blessingWar { get; set; } //10% more damage dealt
        public bool blessingStrength { get; set; } //Start off with 25 more health

        public string ActiveBlessing { get; set; }

        public string Attack1 { get; set; }
        public string Attack2 { get; set; }
        public string Attack3 { get; set; }
        public string Attack4 { get; set; }
        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();
        public List<string> Attacks { get; set; } = new List<string>();

        public Tuple<bool,int> Burn { get; set; } = new Tuple<bool, int>(false, 0);
        //no more duels stuff
        public uint LootBoxCommon { get; set; }
        public uint LootBoxRare { get; set; }
        public uint LootBoxUncommon { get; set; }
        public uint LootBoxEpic { get; set; }
        public uint LootBoxLegendary { get; set; }

        public uint XP { get; set; }

        public DateTime LastXPMessage { get; set; } = DateTime.UtcNow;

        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 50);
            }
        }

        public int UncommonLB { get; set; }
        public int RareLB { get; set; }
        public int EpicLB { get; set; }
        public int LegendaryLB { get; set; }
        public int UncommonLBD { get; set; }//D is for Duels
        public int RareLBD { get; set; }
        public int EpicLBD { get; set; }
        public int LegendaryLBD { get; set; }

        public ulong Id { get; set; }

        public string OverwatchID { get; set; }

        public string OverwatchRegion { get; set; }

        public string OverwatchPlatform { get; set; }

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