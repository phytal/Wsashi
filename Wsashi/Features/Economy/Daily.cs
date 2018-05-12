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
        public enum DailyResult { Success, AlreadyRecieved }

        public static DailyResult GetDaily(SocketUser user)
        {
            var userAccount = GlobalUserAccounts.GetUserAccount(user);
            var difference = DateTime.Now - userAccount.LastDaily;

            if (difference.TotalHours < 24) return DailyResult.AlreadyRecieved;

            userAccount.Money += Constants.DailyMoneyGain;
            userAccount.LastDaily = DateTime.UtcNow;
            GlobalUserAccounts.SaveAccounts();
            return DailyResult.Success;
        }
    }
}
