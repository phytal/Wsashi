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
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Overwatch
{
    public class MyUserStats : WsashiModule
    {
        [Command("myowstats")]
        [Summary("Get your Overwatch statistics. NOTE: You must first register your Battle.net Username and ID with w!owaccount")]
        [Alias("myows", "myoverwatchstats")]
        [Remarks("w!myows <platform(psn/xbl/pc)> Ex: w!myows pc")]
        [Cooldown(10)]
        public async Task GetOwStats([Remainder] string message)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + config.OW + "/stats?playform=" + message);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);


            string cdd = dataObject.eu.stats.competitive.game_stats.damage_done.ToString();
            string cd = dataObject.eu.stats.competitive.game_stats.deaths.ToString();
            string celims = dataObject.eu.stats.competitive.game_stats.eliminations.ToString();
            string cgp = dataObject.eu.stats.competitive.game_stats.games_played.ToString();
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
            string end = dataObject.eu.stats.competitive.overall_stats.endorsement_level.ToString();
            string ends = dataObject.eu.stats.competitive.overall_stats.endorsement_shotcaller.ToString();
            string endt = dataObject.eu.stats.competitive.overall_stats.endorsement_teammate.ToString();
            string endn = dataObject.eu.stats.competitive.overall_stats.endorsement_sportsmanship.ToString();

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
                IconUrl = ti
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{config.OW}'s Overwatch Profile",
                IconUrl = ti
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddField("Competitive Game Stats", $"Games Played: **{cgp}**\nGames Won: **{cgw}**\nGames Lost: **{cgl}**\nEliminations: **{celims}**\nDeaths: **{cd}**\nWin Rate: **{cwr}**\nKills per Death: **{ckpd}**\nBest Kill Streak: **{ckbs}**\nDamage Done: **{cdd}**\nHealing Done: **{chd}**", true);
            embed.AddField("Quickplay Game Stats", $"Games Won: **{qgw}**\nEliminations: **{qelims}**\nDeaths: **{qd}**\nKills per Death: **{qkpd}**\nBest Kill Streak: **{qkbs}**\nDamage Done: **{qdd}**\nHealing Done: **{qhd}**", true);
            embed.AddField("Competitive Medals", $"Total Medals: **{cm}**\n:first_place: Gold Medals: **{cmg}**\n:second_place: Silver Medals: **{cms}**\n:third_place: Bronze Medals: **{cmb}**", true);
            embed.AddField("Quickplay Medals", $"Total Medals: **{qm}**\n:first_place: Gold Medals: **{qmg}**\n:second_place: Silver Medals: **{qms}**\n:third_place: Bronze Medals: **{qmb}**", true);
            embed.AddField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**\nEndorsement Level: **{end}**\nEndorsement Stats: Sportsmanship/Good Teammate/Shotcaller **{endn}/{endt}/{ends}**", true);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("myowstatsqp")]
        [Summary("Get your Overwatch Quickplay statistics. NOTE: You must first register your Battle.net Username and ID with w!owaccount")]
        [Alias("myowsqp", "myoverwatchstatsqp", "myowsquickplay")]
        [Remarks("w!myowsqp <platform (pc/xbl/psn)> Ex: w!myowsqp pc")]
        [Cooldown(10)]
        public async Task GetOwQpStats([Remainder] string message)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + config.OW + "/stats?playform=" + message);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string cwr = dataObject.eu.stats.quickplay.overall_stats.win_rate.ToString();
            string clvl = dataObject.eu.stats.quickplay.overall_stats.level.ToString();
            string cpres = dataObject.eu.stats.quickplay.overall_stats.prestige.ToString();
            string tier = dataObject.eu.stats.quickplay.overall_stats.tier.ToString();
            string avatar = dataObject.eu.stats.quickplay.overall_stats.avatar.ToString();
            string ti = dataObject.eu.stats.competitive.overall_stats.tier_image.ToString();
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
                IconUrl = ti
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{config.OW}'s Quickplay Overwatch Profile"
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddField("Quickplay Game Stats", $"Games Won: **{qgw}**\nEliminations: **{qelims}**\nDeaths: **{qd}**\nKills per Death: **{qkpd}**\nBest Kill Streak: **{qkbs}**\nDamage Done: **{qdd}**\nHealing Done: **{qhd}**", true);
            embed.AddField("Quickplay Medals", $"Total Medals: **{qm}**\n:first_place: Gold Medals: **{qmg}**\n:second_place: Silver Medals: **{qms}**\n:third_place: Bronze Medals: **{qmb}**", true);
            embed.AddField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**", true);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("myowstatscomp")]
        [Summary("Get your Overwatch Competitive statistics. NOTE: You must first register your Battle.net Username and ID with w!owaccount")]
        [Alias("myowsc", "myoverwatchstatscomp", "myowscompetitive")]
        [Remarks("w!myowsc <platform (pc/xbl/psn)> Ex: w!myowsc pc")]
        [Cooldown(10)]

        public async Task GetOwCompStats([Remainder] string message)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var json = await Global.SendWebRequest("https://owapi.net/api/v3/u/" + config.OW + "/stats?playform=" + message);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);


            string cdd = dataObject.eu.stats.competitive.game_stats.damage_done.ToString();
            string cd = dataObject.eu.stats.competitive.game_stats.deaths.ToString();
            string celims = dataObject.eu.stats.competitive.game_stats.eliminations.ToString();
            string cgp = dataObject.eu.stats.competitive.game_stats.games_played.ToString();
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

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OWAPI API",
                IconUrl = ti
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{config.OW}'s Competitive Overwatch Profile",
                IconUrl = ti
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(avatar);
            embed.WithColor(37, 152, 255);
            embed.AddField("Competitive Game Stats", $"Games Played: **{cgp}**\nGames Won: **{cgw}**\nGames Lost: **{cgl}**\nEliminations: **{celims}**\nDeaths: **{cd}**\nWin Rate: **{cwr}**\nKills per Death: **{ckpd}**\nBest Kill Streak: **{ckbs}**\nDamage Done: **{cdd}**\nHealing Done: **{chd}**", true);
            embed.AddField("Competitive Medals", $"Total Medals: **{cm}**\n:first_place: Gold Medals: **{cmg}**\n:second_place: Silver Medals: **{cms}**\n:third_place: Bronze Medals: **{cmb}**", true);
            embed.AddField("Overall", $"Level: **{clvl}**\nPrestige: **{cpres}**\nTier: **{tier}**\nCompetitive Rank: **{ccomprank}**", true);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
