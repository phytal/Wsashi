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
        [Alias("")]
        [Remarks("w!owherostats <hero> <Your Battle.net username and id> <platform (pc/xbl/psn)> <region (us/eu etc.)> Ex: w!owherostats dVa Phytal-1427 pc us")]
        [Cooldown(10)]
        public async Task GetOwCompStats(string hero, string username, string platform, string region)
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);
                hero = hero.ToLower();
                hero = GetHero(hero);

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/heros/{hero}");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string srIcon = dataObject.ratingIcon.ToString();
                //avg
                string AllDamageAvg = dataObject.competitiveStats.careerStats[hero].average.allDamageDoneAvgPer10Min.ToString();
                string BarrierDamageAvg = dataObject.competitiveStats.careerStats[hero].average.barrierDamageDoneAvgPer10Min.ToString();
                string CriticalsAvg = dataObject.competitiveStats.careerStats[hero].average.criticalHitsAvgPer10Min.ToString();
                string DeathAvg = dataObject.competitiveStats.careerStats[hero].average.deathsAvgPer10Min.ToString();
                string ElimAvg = dataObject.competitiveStats.careerStats[hero].average.eliminationsAvgPer10Min.ToString();
                string ElimPerLife = dataObject.competitiveStats.careerStats[hero].average.eliminationsPerLife.ToString();
                string FinalBlowAvg = dataObject.competitiveStats.careerStats[hero].average.finalBlowsAvgPer10Min.ToString();
                string HeroDamageAvg = dataObject.competitiveStats.careerStats[hero].average.heroDamageDoneAvgPer10Min.ToString();
                string MeleeAvg = dataObject.competitiveStats.careerStats[hero].average.meleeFinalBlowsAvgPer10Min.ToString();
                string ObjKillsAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveKillsAvgPer10Min.ToString();
                string ObjTimeAvg = dataObject.competitiveStats.careerStats[hero].average.objectiveTimeAvgPer10Min.ToString();
                string SoloKillAvg = dataObject.competitiveStats.careerStats[hero].average.soloKillsAvgPer10Min.ToString();
                string OnFireAvg = dataObject.competitiveStats.careerStats[hero].average.timeSpentOnFireAvgPer10Min.ToString();
                //best
                string AllDamageInGame = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInGame.ToString();
                string AllDamageInLife = dataObject.competitiveStats.careerStats[hero].best.allDamageDoneMostInLife.ToString();
                string BarrierDamageInGame = dataObject.competitiveStats.careerStats[hero].best.barrierDamageDoneMostInGame.ToString();
                string CritMostInGame = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInGame.ToString();
                string CritMostInLife = dataObject.competitiveStats.careerStats[hero].best.criticalHitsMostInLife.ToString();
                string ElimMostInLife = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInLife.ToString();
                string ElimMostInGame = dataObject.competitiveStats.careerStats[hero].best.eliminationsMostInGame.ToString();
                string FinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.finalBlowsMostInGame.ToString();
                string HeroDmgMostInGame = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInGame.ToString();
                string HeroDmgMostInLife = dataObject.competitiveStats.careerStats[hero].best.heroDamageDoneMostInLife.ToString();
                string KillStreakBest = dataObject.competitiveStats.careerStats[hero].best.killsStreakBest.ToString();
                string MeleeFinalBlowMostInGame = dataObject.competitiveStats.careerStats[hero].best.meleeFinalBlowsMostInGame.ToString();
                string MultikillBest = dataObject.competitiveStats.careerStats[hero].best.multikillsBest.ToString();
                string ObjKillMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveKillsMostInGame.ToString();
                string ObjTimeMostInGame = dataObject.competitiveStats.careerStats[hero].best.objectiveTimeMostInGame.ToString();
                string SoloKillsMostInGame = dataObject.competitiveStats.careerStats[hero].best.soloKillsMostInGame.ToString();
                string OnFireMostInGame = dataObject.competitiveStats.careerStats[hero].best.timeSpentOnFireMostInGame.ToString();
                string WeaponAccuracyBestInGame = dataObject.competitiveStats.careerStats[hero].best.weaponAccuracyBestInGame.ToString();
                //combat
                string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
                string CriticalHits = dataObject.competitiveStats.careerStats[hero].combat.criticalHits.ToString();
                string CriticalHitsAccuracy = dataObject.competitiveStats.careerStats[hero].combat.criticalHitsAccuracy.ToString();
                string DamageDone = dataObject.competitiveStats.careerStats[hero].combat.damageDone.ToString();
                string Deaths = dataObject.competitiveStats.careerStats[hero].combat.deaths.ToString();
                string Elims = dataObject.competitiveStats.careerStats[hero].combat.eliminations.ToString();
                string EnvironmentalKills = dataObject.competitiveStats.careerStats[hero].combat.environmentalKills.ToString();
                string FinalBlows = dataObject.competitiveStats.careerStats[hero].combat.finalBlows.ToString();
                string HeroDmg = dataObject.competitiveStats.careerStats[hero].combat.heroDamageDone.ToString();
                string MeleeFinalBlows = dataObject.competitiveStats.careerStats[hero].combat.meleeFinalBlows.ToString();
                string Multikills = dataObject.competitiveStats.careerStats[hero].combat.multikills.ToString();
                string ObjKills = dataObject.competitiveStats.careerStats[hero].combat.objectiveKills.ToString();
                string ObjTime = dataObject.competitiveStats.careerStats[hero].combat.objectiveTime.ToString();
                string MeleeAccuracy = dataObject.competitiveStats.careerStats[hero].combat.quickMeleeAccuracy.ToString();
                string SoloKills = dataObject.competitiveStats.careerStats[hero].combat.soloKills.ToString();
                string OnFire = dataObject.competitiveStats.careerStats[hero].combat.timeSpentOnFire.ToString();
                string WeaponAccuracy = dataObject.competitiveStats.careerStats[hero].combat.weaponAccuracy.ToString();
                //herospecific
                if (hero == "ana")
                {
                    string BarrierDmgDone = dataObject.competitiveStats.careerStats[hero].combat.barrierDamageDone.ToString();
                }





                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OWAPI API",
                    IconUrl = srIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Competitive Overwatch Profile",
                    IconUrl = endorsementIcon
                };

                var embed = new EmbedBuilder()
                {
                    Author = top,
                    Footer = bottom
                };
                embed.WithThumbnailUrl(playerIcon);
                embed.WithColor(37, 152, 255);
                embed.AddField("Competitive Game Stats", $"Games Played: **{compgames}**\nGames Won: **{compwon}**", true);
                embed.AddField("Competitive Medals", $"Total Medals: **{compmedal}**\n:first_place: Gold Medals: **{compmedalGold}**\n:second_place: Silver Medals: **{compmedalSilver}**\n:third_place: Bronze Medals: **{compmedalBronze}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nSR: **{sr}**\nEndorsement Level: **{endorsement}**", true);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Oops! Are you sure that your Overwatch career profile is set to public and you typed in your username correctly?\n**w!owsc <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstatscomp Phytal-1427 pc**");
            }
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
