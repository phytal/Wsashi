using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API
{
    public class Fortnite : WsashiModule
    {
        public static async Task<string> SendWebRequest(string requestUrl)
        {
            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Add("TRN-API-Key", "c164aee8-a5f8-45f2-aa13-e3b5faa3375d");
                using (var response = await client.GetAsync(requestUrl))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return response.StatusCode.ToString();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        [Command("fn")]
        [Alias("fortnite")]
        [Summary("Get a Fortnite user's statistics.")]
        [Remarks("w!fn <platform (pc/xbox/psn)> <Fortnite username> Ex: w!fn pc Phytal")]
        [Cooldown(10)]
        public async Task GetFnStats([Remainder] string message)
        {
            string user = message.Split(' ').Last();
            string platform = message.Remove(message.IndexOf(user));
            platform = platform.Replace(' ', '/');
            var json = await SendWebRequest($"https://api.fortnitetracker.com/v1/profile{"/" + platform}" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string username = dataObject.epicUserHandle.ToString();
            string matchesPlayed = dataObject.lifeTimeStats[7].value.ToString();
            string kd = dataObject.lifeTimeStats[11].value.ToString();
            string platformm = dataObject.platformNameLong.ToString();
            string kills = dataObject.lifeTimeStats[10].value.ToString();
            string wins = dataObject.lifeTimeStats[8].value.ToString();
            string winpercent = dataObject.lifeTimeStats[9].value.ToString();
            string top5 = dataObject.lifeTimeStats[0].value.ToString();
            string top10 = dataObject.lifeTimeStats[3].value.ToString();
            string top25 = dataObject.lifeTimeStats[5].value.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://i.imgur.com/nFQPvWj.png");
            embed.WithTitle($"Stats for {username} on {platformm}");
            embed.AddField("Top 5s", top5, true);
            embed.AddField("Top 10s", top10, true);
            embed.AddField("Top 25s", top25, true);
            embed.AddField("Matches Played", matchesPlayed, true);
            embed.AddField("Wins", wins, true);
            embed.AddField("Win Percent", winpercent, true);
            embed.AddField("Kills", kills, true);
            embed.AddField("K/D", kd, true);
            embed.WithFooter("Powered by TRN Fortnite Tracker Network API");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}
