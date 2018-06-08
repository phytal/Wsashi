using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsashi.Entities
{
    public class GlobalWasagotchiUserAccount : IGlobalAccount
    {

        public ulong Id { get; set; }

        public bool Have { get; set; }

        public string Name { get; set; }

        public uint Hunger { get; set; }

        public DateTime BoughtSince { get; set; }
    
        public uint XP { get; set; }

        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 200);
            }
        }

        public uint RoomCost
        {
            get
            {
                return ((rLvl + 1) * 275);
            }
        }

        public uint rLvl { get; set; }

        public uint Attention { get; set; }

        //public string Owner { get; set; }

        public uint Waste { get; set; }

        public bool Sick { get; set; }

        public DateTime SickSince { get; set; }

        public bool RanAway { get; set; }

        public string pfp { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }
}