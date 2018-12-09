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
            try
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
                embed.AddField("Competitive Averages", $"All Damage Done per 10 Minutes: **{CompAllDamageAvg}**\nBarrier Damage Done per 10 Minutes: **{CompBarrierDamageAvg}**\nHero Damage Done per 10 Minutes: **{CompHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{CompCriticalsAvg}**\nDeaths per 10 Minutes: **{CompDeathAvg}**\nEliminations per 10 Minutes: **{CompElimAvg}**\nEliminations per Life: **{CompElimPerLife}**\nFinal Blows per 10 Minutes: **{CompFinalBlowAvg}**\nMelee Final Blows per 10 Minutes: **{CompMeleeAvg}**\nObjective Time per 10 Minutes: **{CompObjTimeAvg}**\nObjective Kills per 10 Minutes: **{CompObjKillsAvg}**\nSolo Kills per 10 Minutes: **{CompSoloKillAvg}**\nTime on Fire per 10 Minutes: **{CompOnFireAvg}**", true);
                embed.AddField("Competitive Best", $"All Damage in Game: **{CompAllDamageInGame}**\nAll Damage in Life: **{CompAllDamageInLife}**\nBarrier Damage in Game: **{CompBarrierDamageInGame}**\nCriticals in Game: **{CompCritMostInGame}**\nCriticals in Life: **{CompCritMostInLife}**\nEliminations in Game: **{CompElimMostInGame}**\nEliminations in Life: **{CompElimMostInLife}**\nFinal Blows in Game: **{CompFinalBlowMostInGame}**\nHero Damage in Game: **{CompHeroDmgMostInGame}**\nHero Damage in Life: **{CompHeroDmgMostInLife}**\nKill Streak: **{CompKillStreakBest}**\nMelee Final Blows in Game: **{CompMeleeFinalBlowMostInGame}**\nMultikill: **{CompMultikillBest}**\nObjective Kills in Game: **{CompObjKillMostInGame}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nSolo Kills in Game: **{CompSoloKillsMostInGame}**\nOn Fire Time in Game: **{CompOnFireMostInGame}**\nWeapon Accuracy in Game: **{CompWeaponAccuracyBestInGame}**", true);
                embed.AddField("Competitive Totals", $"Barrier Damage Done: **{CompBarrierDmgDone}**\nCritical Hits: **{CompCriticalHits}**\nObjective Time in Game: **{CompObjTimeMostInGame}**\nCritical Hit Accuracy: **{CompCriticalHitsAccuracy}**\nDamage Done: **{CompDamageDone}**\nDeaths: **{CompDeaths}**\nEliminations: **{CompElims}**\nEnvironmental Kills: **{CompEnvironmentalKills}**\nFinal Blows: **{CompFinalBlows}**\nHero Damage: **{CompHeroDmg}**\nMelee Final Blows: **{CompMeleeFinalBlows}**\nMultikills: **{CompMultikills}**\nObjective Kills: **{CompObjKills}**\nObjective Time: **{CompObjTime}**\nMelee Accuracy: **{CompMeleeAccuracy}**\nSolo Kills: **{CompSoloKills}**\nOn Fire Time: **{CompOnFire}**\nWeapon Accuracy: **{CompWeaponAccuracy}**", true);
                embed.AddField("Quickplay Averages", $"All Damage Done per 10 Minutes: **{QpAllDamageAvg}**\nBarrier Damage Done per 10 Minutes: **{QpBarrierDamageAvg}**\nHero Damage Done per 10 Minutes: **{QpHeroDamageAvg}**\nCritical Hits per 10 Minutes: **{QpCriticalsAvg}**\nDeaths per 10 Minutes: **{QpDeathAvg}**\nEliminations per 10 Minutes: **{QpElimAvg}**\nEliminations per Life: **{QpElimPerLife}**\nFinal Blows per 10 Minutes: **{QpFinalBlowAvg}**\nMelee Final Blows per 10 Minutes: **{QpMeleeAvg}**\nObjective Time per 10 Minutes: **{QpObjTimeAvg}**\nObjective Kills per 10 Minutes: **{QpObjKillsAvg}**\nSolo Kills per 10 Minutes: **{QpSoloKillAvg}**\nTime on Fire per 10 Minutes: **{QpOnFireAvg}**", true);
                embed.AddField("Quickplay Best", $"All Damage in Game: **{QpAllDamageInGame}**\nAll Damage in Life: **{QpAllDamageInLife}**\nBarrier Damage in Game: **{QpBarrierDamageInGame}**\nCriticals in Game: **{QpCritMostInGame}**\nCriticals in Life: **{QpCritMostInLife}**\nEliminations in Game: **{QpElimMostInGame}**\nEliminations in Life: **{QpElimMostInLife}**\nFinal Blows in Game: **{QpFinalBlowMostInGame}**\nHero Damage in Game: **{QpHeroDmgMostInGame}**\nHero Damage in Life: **{QpHeroDmgMostInLife}**\nKill Streak: **{QpKillStreakBest}**\nMelee Final Blows in Game: **{QpMeleeFinalBlowMostInGame}**\nMultikill: **{QpMultikillBest}**\nObjective Kills in Game: **{QpObjKillMostInGame}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nSolo Kills in Game: **{QpSoloKillsMostInGame}**\nOn Fire Time in Game: **{QpOnFireMostInGame}**\nWeapon Accuracy in Game: **{QpWeaponAccuracyBestInGame}**", true);
                embed.AddField("Quickplay Totals", $"Barrier Damage Done: **{QpBarrierDmgDone}**\nCritical Hits: **{QpCriticalHits}**\nObjective Time in Game: **{QpObjTimeMostInGame}**\nCritical Hit Accuracy: **{QpCriticalHitsAccuracy}**\nDamage Done: **{QpDamageDone}**\nDeaths: **{QpDeaths}**\nEliminations: **{QpElims}**\nEnvironmental Kills: **{QpEnvironmentalKills}**\nFinal Blows: **{QpFinalBlows}**\nHero Damage: **{QpHeroDmg}**\nMelee Final Blows: **{QpMeleeFinalBlows}**\nMultikills: **{QpMultikills}**\nObjective Kills: **{QpObjKills}**\nObjective Time: **{QpObjTime}**\nMelee Accuracy: **{QpMeleeAccuracy}**\nSolo Kills: **{QpSoloKills}**\nOn Fire Time: **{QpOnFire}**\nWeapon Accuracy: **{QpWeaponAccuracy}**", true);

                //herospecific WIP
                if (hero == "ana")
                {
                    string QpBioticKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.bioticGrenadeKills.ToString();
                    string QpEnemiesSlept = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSlept.ToString();
                    string QpEnemiesSleptPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSleptAvgPer10Min.ToString();
                    string QpEnemiesSleptMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSleptMostInGame.ToString();
                    string QpNanoAssists = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssists.ToString();
                    string QpNanoAssistsPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssistsAvgPer10Min.ToString();
                    string QpMostNanoAssistsIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssistsMostInGame.ToString();
                    string QpNanosApplied = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsApplied.ToString();
                    string QpNanosAppliedPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsAppliedAvgPer10Min.ToString();
                    string QpNanoAppliedMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsAppliedMostInGame.ToString();
                    string QpScopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracy.ToString();
                    string QpScopedAccuracyBestIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracyBestInGame.ToString();
                    string QpSecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();
                    string QpSelfHealing = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealing.ToString();
                    string QpSelfHealingPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingAvgPer10Min.ToString();
                    string QpSelfHealingMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingMostInGame.ToString();
                    string QpUnscopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.unscopedAccuracy.ToString();
                    string QpUnscopedAccuracyBestIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.unscopedAccuracyBestInGame.ToString();

                    string CompBioticKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.bioticGrenadeKills.ToString();
                    string CompEnemiesSlept = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSlept.ToString();
                    string CompEnemiesSleptPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSleptAvgPer10Min.ToString();
                    string CompEnemiesSleptMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.enemiesSleptMostInGame.ToString();
                    string CompNanoAssists = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssists.ToString();
                    string CompNanoAssistsPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssistsAvgPer10Min.ToString();
                    string CompMostNanoAssistsIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostAssistsMostInGame.ToString();
                    string CompNanosApplied = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsApplied.ToString();
                    string CompNanosAppliedPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsAppliedAvgPer10Min.ToString();
                    string CompNanoAppliedMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.nanoBoostsAppliedMostInGame.ToString();
                    string CompScopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracy.ToString();
                    string CompScopedAccuracyBestIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracyBestInGame.ToString();
                    string CompSecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();
                    string CompSelfHealing = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealing.ToString();
                    string CompSelfHealingPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingAvgPer10Min.ToString();
                    string CompSelfHealingMostIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingMostInGame.ToString();
                    string CompUnscopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.unscopedAccuracy.ToString();
                    string CompUnscopedAccuracyBestIG = dataObject.competitiveStats.careerStats[hero].heroSpecific.unscopedAccuracyBestInGame.ToString();
                    embed.AddField("Quickplay Hero Specific", $"Biotic Grenade Kills: **{QpBioticKills}**\nEnemies Slept: **{QpEnemiesSlept}**\nAverage Enemies Slept per 10 Minutes: **{QpEnemiesSleptPer10Min}**\nMost Enemies Slept In Game: **{QpEnemiesSleptPer10Min}**\nNano Boost Assists: **{QpNanoAssists}**\nNano Boost Assists Per 10 Minutes: **{QpNanosAppliedPer10Min}**\nMost Nano Boost Assists In Game: **{QpMostNanoAssistsIG}**\nNano Boosts Applied: **{QpNanosApplied}**\nNano Boosts Applied Per 10 Minutes: **{QpNanosAppliedPer10Min}**\nNano Boosts Applied Most In Game: **{QpNanoAppliedMostIG}**\nScoped Accuracy: **{QpScopedAccuracy}**\nBest Scoped Accuracy In Game: **{QpScopedAccuracyBestIG}**\nSecondary Fire Accuracy: **{QpSecondaryFireAccuracy}**\nSelf Healing: **{QpSelfHealing}**\nSelf Healing Per 10 Minutes: **{QpSelfHealingPer10Min}**\nMost Self Healing In Game: **{QpSelfHealingMostIG}**\nUnscoped Accuracy: **{QpUnscopedAccuracy}**\nBest Unscoped Accuracy In Game: **{QpScopedAccuracyBestIG}**");
                    embed.AddField("Competitive Hero Specific", $"Biotic Grenade Kills: **{CompBioticKills}**\nEnemies Slept: **{CompEnemiesSlept}**\nAverage Enemies Slept per 10 Minutes: **{CompEnemiesSleptPer10Min}**\nMost Enemies Slept In Game: **{CompEnemiesSleptPer10Min}**\nNano Boost Assists: **{CompNanoAssists}**\nNano Boost Assists Per 10 Minutes: **{CompNanosAppliedPer10Min}**\nMost Nano Boost Assists In Game: **{CompMostNanoAssistsIG}**\nNano Boosts Applied: **{CompNanosApplied}**\nNano Boosts Applied Per 10 Minutes: **{CompNanosAppliedPer10Min}**\nNano Boosts Applied Most In Game: **{CompNanoAppliedMostIG}**\nScoped Accuracy: **{CompScopedAccuracy}**\nBest Scoped Accuracy In Game: **{CompScopedAccuracyBestIG}**\nSecondary Fire Accuracy: **{CompSecondaryFireAccuracy}**\nSelf Healing: **{CompSelfHealing}**\nSelf Healing Per 10 Minutes: **{CompSelfHealingPer10Min}**\nMost Self Healing In Game: **{CompSelfHealingMostIG}**\nUnscoped Accuracy: **{CompUnscopedAccuracy}**\nBest Unscoped Accuracy In Game: **{CompScopedAccuracyBestIG}**");
                }
                if (hero == "ashe")
                {
                    string CompbobKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKills.ToString();
                    string CompbobKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKillsAvgPer10Min.ToString();
                    string CompbobKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKillsMostInGame.ToString();
                    string CompcoachGunKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKills.ToString();
                    string CompcoachGunKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKillsAvgPer10Min.ToString();
                    string CompcoachGunKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKillsMostInGame.ToString();
                    string CompdynamiteKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKills.ToString();
                    string CompdynamiteKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKillsAvgPer10Min.ToString();
                    string CompdynamiteKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKillsMostInGame.ToString();
                    string CompscopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracy.ToString();
                    string CompscopedAccuracyBestInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracyBestInGame.ToString();
                    string CompscopedCriticalHits = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHits.ToString();
                    string CompscopedCriticalHitsAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsAccuracy.ToString();
                    string CompscopedCriticalHitsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsAvgPer10Min.ToString();
                    string CompscopedCriticalHitsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsMostInGame.ToString();
                    string CompsecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();

                    string QpbobKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKills.ToString();
                    string QpbobKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKillsAvgPer10Min.ToString();
                    string QpbobKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.bobKillsMostInGame.ToString();
                    string QpcoachGunKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKills.ToString();
                    string QpcoachGunKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKillsAvgPer10Min.ToString();
                    string QpcoachGunKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.coachGunKillsMostInGame.ToString();
                    string QpdynamiteKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKills.ToString();
                    string QpdynamiteKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKillsAvgPer10Min.ToString();
                    string QpdynamiteKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.dynamiteKillsMostInGame.ToString();
                    string QpscopedAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracy.ToString();
                    string QpscopedAccuracyBestInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedAccuracyBestInGame.ToString();
                    string QpscopedCriticalHits = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHits.ToString();
                    string QpscopedCriticalHitsAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsAccuracy.ToString();
                    string QpscopedCriticalHitsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsAvgPer10Min.ToString();
                    string QpscopedCriticalHitsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.scopedCriticalHitsMostInGame.ToString();
                    string QpsecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();

                    embed.AddField("Quickplay Hero Specific", $"BOB Kills: **{QpbobKills}**\nAverage BOB Kills Per 10 Minutes: **{QpbobKillsAvgPer10Min}**\nMost BOB Kills In Game: **{QpbobKillsMostInGame}**\nCoach Gun Kills: **{QpcoachGunKills}**\nAverage Coach Gun Kills Per 10 Minutes: **{QpcoachGunKillsAvgPer10Min}**\nMost Coach Gun Kills In Game: **{QpcoachGunKillsMostInGame}**\nDynamite Kills: **{QpdynamiteKills}**\nAverage Dynamite Kills Per 10 Minutes: **{QpdynamiteKillsAvgPer10Min}**\nMost Dynamite Kills In Game Kills: **{QpdynamiteKillsMostInGame}**\nScoped Accuracy: **{QpscopedAccuracy}**\nBest Scoped Accuracy In Game: **{QpscopedAccuracyBestInGame}**\nScoped Creitical Hits: **{QpscopedCriticalHits}**\nScoped Critical Hits Accuracy: **{QpscopedCriticalHitsAccuracy}**\nAverage Scoped Critical Hits Per 10 Minutes: **{QpscopedCriticalHitsAvgPer10Min}**\nMost Scoped Critical Hits In Game: **{QpscopedCriticalHitsMostInGame}**\nSecondary Fire Accuracy: **{QpsecondaryFireAccuracy}**", true);
                    embed.AddField("Competitive Hero Specific", $"BOB Kills: **{CompbobKills}**\nAverage BOB Kills Per 10 Minutes: **{CompbobKillsAvgPer10Min}**\nMost BOB Kills In Game: **{CompbobKillsMostInGame}**\nCoach Gun Kills: **{CompcoachGunKills}**\nAverage Coach Gun Kills Per 10 Minutes: **{CompcoachGunKillsAvgPer10Min}**\nMost Coach Gun Kills In Game: **{CompcoachGunKillsMostInGame}**\nDynamite Kills: **{CompdynamiteKills}**\nAverage Dynamite Kills Per 10 Minutes: **{CompdynamiteKillsAvgPer10Min}**\nMost Dynamite Kills In Game Kills: **{CompdynamiteKillsMostInGame}**\nScoped Accuracy: **{CompscopedAccuracy}**\nBest Scoped Accuracy In Game: **{CompscopedAccuracyBestInGame}**\nScoped Creitical Hits: **{CompscopedCriticalHits}**\nScoped Critical Hits Accuracy: **{CompscopedCriticalHitsAccuracy}**\nAverage Scoped Critical Hits Per 10 Minutes: **{CompscopedCriticalHitsAvgPer10Min}**\nMost Scoped Critical Hits In Game: **{CompscopedCriticalHitsMostInGame}**\nSecondary Fire Accuracy: **{CompsecondaryFireAccuracy}**", true);
                }
                if (hero == "bastion")
                {
                    string QpreconKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKills.ToString();
                    string QpreconKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKillsAvgPer10Min.ToString();
                    string QpreconKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKillsMostInGame.ToString();
                    string QpsecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();
                    string QpselfHealing = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealing.ToString();
                    string QpselfHealingAvgPer10Min = dataObject.competitiveStats.careerStats[hero].selfHealingAvgPer10Min.selfHealingAvgPer10Min.ToString();
                    string QpselfHealingMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingMostInGame.ToString();
                    string QpsentryKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKills.ToString();
                    string QpsentryKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKillsAvgPer10Min.ToString();
                    string QpsentryKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKillsMostInGame.ToString();
                    string QptankKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKills.ToString();
                    string QptankKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKillsAvgPer10Min.ToString();
                    string QptankKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKillsMostInGame.ToString();

                    string CompreconKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKills.ToString();
                    string CompreconKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKillsAvgPer10Min.ToString();
                    string CompreconKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.reconKillsMostInGame.ToString();
                    string CompsecondaryFireAccuracy = dataObject.competitiveStats.careerStats[hero].heroSpecific.secondaryFireAccuracy.ToString();
                    string CompselfHealing = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealing.ToString();
                    string CompselfHealingAvgPer10Min = dataObject.competitiveStats.careerStats[hero].selfHealingAvgPer10Min.selfHealingAvgPer10Min.ToString();
                    string CompselfHealingMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.selfHealingMostInGame.ToString();
                    string CompsentryKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKills.ToString();
                    string CompsentryKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKillsAvgPer10Min.ToString();
                    string CompsentryKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.sentryKillsMostInGame.ToString();
                    string ComptankKills = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKills.ToString();
                    string ComptankKillsAvgPer10Min = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKillsAvgPer10Min.ToString();
                    string ComptankKillsMostInGame = dataObject.competitiveStats.careerStats[hero].heroSpecific.tankKillsMostInGame.ToString();


                    embed.AddField("Quickplay Hero Specific", $"Recon Kills: **{QpreconKills}**\nAverage Recon Kills Per 10 Minutes: **{QpreconKillsAvgPer10Min}**\nMost Recon Kills In Game: **{QpreconKillsMostInGame}**\nSecondary Fire Accuracy: **{QpsecondaryFireAccuracy}**\nSelf Healing: **{QpselfHealing}**\nAverage Self Healing Per 10 Minutes: **{QpselfHealingAvgPer10Min}**\nMost Self Healing In Game: **{QpselfHealingMostInGame}**\nSentry Kills: **{QpsentryKills}**\nAverage Sentry Kills Per 10 Minutes: **{QpsentryKillsAvgPer10Min}**\nMost Sentry Kills In Game: **{QpsentryKillsMostInGame}**\nTank Kills: **{QptankKills}**\nAverage Tank Kills Per 10 Minutes: **{QpreconKills}**\nMost Tank Kills In Game: **{QptankKillsMostInGame}**");
                    embed.AddField("Competetive Hero Specific", $"Recon Kills: **{CompreconKills}**\nAverage Recon Kills Per 10 Minutes: **{CompreconKillsAvgPer10Min}**\nMost Recon Kills In Game: **{CompreconKillsMostInGame}**\nSecondary Fire Accuracy: **{CompsecondaryFireAccuracy}**\nSelf Healing: **{CompselfHealing}**\nAverage Self Healing Per 10 Minutes: **{CompselfHealingAvgPer10Min}**\nMost Self Healing In Game: **{CompselfHealingMostInGame}**\nSentry Kills: **{CompsentryKills}**\nAverage Sentry Kills Per 10 Minutes: **{CompsentryKillsAvgPer10Min}**\nMost Sentry Kills In Game: **{CompsentryKillsMostInGame}**\nTank Kills: **{ComptankKills}**\nAverage Tank Kills Per 10 Minutes: **{CompreconKills}**\nMost Tank Kills In Game: **{ComptankKillsMostInGame}**");
                }
                if (hero == "brigitte")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "dVa")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "doomfist")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "genji")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "hanzo")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "junkrat")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "lúcio")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "mccree")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "mei")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "mercy")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "moria")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "orisa")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "pharah")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "reaper")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "reinhardt")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "roadhog")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "soldier76")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "sombra")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "symmetra")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "torbjörn")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "tracer")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "widowmaker")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "winston")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "wreckingBall")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "zarya")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                if (hero == "zenyatta")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].heroSpecific.barrierDamageDone.ToString();
                    embed.AddField("Hero Specific", $"Biotic Grenade Kills: **{ }**\n");
                }
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Make sure you have played competetive and quckplay with this hero, otherwise check your command.\n**w!owhs <hero> <Your Battle.net username and id> <platform (pc/xbl/psn)> <region> Ex: w!owhs dVa Phytal-1427 pc us**");
            }
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
            else if (value == "wreckingball" || value == "hammond") return $"wreckingBall";
            else return value;
        }
    }
}
