using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Overwatch
{
    public class UserStats : ModuleBase
    {
        [Command("owstats", RunMode = RunMode.Async)]
        [Summary("Get a Overwatch user's statistics. Usage: w!owstats <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstats Phytal-1427 pc")]
        [Alias("ows", "overwatchstats")]
        [Cooldown(10)]

        public async Task GetOwStats([Remainder] string message)
        {
            string platform = message.Split(' ').Last();
            string user = message.Remove(message.IndexOf(platform));

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + user + "/stats?playform=" + platform);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);


            string cdd = dataObject.eu.stats.competitive.game_stats.damage_done.ToString();
            string cd = dataObject.eu.stats.competitive.game_stats.deaths.ToString();
            string celims = dataObject.eu.stats.competitive.game_stats.eliminations.ToString();
            string cgp = dataObject.eu.stats.competitive.game_stats.games_played.ToString();
            string cgt = dataObject.eu.stats.competitive.game_stats.games_tied.ToString();
            string cgw = dataObject.eu.stats.competitive.game_stats.games_won.ToString();
            string cgl = dataObject.eu.stats.competitive.game_stats.games_lost.ToString();
            string ckpd = dataObject.eu.stats.competitive.game_stats.kpd.ToString();
            string cm = dataObject.eu.stats.competitive.game_stats.medals.ToString();
            string cmg = dataObject.eu.stats.competitive.game_stats.medals_gold.ToString();
            string cms = dataObject.eu.stats.competitive.game_stats.medals_silver.ToString();
            string cmb = dataObject.eu.stats.competitive.game_stats.medals_bronze.ToString();
            string ckbs = dataObject.eu.stats.competitive.game_stats.kill_streak_best.ToString();
            string chd =  dataObject.eu.stats.competitive.game_stats.healing_done.ToString();

            string cwr = dataObject.eu.stats.competitive.overall_stats.win_rate.ToString();
            string clvl = dataObject.eu.stats.competitive.overall_stats.level.ToString();
            string cpres = dataObject.eu.stats.competitive.overall_stats.prestige.ToString();
            string ccomprank = dataObject.eu.stats.competitive.overall_stats.comprank.ToString();
            string tier = dataObject.eu.stats.competitive.overall_stats.tier.ToString();
            string avatar = dataObject.eu.stats.competitive.overall_stats.avatar.ToString();
            string ti = dataObject.eu.stats.competitive.overall_stats.tier_image.ToString();
            string ri = dataObject.eu.stats.competitive.overall_stats.rank_image.ToString();

            string qdd = dataObject.eu.stats.quickplay.game_stats.damage_done.ToString();
            string qd = dataObject.eu.stats.quickplay.game_stats.deaths.ToString();
            string qelims = dataObject.eu.stats.quickplay.game_stats.eliminations.ToString();
            string qgw = dataObject.eu.stats.quickplay.game_stats.games_won.ToString();
            string qkpd = dataObject.eu.stats.quickplay.game_stats.kpd.ToString();
            string qm = dataObject.eu.stats.quickplay.game_stats.medals.ToString();
            string qmg = dataObject.eu.stats.quickplay.game_stats.medals_gold.ToString();
            string qms = dataObject.eu.stats.quickplay.game_stats.medals_silver.ToString();
            string qmb = dataObject.eu.stats.quickplay.game_stats.medals_bronze.ToString();
            string qkbs = dataObject.eu.stats.quickplay.game_stats.kill_streak_best.ToString();
            string qhd = dataObject.eu.stats.quickplay.game_stats.healing_done.ToString();

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OWAPI API",
                IconUrl = ri
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{user}'s Overwatch Profile",
                IconUrl = ti
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddInlineField("Competitive Game Stats", $"Games Played: **{cgp}**\nGames Won: **{cgw}**\nGames Lost: **{cgl}**\nGames Tied: **{cgt}**\nEliminations: **{celims}**\nDeaths: **{cd}**\nWin Rate: **{cwr}**\nKills per Death: **{ckpd}**\nBest Kill Streak: **{ckbs}**\nDamage Done: **{cdd}**\nHealing Done: **{chd}**");
            embed.AddInlineField("Quickplay Game Stats", $"Games Won: **{qgw}**\nEliminations: **{qelims}**\nDeaths: **{qd}**\nKills per Death: **{qkpd}**\nBest Kill Streak: **{qkbs}**\nDamage Done: **{qdd}**\nHealing Done: **{qhd}**");
            embed.AddInlineField("Competitive Medals", $"Total Medals: **{cm}**\n:first_place: Gold Medals: **{cmg}**\n:second_place: Silver Medals: **{cms}**\n:third_place: Bronze Medals: **{cmb}**");
            embed.AddInlineField("Quickplay Medals", $"Total Medals: **{qm}**\n:first_place: Gold Medals: **{qmg}**\n:second_place: Silver Medals: **{qms}**\n:third_place: Bronze Medals: **{qmb}**");
            embed.AddInlineField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("owstatsqp", RunMode = RunMode.Async)]
        [Summary("Get a Overwatch user's Quickplay statistics. Usage: w!owstats <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstatsqp Phytal-1427 pc")]
        [Alias("owsqp", "overwatchstatsqp", "owsquickplay")]
        [Cooldown(10)]

        public async Task GetOwQpStats([Remainder] string message)
        {
            string platform = message.Split(' ').Last();
            string user = message.Remove(message.IndexOf(platform));

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + user + "/stats?playform=" + platform);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string cwr = dataObject.eu.stats.quickplay.overall_stats.win_rate.ToString();
            string clvl = dataObject.eu.stats.quickplay.overall_stats.level.ToString();
            string cpres = dataObject.eu.stats.quickplay.overall_stats.prestige.ToString();
            string tier = dataObject.eu.stats.quickplay.overall_stats.tier.ToString();
            string avatar = dataObject.eu.stats.quickplay.overall_stats.avatar.ToString();
            string ri = dataObject.eu.stats.quickplay.overall_stats.rank_image.ToString();
            string ccomprank = dataObject.eu.stats.competitive.overall_stats.comprank.ToString();

            string qdd = dataObject.eu.stats.quickplay.game_stats.damage_done.ToString();
            string qd = dataObject.eu.stats.quickplay.game_stats.deaths.ToString();
            string qelims = dataObject.eu.stats.quickplay.game_stats.eliminations.ToString();
            string qgw = dataObject.eu.stats.quickplay.game_stats.games_won.ToString();
            string qkpd = dataObject.eu.stats.quickplay.game_stats.kpd.ToString();
            string qm = dataObject.eu.stats.quickplay.game_stats.medals.ToString();
            string qmg = dataObject.eu.stats.quickplay.game_stats.medals_gold.ToString();
            string qms = dataObject.eu.stats.quickplay.game_stats.medals_silver.ToString();
            string qmb = dataObject.eu.stats.quickplay.game_stats.medals_bronze.ToString();
            string qkbs = dataObject.eu.stats.quickplay.game_stats.kill_streak_best.ToString();
            string qhd = dataObject.eu.stats.quickplay.game_stats.healing_done.ToString();

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OWAPI API",
                IconUrl = ri
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{user}'s Quickplay Overwatch Profile"
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddInlineField("Quickplay Game Stats", $"Games Won: **{qgw}**\nEliminations: **{qelims}**\nDeaths: **{qd}**\nKills per Death: **{qkpd}**\nBest Kill Streak: **{qkbs}**\nDamage Done: **{qdd}**\nHealing Done: **{qhd}**");
            embed.AddInlineField("Quickplay Medals", $"Total Medals: **{qm}**\n:first_place: Gold Medals: **{qmg}**\n:second_place: Silver Medals: **{qms}**\n:third_place: Bronze Medals: **{qmb}**");
            embed.AddInlineField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("owstatscomp", RunMode = RunMode.Async)]
        [Summary("Get a Overwatch user's Competitive statistics. Usage: w!owstats <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstatscomp Phytal-1427 pc")]
        [Alias("owsc", "overwatchstatscomp", "owscompetitive")]
        [Cooldown(10)]

        public async Task GetOwCompStats([Remainder] string message)
        {
            string platform = message.Split(' ').Last();
            string user = message.Remove(message.IndexOf(platform));

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + user + "/stats?playform=" + platform);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);


            string cdd = dataObject.eu.stats.competitive.game_stats.damage_done.ToString();
            string cd = dataObject.eu.stats.competitive.game_stats.deaths.ToString();
            string celims = dataObject.eu.stats.competitive.game_stats.eliminations.ToString();
            string cgp = dataObject.eu.stats.competitive.game_stats.games_played.ToString();
            string cgt = dataObject.eu.stats.competitive.game_stats.games_tied.ToString();
            string cgw = dataObject.eu.stats.competitive.game_stats.games_won.ToString();
            string cgl = dataObject.eu.stats.competitive.game_stats.games_lost.ToString();
            string ckpd = dataObject.eu.stats.competitive.game_stats.kpd.ToString();
            string cm = dataObject.eu.stats.competitive.game_stats.medals.ToString();
            string cmg = dataObject.eu.stats.competitive.game_stats.medals_gold.ToString();
            string cms = dataObject.eu.stats.competitive.game_stats.medals_silver.ToString();
            string cmb = dataObject.eu.stats.competitive.game_stats.medals_bronze.ToString();
            string ckbs = dataObject.eu.stats.competitive.game_stats.kill_streak_best.ToString();
            string chd = dataObject.eu.stats.competitive.game_stats.healing_done.ToString();

            string cwr = dataObject.eu.stats.competitive.overall_stats.win_rate.ToString();
            string clvl = dataObject.eu.stats.competitive.overall_stats.level.ToString();
            string cpres = dataObject.eu.stats.competitive.overall_stats.prestige.ToString();
            string ccomprank = dataObject.eu.stats.competitive.overall_stats.comprank.ToString();
            string tier = dataObject.eu.stats.competitive.overall_stats.tier.ToString();
            string avatar = dataObject.eu.stats.competitive.overall_stats.avatar.ToString();
            string ti = dataObject.eu.stats.competitive.overall_stats.tier_image.ToString();
            string ri = dataObject.eu.stats.competitive.overall_stats.rank_image.ToString();

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OWAPI API",
                IconUrl = ri
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{user}'s Competitive Overwatch Profile",
                IconUrl = ti
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddInlineField("Competitive Game Stats", $"Games Played: **{cgp}**\nGames Won: **{cgw}**\nGames Lost: **{cgl}**\nGames Tied: **{cgt}**\nEliminations: **{celims}**\nDeaths: **{cd}**\nWin Rate: **{cwr}**\nKills per Death: **{ckpd}**\nBest Kill Streak: **{ckbs}**\nDamage Done: **{cdd}**\nHealing Done: **{chd}**");
            embed.AddInlineField("Competitive Medals", $"Total Medals: **{cm}**\n:first_place: Gold Medals: **{cmg}**\n:second_place: Silver Medals: **{cms}**\n:third_place: Bronze Medals: **{cmb}**");
            embed.AddInlineField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }
}
