using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Economy
{
    public static class Daily
    {
        public struct DailyResult
        {
            public bool Success;
            public TimeSpan RefreshTimeSpan;
        }

        public static DailyResult GetDaily(ulong userId)
        {
            var account = GlobalUserAccounts.GetUserAccount(userId);
            var difference = DateTime.UtcNow - account.LastDaily.AddDays(1);

            if (difference.TotalHours < 0) return new DailyResult { Success = false, RefreshTimeSpan = difference };

            account.Money += Constants.DailyMoneyGain;
            account.LastDaily = DateTime.UtcNow;
            GlobalUserAccounts.SaveAccounts(userId);
            return new DailyResult { Success = true };
        }

        public static DailyResult GetRep(ulong userId)
        {
            var account = GlobalUserAccounts.GetUserAccount(userId);
            var difference = DateTime.UtcNow - account.LastRep.AddDays(1);

            if (difference.TotalHours < 0) return new DailyResult { Success = false, RefreshTimeSpan = difference };

            account.LastRep = DateTime.UtcNow;
            GlobalUserAccounts.SaveAccounts(userId);
            return new DailyResult { Success = true };
        }
    }
}
