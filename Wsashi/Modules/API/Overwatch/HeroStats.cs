using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Overwatch
{
    public class HeroStats : WsashiModule
    {
        [Command("owherostats")]
        [Summary("Get a Overwatch user's statistics for a specific hero.")]
        [Alias("owhs")]
        [Remarks("w!owherostats <hero> <Your Battle.net username and id> <platform (pc/xbl/psn)> <region (us/eu etc.)> Ex: w!owherostats dVa Phytal-1427 pc us")]
        [Cooldown(10)]
        public async Task GetOwHeroStats(string hero, string username, string platform, string region)
        {
            string originalhero = hero;
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            hero = hero.ToLower();
            hero = GetHero(hero);

            var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/heroes/{hero}");

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string endorsementIcon = dataObject.endorsementIcon.ToString();
            string playerIcon = dataObject.icon.ToString();
            string srIcon = dataObject.ratingIcon.ToString();
            //compstats
            //avg
            string CompAllDamageAvg = dataObject.competitiveStats.careerStats[hero].average.allDamageDoneAvgPer10Min.ToString();
            string CompBarrierDamageAvg = dataObject.competitiveStats.careerStats[hero].average.barrierDamageDoneAvgPer10Min.ToString();
            string CompCriticalsAvg = dataObject.competitiveStats.careerStats[hero].average.criticalHitsAvgPer10Min.ToString();
            string CompDeathAvg = dataObject.competitiveStats.careerStats[hero].average.deathsAvgPer10Min.ToString();
            string CompElimAvg = dataObject.competitiveStats.careerStats[hero].average.eliminationsAvgPer10Min.ToString();
            string CompElimPerLife = dataObject.competitiveStats.careerStats[hero].average.eliminationsPerLife.ToString();
            string CompFinalBlowAvg = dataObject.competitiveStats.careerStats[hero].average.finalBlowsAvgPer10Min.ToString();
            string CompHeroDamageAvg = dataObject.competitiveStats.careerStats[hero].average.heroDamageDoneAvgPer10Min.ToString();
            string CompMeleeAvg = dataObject.competitiveStats.careerStats[hero].average.meleeFinalBlowsAvgPer10Min.ToString();
            string CompObjKillsAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveKillsAvgPer10Min.ToString();
            string CompObjTimeAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveTimeAvgPer10Min.ToString();
            string CompSoloKillAvg = dataObject.competitiveStats.careerStats[hero].average.soloKillsAvgPer10Min.ToString();
            string CompOnFireAvg = dataObject.competitiveStats.careerStats[hero].average.timeSpentOnFireAvgPer10Min.ToString();
            //best
            string CompAllDamageInGame = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInGame.ToString();
            string CompAllDamageInLife = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInLife.ToString();
            string CompBarrierDamageInGame = dataObject.competitiveStats.careerStats[hero].best.barrierDamageDoneMostInGame.ToString();
            string CompCritMostInGame = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInGame.ToString();
            string CompCritMostInLife = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInLife.ToString();
            string CompElimMostInLife = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInLife.ToString();
            string CompElimMostInGame = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInGame.ToString();
            string CompFinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.finalBlowsMostInGame.ToString();
            string CompHeroDmgMostInGame = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInGame.ToString();
            string CompHeroDmgMostInLife = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInLife.ToString();
            string CompKillStreakBest = dataObject.competitiveStats.careerStats[hero].best.killsStreakBest.ToString();
            string CompMeleeFinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.meleeFinalBlowsMostInGame.ToString();
            string CompMultikillBest = dataObject.competitiveStats.careerStats[hero].best.multikillsBest.ToString();
            string CompObjKillMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveKillsMostInGame.ToString();
            string CompObjTimeMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveTimeMostInGame.ToString();
            string CompSoloKillsMostInGame = dataObject.competitiveStats.careerStats[hero].best.soloKillsMostInGame.ToString();
            string CompOnFireMostInGame = dataObject.competitiveStats.careerStats[hero].best.timeSpentOnFireMostInGame.ToString();
            string CompWeaponAccuracyBestInGame = dataObject.competitiveStats.careerStats[hero].best.weaponAccuracyBestInGame.ToString();
            //combat
            string CompBarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
            string CompCriticalHits = dataObject.competitiveStats.careerStats[hero].combat.criticalHits.ToString();
            string CompCriticalHitsAccuracy = dataObject.competitiveStats.careerStats[hero].combat.criticalHitsAccuracy.ToString();
            string CompDamageDone = dataObject.competitiveStats.careerStats[hero].combat.damageDone.ToString();
            string CompDeaths = dataObject.competitiveStats.careerStats[hero].combat.deaths.ToString();
            string CompElims = dataObject.competitiveStats.careerStats[hero].combat.eliminations.ToString();
            string CompEnvironmentalKills = dataObject.competitiveStats.careerStats[hero].combat.environmentalKills.ToString();
            string CompFinalBlows = dataObject.competitiveStats.careerStats[hero].combat.finalBlows.ToString();
            string CompHeroDmg = dataObject.competitiveStats.careerStats[hero].combat.heroDamageDone.ToString();
            string CompMeleeFinalBlows = dataObject.competitiveStats.careerStats[hero].combat.meleeFinalBlows.ToString();
            string CompMultikills = dataObject.competitiveStats.careerStats[hero].combat.multikills.ToString();
            string CompObjKills = dataObject.competitiveStats.careerStats[hero].combat.objectiveKills.ToString();
            string CompObjTime = dataObject.competitiveStats.careerStats[hero].combat.objectiveTime.ToString();
            string CompMeleeAccuracy = dataObject.competitiveStats.careerStats[hero].combat.quickMeleeAccuracy.ToString();
            string CompSoloKills = dataObject.competitiveStats.careerStats[hero].combat.soloKills.ToString();
            string CompOnFire = dataObject.competitiveStats.careerStats[hero].combat.timeSpentOnFire.ToString();
            string CompWeaponAccuracy = dataObject.competitiveStats.careerStats[hero].combat.weaponAccuracy.ToString();

            //quickplay stats
            string QpAllDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.allDamageDoneAvgPer10Min.ToString();
            string QpBarrierDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.barrierDamageDoneAvgPer10Min.ToString();
            string QpCriticalsAvg = dataObject.quickPlayStats.careerStats[hero].average.criticalHitsAvgPer10Min.ToString();
            string QpDeathAvg = dataObject.quickPlayStats.careerStats[hero].average.deathsAvgPer10Min.ToString();
            string QpElimAvg = dataObject.quickPlayStats.careerStats[hero].average.eliminationsAvgPer10Min.ToString();
            string QpElimPerLife = dataObject.quickPlayStats.careerStats[hero].average.eliminationsPerLife.ToString();
            string QpFinalBlowAvg = dataObject.quickPlayStats.careerStats[hero].average.finalBlowsAvgPer10Min.ToString();
            string QpHeroDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.heroDamageDoneAvgPer10Min.ToString();
            string QpMeleeAvg = dataObject.quickPlayStats.careerStats[hero].average.meleeFinalBlowsAvgPer10Min.ToString();
            string QpObjKillsAvg = dataObject.quickPlayStats.careerStats[hero].average.objectiveKillsAvgPer10Min.ToString();
            string QpObjTimeAvg = dataObject.quickPlayStats.careerStats[hero].average.objectiveTimeAvgPer10Min.ToString();
            string QpSoloKillAvg = dataObject.quickPlayStats.careerStats[hero].average.soloKillsAvgPer10Min.ToString();
            string QpOnFireAvg = dataObject.quickPlayStats.careerStats[hero].average.timeSpentOnFireAvgPer10Min.ToString();
            //best
            string QpAllDamageInGame = dataObject.quickPlayStats.careerStats[hero].best.allDamageDoneMostInGame.ToString();
            string QpAllDamageInLife = dataObject.quickPlayStats.careerStats[hero].best.allDamageDoneMostInLife.ToString();
            string QpBarrierDamageInGame = dataObject.quickPlayStats.careerStats[hero].best.barrierDamageDoneMostInGame.ToString();
            string QpCritMostInGame = dataObject.quickPlayStats.careerStats[hero].best.criticalHitsMostInGame.ToString();
            string QpCritMostInLife = dataObject.quickPlayStats.careerStats[hero].best.criticalHitsMostInLife.ToString();
            string QpElimMostInLife = dataObject.quickPlayStats.careerStats[hero].best.eliminationsMostInLife.ToString();
            string QpElimMostInGame = dataObject.quickPlayStats.careerStats[hero].best.eliminationsMostInGame.ToString();
            string QpFinalBlowMostInGame = dataObject.quickPlayStats.careerStats[hero].best.finalBlowsMostInGame.ToString();
            string QpHeroDmgMostInGame = dataObject.quickPlayStats.careerStats[hero].best.heroDamageDoneMostInGame.ToString();
            string QpHeroDmgMostInLife = dataObject.quickPlayStats.careerStats[hero].best.heroDamageDoneMostInLife.ToString();
            string QpKillStreakBest = dataObject.quickPlayStats.careerStats[hero].best.killsStreakBest.ToString();
            string QpMeleeFinalBlowMostInGame = dataObject.quickPlayStats.careerStats[hero].best.meleeFinalBlowsMostInGame.ToString();
            string QpMultikillBest = dataObject.quickPlayStats.careerStats[hero].best.multikillsBest.ToString();
            string QpObjKillMostInGame = dataObject.quickPlayStats.careerStats[hero].best.objectiveKillsMostInGame.ToString();
            string QpObjTimeMostInGame = dataObject.quickPlayStats.careerStats[hero].best.objectiveTimeMostInGame.ToString();
            string QpSoloKillsMostInGame = dataObject.quickPlayStats.careerStats[hero].best.soloKillsMostInGame.ToString();
            string QpOnFireMostInGame = dataObject.quickPlayStats.careerStats[hero].best.timeSpentOnFireMostInGame.ToString();
            string QpWeaponAccuracyBestInGame = dataObject.quickPlayStats.careerStats[hero].best.weaponAccuracyBestInGame.ToString();
            //combat
            string QpBarrierDmgDone = dataObject.quickPlayStats.careerStats[hero].combat.barrierDamageDone.ToString();
            string QpCriticalHits = dataObject.quickPlayStats.careerStats[hero].combat.criticalHits.ToString();
            string QpCriticalHitsAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.criticalHitsAccuracy.ToString();
            string QpDamageDone = dataObject.quickPlayStats.careerStats[hero].combat.damageDone.ToString();
            string QpDeaths = dataObject.quickPlayStats.careerStats[hero].combat.deaths.ToString();
            string QpElims = dataObject.quickPlayStats.careerStats[hero].combat.eliminations.ToString();
            string QpEnvironmentalKills = dataObject.quickPlayStats.careerStats[hero].combat.environmentalKills.ToString();
            string QpFinalBlows = dataObject.quickPlayStats.careerStats[hero].combat.finalBlows.ToString();
            string QpHeroDmg = dataObject.quickPlayStats.careerStats[hero].combat.heroDamageDone.ToString();
            string QpMeleeFinalBlows = dataObject.quickPlayStats.careerStats[hero].combat.meleeFinalBlows.ToString();
            string QpMultikills = dataObject.quickPlayStats.careerStats[hero].combat.multikills.ToString();
            string QpObjKills = dataObject.quickPlayStats.careerStats[hero].combat.objectiveKills.ToString();
            string QpObjTime = dataObject.quickPlayStats.careerStats[hero].combat.objectiveTime.ToString();
            string QpMeleeAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.quickMeleeAccuracy.ToString();
            string QpSoloKills = dataObject.quickPlayStats.careerStats[hero].combat.soloKills.ToString();
            string QpOnFire = dataObject.quickPlayStats.careerStats[hero].combat.timeSpentOnFire.ToString();
            string QpWeaponAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.weaponAccuracy.ToString();

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OW-API",
                IconUrl = srIcon
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{username}'s Hero Stats for {originalhero}",
                IconUrl = endorsementIcon
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(playerIcon);
            embed.WithColor(37, 152, 255);
            embed.AddField("Competitive Averages", $"All Damage Done per 10 Minutes: **{CompAllDamageAvg}**\nBarrier Damage Done per 10 minutes: **{CompBarrierDamageAvg}**\nHero Damage Done per 10 minutes: **{CompHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{CompCriticalsAvg}**\nDeaths per 10 Minutes: **{CompDeathAvg}**\nEliminations per 10 minutes: **{CompElimAvg}**\nEliminations per Life: **{CompElimPerLife}**\nFinal Blows per 10 minutes: **{CompFinalBlowAvg}**\nMelee Final Blows per 10 minutes: **{CompMeleeAvg}**\nObjective Time per 10 minutes: **{CompObjTimeAvg}**\nObjective Kills per 10 minutes: **{CompObjKillsAvg}**\nSolo Kills per 10 minutes: **{CompSoloKillAvg}**\nTime on Fire per 10 minutes: **{CompOnFireAvg}**", true);
            embed.AddField("Competitive Best", $"All Damage in Game: **{CompAllDamageInGame}**\nAll Damage in Life: **{CompAllDamageInLife}**\nBarrier Damage in Game: **{CompBarrierDamageInGame}**\nCriticals in Game: **{CompCritMostInGame}**\nCriticals in Life: **{CompCritMostInLife}**\nEliminations in Game: **{CompElimMostInGame}**\nEliminations in Life: **{CompElimMostInLife}**\nFinal Blows in Game: **{CompFinalBlowMostInGame}**\nHero Damage in Game: **{CompHeroDmgMostInGame}**\nHero Damage in Life: **{CompHeroDmgMostInLife}**\nKill Streak: **{CompKillStreakBest}**\nMelee Final Blows in Game: **{CompMeleeFinalBlowMostInGame}**\nMultikill: **{CompMultikillBest}**\nObjective Kills in Game: **{CompObjKillMostInGame}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nSolo Kills in Game: **{CompSoloKillsMostInGame}**\nOn Fire Time in Game: **{CompOnFireMostInGame}**\nWeapon Accuracy in Game: **{CompWeaponAccuracyBestInGame}**", true);
            embed.AddField("Competitive Totals", $"Barrier Damage Done: **{CompBarrierDmgDone}**\nCritical Hits: **{CompCriticalHits}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nCritical Hit Accuracy: **{CompCriticalHitsAccuracy}**\nDamage Done: **{CompDamageDone}**\nDeaths: **{CompDeaths}**\nEliminations: **{CompElims}**\nEnvironmental Kills: **{CompEnvironmentalKills}**\nFinal Blows: **{CompFinalBlows}**\nHero Damage: **{CompHeroDmg}**\nMelee Final Blows: **{CompMeleeFinalBlows}**\nMultikills: **{CompMultikills}**\nObjective Kills: **{CompObjKills}**\nObjective Time: **{CompObjTime}**\nMelee Accuracy: **{CompMeleeAccuracy}**\nSolo Kills: **{CompSoloKills}**\nOn Fire Time: **{CompOnFire}**\nWeapon Accuracy: **{CompWeaponAccuracy}**", true);
            embed.AddField("Quickplay Averages", $"All Damage Done per 10 Minutes: **{QpAllDamageAvg}**\nBarrier Damage Done per 10 minutes: **{QpBarrierDamageAvg}**\nHero Damage Done per 10 minutes: **{QpHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{QpCriticalsAvg}**\nDeaths per 10 Minutes: **{QpDeathAvg}**\nEliminations per 10 minutes: **{QpElimAvg}**\nEliminations per Life: **{QpElimPerLife}**\nFinal Blows per 10 minutes: **{QpFinalBlowAvg}**\nMelee Final Blows per 10 minutes: **{QpMeleeAvg}**\nObjective Time per 10 minutes: **{QpObjTimeAvg}**\nObjective Kills per 10 minutes: **{QpObjKillsAvg}**\nSolo Kills per 10 minutes: **{QpSoloKillAvg}**\nTime on Fire per 10 minutes: **{QpOnFireAvg}**", true);
            embed.AddField("Quickplay Best", $"All Damage in Game: **{QpAllDamageInGame}**\nAll Damage in Life: **{QpAllDamageInLife}**\nBarrier Damage in Game: **{QpBarrierDamageInGame}**\nCriticals in Game: **{QpCritMostInGame}**\nCriticals in Life: **{QpCritMostInLife}**\nEliminations in Game: **{QpElimMostInGame}**\nEliminations in Life: **{QpElimMostInLife}**\nFinal Blows in Game: **{QpFinalBlowMostInGame}**\nHero Damage in Game: **{QpHeroDmgMostInGame}**\nHero Damage in Life: **{QpHeroDmgMostInLife}**\nKill Streak: **{QpKillStreakBest}**\nMelee Final Blows in Game: **{QpMeleeFinalBlowMostInGame}**\nMultikill: **{QpMultikillBest}**\nObjective Kills in Game: **{QpObjKillMostInGame}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nSolo Kills in Game: **{QpSoloKillsMostInGame}**\nOn Fire Time in Game: **{QpOnFireMostInGame}**\nWeapon Accuracy in Game: **{QpWeaponAccuracyBestInGame}**", true);
            embed.AddField("Quickplay Totals", $"Barrier Damage Done: **{QpBarrierDmgDone}**\nCritical Hits: **{QpCriticalHits}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nCritical Hit Accuracy: **{QpCriticalHitsAccuracy}**\nDamage Done: **{QpDamageDone}**\nDeaths: **{QpDeaths}**\nEliminations: **{QpElims}**\nEnvironmental Kills: **{QpEnvironmentalKills}**\nFinal Blows: **{QpFinalBlows}**\nHero Damage: **{QpHeroDmg}**\nMelee Final Blows: **{QpMeleeFinalBlows}**\nMultikills: **{QpMultikills}**\nObjective Kills: **{QpObjKills}**\nObjective Time: **{QpObjTime}**\nMelee Accuracy: **{QpMeleeAccuracy}**\nSolo Kills: **{QpSoloKills}**\nOn Fire Time: **{QpOnFire}**\nWeapon Accuracy: **{QpWeaponAccuracy}**", true);

            //herospecific WIP
            if (hero == "ana")
            {
                string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
            }
            await Context.Channel.SendMessageAsync("", embed: embed.Build());

        }
        [Command("myowherostats")]
        [Summary("Get your statistics for a specific hero.")]
        [Alias("owhs")]
        [Remarks("w!owherostats <hero>Ex: w!owherostats dVa")]
        [Cooldown(10)]
        public async Task GetMyOwHeroStats(string hero)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{config.OverwatchPlatform}/{config.OverwatchRegion}/{config.OverwatchID}/heroes/{hero}");

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string endorsementIcon = dataObject.endorsementIcon.ToString();
            string playerIcon = dataObject.icon.ToString();
            string srIcon = dataObject.ratingIcon.ToString();
            //compstats
            //avg
            string CompAllDamageAvg = dataObject.competitiveStats.careerStats[hero].average.allDamageDoneAvgPer10Min.ToString();
            string CompBarrierDamageAvg = dataObject.competitiveStats.careerStats[hero].average.barrierDamageDoneAvgPer10Min.ToString();
            string CompCriticalsAvg = dataObject.competitiveStats.careerStats[hero].average.criticalHitsAvgPer10Min.ToString();
            string CompDeathAvg = dataObject.competitiveStats.careerStats[hero].average.deathsAvgPer10Min.ToString();
            string CompElimAvg = dataObject.competitiveStats.careerStats[hero].average.eliminationsAvgPer10Min.ToString();
            string CompElimPerLife = dataObject.competitiveStats.careerStats[hero].average.eliminationsPerLife.ToString();
            string CompFinalBlowAvg = dataObject.competitiveStats.careerStats[hero].average.finalBlowsAvgPer10Min.ToString();
            string CompHeroDamageAvg = dataObject.competitiveStats.careerStats[hero].average.heroDamageDoneAvgPer10Min.ToString();
            string CompMeleeAvg = dataObject.competitiveStats.careerStats[hero].average.meleeFinalBlowsAvgPer10Min.ToString();
            string CompObjKillsAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveKillsAvgPer10Min.ToString();
            string CompObjTimeAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveTimeAvgPer10Min.ToString();
            string CompSoloKillAvg = dataObject.competitiveStats.careerStats[hero].average.soloKillsAvgPer10Min.ToString();
            string CompOnFireAvg = dataObject.competitiveStats.careerStats[hero].average.timeSpentOnFireAvgPer10Min.ToString();
            //best
            string CompAllDamageInGame = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInGame.ToString();
            string CompAllDamageInLife = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInLife.ToString();
            string CompBarrierDamageInGame = dataObject.competitiveStats.careerStats[hero].best.barrierDamageDoneMostInGame.ToString();
            string CompCritMostInGame = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInGame.ToString();
            string CompCritMostInLife = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInLife.ToString();
            string CompElimMostInLife = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInLife.ToString();
            string CompElimMostInGame = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInGame.ToString();
            string CompFinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.finalBlowsMostInGame.ToString();
            string CompHeroDmgMostInGame = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInGame.ToString();
            string CompHeroDmgMostInLife = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInLife.ToString();
            string CompKillStreakBest = dataObject.competitiveStats.careerStats[hero].best.killsStreakBest.ToString();
            string CompMeleeFinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.meleeFinalBlowsMostInGame.ToString();
            string CompMultikillBest = dataObject.competitiveStats.careerStats[hero].best.multikillsBest.ToString();
            string CompObjKillMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveKillsMostInGame.ToString();
            string CompObjTimeMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveTimeMostInGame.ToString();
            string CompSoloKillsMostInGame = dataObject.competitiveStats.careerStats[hero].best.soloKillsMostInGame.ToString();
            string CompOnFireMostInGame = dataObject.competitiveStats.careerStats[hero].best.timeSpentOnFireMostInGame.ToString();
            string CompWeaponAccuracyBestInGame = dataObject.competitiveStats.careerStats[hero].best.weaponAccuracyBestInGame.ToString();
            //combat
            string CompBarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
            string CompCriticalHits = dataObject.competitiveStats.careerStats[hero].combat.criticalHits.ToString();
            string CompCriticalHitsAccuracy = dataObject.competitiveStats.careerStats[hero].combat.criticalHitsAccuracy.ToString();
            string CompDamageDone = dataObject.competitiveStats.careerStats[hero].combat.damageDone.ToString();
            string CompDeaths = dataObject.competitiveStats.careerStats[hero].combat.deaths.ToString();
            string CompElims = dataObject.competitiveStats.careerStats[hero].combat.eliminations.ToString();
            string CompEnvironmentalKills = dataObject.competitiveStats.careerStats[hero].combat.environmentalKills.ToString();
            string CompFinalBlows = dataObject.competitiveStats.careerStats[hero].combat.finalBlows.ToString();
            string CompHeroDmg = dataObject.competitiveStats.careerStats[hero].combat.heroDamageDone.ToString();
            string CompMeleeFinalBlows = dataObject.competitiveStats.careerStats[hero].combat.meleeFinalBlows.ToString();
            string CompMultikills = dataObject.competitiveStats.careerStats[hero].combat.multikills.ToString();
            string CompObjKills = dataObject.competitiveStats.careerStats[hero].combat.objectiveKills.ToString();
            string CompObjTime = dataObject.competitiveStats.careerStats[hero].combat.objectiveTime.ToString();
            string CompMeleeAccuracy = dataObject.competitiveStats.careerStats[hero].combat.quickMeleeAccuracy.ToString();
            string CompSoloKills = dataObject.competitiveStats.careerStats[hero].combat.soloKills.ToString();
            string CompOnFire = dataObject.competitiveStats.careerStats[hero].combat.timeSpentOnFire.ToString();
            string CompWeaponAccuracy = dataObject.competitiveStats.careerStats[hero].combat.weaponAccuracy.ToString();

            //quickplay stats
            string QpAllDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.allDamageDoneAvgPer10Min.ToString();
            string QpBarrierDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.barrierDamageDoneAvgPer10Min.ToString();
            string QpCriticalsAvg = dataObject.quickPlayStats.careerStats[hero].average.criticalHitsAvgPer10Min.ToString();
            string QpDeathAvg = dataObject.quickPlayStats.careerStats[hero].average.deathsAvgPer10Min.ToString();
            string QpElimAvg = dataObject.quickPlayStats.careerStats[hero].average.eliminationsAvgPer10Min.ToString();
            string QpElimPerLife = dataObject.quickPlayStats.careerStats[hero].average.eliminationsPerLife.ToString();
            string QpFinalBlowAvg = dataObject.quickPlayStats.careerStats[hero].average.finalBlowsAvgPer10Min.ToString();
            string QpHeroDamageAvg = dataObject.quickPlayStats.careerStats[hero].average.heroDamageDoneAvgPer10Min.ToString();
            string QpMeleeAvg = dataObject.quickPlayStats.careerStats[hero].average.meleeFinalBlowsAvgPer10Min.ToString();
            string QpObjKillsAvg = dataObject.quickPlayStats.careerStats[hero].average.objectiveKillsAvgPer10Min.ToString();
            string QpObjTimeAvg = dataObject.quickPlayStats.careerStats[hero].average.objectiveTimeAvgPer10Min.ToString();
            string QpSoloKillAvg = dataObject.quickPlayStats.careerStats[hero].average.soloKillsAvgPer10Min.ToString();
            string QpOnFireAvg = dataObject.quickPlayStats.careerStats[hero].average.timeSpentOnFireAvgPer10Min.ToString();
            //best
            string QpAllDamageInGame = dataObject.quickPlayStats.careerStats[hero].best.allDamageDoneMostInGame.ToString();
            string QpAllDamageInLife = dataObject.quickPlayStats.careerStats[hero].best.allDamageDoneMostInLife.ToString();
            string QpBarrierDamageInGame = dataObject.quickPlayStats.careerStats[hero].best.barrierDamageDoneMostInGame.ToString();
            string QpCritMostInGame = dataObject.quickPlayStats.careerStats[hero].best.criticalHitsMostInGame.ToString();
            string QpCritMostInLife = dataObject.quickPlayStats.careerStats[hero].best.criticalHitsMostInLife.ToString();
            string QpElimMostInLife = dataObject.quickPlayStats.careerStats[hero].best.eliminationsMostInLife.ToString();
            string QpElimMostInGame = dataObject.quickPlayStats.careerStats[hero].best.eliminationsMostInGame.ToString();
            string QpFinalBlowMostInGame = dataObject.quickPlayStats.careerStats[hero].best.finalBlowsMostInGame.ToString();
            string QpHeroDmgMostInGame = dataObject.quickPlayStats.careerStats[hero].best.heroDamageDoneMostInGame.ToString();
            string QpHeroDmgMostInLife = dataObject.quickPlayStats.careerStats[hero].best.heroDamageDoneMostInLife.ToString();
            string QpKillStreakBest = dataObject.quickPlayStats.careerStats[hero].best.killsStreakBest.ToString();
            string QpMeleeFinalBlowMostInGame = dataObject.quickPlayStats.careerStats[hero].best.meleeFinalBlowsMostInGame.ToString();
            string QpMultikillBest = dataObject.quickPlayStats.careerStats[hero].best.multikillsBest.ToString();
            string QpObjKillMostInGame = dataObject.quickPlayStats.careerStats[hero].best.objectiveKillsMostInGame.ToString();
            string QpObjTimeMostInGame = dataObject.quickPlayStats.careerStats[hero].best.objectiveTimeMostInGame.ToString();
            string QpSoloKillsMostInGame = dataObject.quickPlayStats.careerStats[hero].best.soloKillsMostInGame.ToString();
            string QpOnFireMostInGame = dataObject.quickPlayStats.careerStats[hero].best.timeSpentOnFireMostInGame.ToString();
            string QpWeaponAccuracyBestInGame = dataObject.quickPlayStats.careerStats[hero].best.weaponAccuracyBestInGame.ToString();
            //combat
            string QpBarrierDmgDone = dataObject.quickPlayStats.careerStats[hero].combat.barrierDamageDone.ToString();
            string QpCriticalHits = dataObject.quickPlayStats.careerStats[hero].combat.criticalHits.ToString();
            string QpCriticalHitsAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.criticalHitsAccuracy.ToString();
            string QpDamageDone = dataObject.quickPlayStats.careerStats[hero].combat.damageDone.ToString();
            string QpDeaths = dataObject.quickPlayStats.careerStats[hero].combat.deaths.ToString();
            string QpElims = dataObject.quickPlayStats.careerStats[hero].combat.eliminations.ToString();
            string QpEnvironmentalKills = dataObject.quickPlayStats.careerStats[hero].combat.environmentalKills.ToString();
            string QpFinalBlows = dataObject.quickPlayStats.careerStats[hero].combat.finalBlows.ToString();
            string QpHeroDmg = dataObject.quickPlayStats.careerStats[hero].combat.heroDamageDone.ToString();
            string QpMeleeFinalBlows = dataObject.quickPlayStats.careerStats[hero].combat.meleeFinalBlows.ToString();
            string QpMultikills = dataObject.quickPlayStats.careerStats[hero].combat.multikills.ToString();
            string QpObjKills = dataObject.quickPlayStats.careerStats[hero].combat.objectiveKills.ToString();
            string QpObjTime = dataObject.quickPlayStats.careerStats[hero].combat.objectiveTime.ToString();
            string QpMeleeAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.quickMeleeAccuracy.ToString();
            string QpSoloKills = dataObject.quickPlayStats.careerStats[hero].combat.soloKills.ToString();
            string QpOnFire = dataObject.quickPlayStats.careerStats[hero].combat.timeSpentOnFire.ToString();
            string QpWeaponAccuracy = dataObject.quickPlayStats.careerStats[hero].combat.weaponAccuracy.ToString();

            var bottom = new EmbedFooterBuilder()
            {
                Text = "Powered by the OW-API",
                IconUrl = srIcon
            };

            var top = new EmbedAuthorBuilder()
            {
                Name = $"{config.OverwatchID}'s Hero Stats for {hero}",
                IconUrl = endorsementIcon
            };

            var embed = new EmbedBuilder()
            {
                Author = top,
                Footer = bottom
            };
            embed.WithThumbnailUrl(playerIcon);
            embed.WithColor(37, 152, 255);
            embed.AddField("Competitive Averages", $"All Damage Done per 10 Minutes: **{CompAllDamageAvg}**\nBarrier Damage Done per 10 minutes: **{CompBarrierDamageAvg}**\nHero Damage Done per 10 minutes: **{CompHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{CompCriticalsAvg}**\nDeaths per 10 Minutes: **{CompDeathAvg}**\nEliminations per 10 minutes: **{CompElimAvg}**\nEliminations per Life: **{CompElimPerLife}**\nFinal Blows per 10 minutes: **{CompFinalBlowAvg}**\nMelee Final Blows per 10 minutes: **{CompMeleeAvg}**\nObjective Time per 10 minutes: **{CompObjTimeAvg}**\nObjective Kills per 10 minutes: **{CompObjKillsAvg}**\nSolo Kills per 10 minutes: **{CompSoloKillAvg}**\nTime on Fire per 10 minutes: **{CompOnFireAvg}**", true);
            embed.AddField("Competitive Best", $"All Damage in Game: **{CompAllDamageInGame}**\nAll Damage in Life: **{CompAllDamageInLife}**\nBarrier Damage in Game: **{CompBarrierDamageInGame}**\nCriticals in Game: **{CompCritMostInGame}**\nCriticals in Life: **{CompCritMostInLife}**\nEliminations in Game: **{CompElimMostInGame}**\nEliminations in Life: **{CompElimMostInLife}**\nFinal Blows in Game: **{CompFinalBlowMostInGame}**\nHero Damage in Game: **{CompHeroDmgMostInGame}**\nHero Damage in Life: **{CompHeroDmgMostInLife}**\nKill Streak: **{CompKillStreakBest}**\nMelee Final Blows in Game: **{CompMeleeFinalBlowMostInGame}**\nMultikill: **{CompMultikillBest}**\nObjective Kills in Game: **{CompObjKillMostInGame}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nSolo Kills in Game: **{CompSoloKillsMostInGame}**\nOn Fire Time in Game: **{CompOnFireMostInGame}**\nWeapon Accuracy in Game: **{CompWeaponAccuracyBestInGame}**", true);
            embed.AddField("Competitive Totals", $"Barrier Damage Done: **{CompBarrierDmgDone}**\nCritical Hits: **{CompCriticalHits}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nCritical Hit Accuracy: **{CompCriticalHitsAccuracy}**\nDamage Done: **{CompDamageDone}**\nDeaths: **{CompDeaths}**\nEliminations: **{CompElims}**\nEnvironmental Kills: **{CompEnvironmentalKills}**\nFinal Blows: **{CompFinalBlows}**\nHero Damage: **{CompHeroDmg}**\nMelee Final Blows: **{CompMeleeFinalBlows}**\nMultikills: **{CompMultikills}**\nObjective Kills: **{CompObjKills}**\nObjective Time: **{CompObjTime}**\nMelee Accuracy: **{CompMeleeAccuracy}**\nSolo Kills: **{CompSoloKills}**\nOn Fire Time: **{CompOnFire}**\nWeapon Accuracy: **{CompWeaponAccuracy}**", true);
            embed.AddField("Quickplay Averages", $"All Damage Done per 10 Minutes: **{QpAllDamageAvg}**\nBarrier Damage Done per 10 minutes: **{QpBarrierDamageAvg}**\nHero Damage Done per 10 minutes: **{QpHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{QpCriticalsAvg}**\nDeaths per 10 Minutes: **{QpDeathAvg}**\nEliminations per 10 minutes: **{QpElimAvg}**\nEliminations per Life: **{QpElimPerLife}**\nFinal Blows per 10 minutes: **{QpFinalBlowAvg}**\nMelee Final Blows per 10 minutes: **{QpMeleeAvg}**\nObjective Time per 10 minutes: **{QpObjTimeAvg}**\nObjective Kills per 10 minutes: **{QpObjKillsAvg}**\nSolo Kills per 10 minutes: **{QpSoloKillAvg}**\nTime on Fire per 10 minutes: **{QpOnFireAvg}**", true);
            embed.AddField("Quickplay Best", $"All Damage in Game: **{QpAllDamageInGame}**\nAll Damage in Life: **{QpAllDamageInLife}**\nBarrier Damage in Game: **{QpBarrierDamageInGame}**\nCriticals in Game: **{QpCritMostInGame}**\nCriticals in Life: **{QpCritMostInLife}**\nEliminations in Game: **{QpElimMostInGame}**\nEliminations in Life: **{QpElimMostInLife}**\nFinal Blows in Game: **{QpFinalBlowMostInGame}**\nHero Damage in Game: **{QpHeroDmgMostInGame}**\nHero Damage in Life: **{QpHeroDmgMostInLife}**\nKill Streak: **{QpKillStreakBest}**\nMelee Final Blows in Game: **{QpMeleeFinalBlowMostInGame}**\nMultikill: **{QpMultikillBest}**\nObjective Kills in Game: **{QpObjKillMostInGame}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nSolo Kills in Game: **{QpSoloKillsMostInGame}**\nOn Fire Time in Game: **{QpOnFireMostInGame}**\nWeapon Accuracy in Game: **{QpWeaponAccuracyBestInGame}**", true);
            embed.AddField("Quickplay Totals", $"Barrier Damage Done: **{QpBarrierDmgDone}**\nCritical Hits: **{QpCriticalHits}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nCritical Hit Accuracy: **{QpCriticalHitsAccuracy}**\nDamage Done: **{QpDamageDone}**\nDeaths: **{QpDeaths}**\nEliminations: **{QpElims}**\nEnvironmental Kills: **{QpEnvironmentalKills}**\nFinal Blows: **{QpFinalBlows}**\nHero Damage: **{QpHeroDmg}**\nMelee Final Blows: **{QpMeleeFinalBlows}**\nMultikills: **{QpMultikills}**\nObjective Kills: **{QpObjKills}**\nObjective Time: **{QpObjTime}**\nMelee Accuracy: **{QpMeleeAccuracy}**\nSolo Kills: **{QpSoloKills}**\nOn Fire Time: **{QpOnFire}**\nWeapon Accuracy: **{QpWeaponAccuracy}**", true);

            //herospecific
            if (hero == "ana")
            {
                string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
            }
            await Context.Channel.SendMessageAsync("", embed: embed.Build());

        }
        private string GetHero(string value)
        {
            if (value == "dva" || value == "d.va") return $"dVa";
            else if (value == "baguette") return $"brigitte";
            else if (value == "lucio") return $"lúcio";
            else if (value == "torbjorn" || value == "torb") return $"torbjörn";
            else if (value == "soldier") return $"soldier76";
            else if (value == "mcree") return $"mcree";
            else if (value == "widow") return $"widowmaker";
            else if (value == "sym") return $"symmetra";
            else if (value == "rein") return $"reinhardt";
            else if (value == "zen") return $"zenyatta";
            else return value;
        }
    }
}
