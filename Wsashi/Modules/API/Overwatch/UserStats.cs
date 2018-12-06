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
using Wsashi.Entities;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.Overwatch
{
    public class UserStats : WsashiModule
    {
        [Command("owstats")]
        [Summary("Get a Overwatch user's statistics.")]
        [Alias("ows", "overwatchstats")]
        [Remarks("w!owstats <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstats Phytal-1427 pc")]
        [Cooldown(10)]
        public async Task GetOwStats(string username, string platform, string region)
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/profile");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string compcards = dataObject.competitiveStats.awards.cards.ToString();
                string compmedal = dataObject.competitiveStats.awards.medals.ToString();
                string compmedalGold = dataObject.competitiveStats.awards.medalsGold.ToString();
                string compmedalSilver = dataObject.competitiveStats.awards.medalsSilver.ToString();
                string compmedalBronze = dataObject.competitiveStats.awards.medalsBronze.ToString();
                string compgames = dataObject.competitiveStats.games.played.ToString();
                string compwon = dataObject.competitiveStats.games.won.ToString();

                string qpcards = dataObject.quickPlayStats.awards.cards.ToString();
                string qpmedal = dataObject.quickPlayStats.awards.medals.ToString();
                string qpmedalGold = dataObject.quickPlayStats.awards.medalsGold.ToString();
                string qpmedalSilver = dataObject.quickPlayStats.awards.medalsSilver.ToString();
                string qpmedalBronze = dataObject.quickPlayStats.awards.medalsBronze.ToString();
                string qpgames = dataObject.quickPlayStats.games.played.ToString();
                string qpwon = dataObject.quickPlayStats.games.won.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                string sr = dataObject.rating.ToString();
                string srIcon = dataObject.ratingIcon.ToString();

                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OW-API",
                    IconUrl = srIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Overwatch Profile",
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
                embed.AddField("Quickplay Game Stats", $"Games Played: **{qpgames}**\nGames Won: **{qpwon}**", true);
                embed.AddField("Competitive Medals", $"Total Medals: **{compmedal}**\n:first_place: Gold Medals: **{compmedalGold}**\n:second_place: Silver Medals: **{compmedalSilver}**\n:third_place: Bronze Medals: **{compmedalBronze}**", true);
                embed.AddField("Quickplay Medals", $"Total Medals: **{qpmedal}**\n:first_place: Gold Medals: **{qpmedalGold}**\n:second_place: Silver Medals: **{qpmedalSilver}**\n:third_place: Bronze Medals: **{qpmedalBronze}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nSR: **{sr}**\nEndorsement Level: **{endorsement}**", true);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Oops! Are you sure that your Overwatch career profile is set to public and you typed in your username correctly?\n**w!ows <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstats Phytal-1427 pc**\nNote that you must have completed your placement matches in competetive for this to show up, otherwise use w!owsqp");
            }
        }

        [Command("owstatsqp")]
        [Summary("Get a Overwatch user's Quickplay statistics.")]
        [Alias("owsqp", "overwatchstatsqp", "owsquickplay")]
        [Remarks("w!owsqp <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstatsqp Phytal-1427 pc")]
        [Cooldown(10)]
        public async Task GetOwQPStats(string username, string platform, string region)
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/profile");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string qpcards = dataObject.quickPlayStats.awards.cards.ToString();
                string qpmedal = dataObject.quickPlayStats.awards.medals.ToString();
                string qpmedalGold = dataObject.quickPlayStats.awards.medalsGold.ToString();
                string qpmedalSilver = dataObject.quickPlayStats.awards.medalsSilver.ToString();
                string qpmedalBronze = dataObject.quickPlayStats.awards.medalsBronze.ToString();
                string qpgames = dataObject.quickPlayStats.games.played.ToString();
                string qpwon = dataObject.quickPlayStats.games.won.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string levelIcon = dataObject.levelIcon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OWAPI API",
                    IconUrl = levelIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Quickplay Overwatch Profile",
                    IconUrl = endorsementIcon
                };

                var embed = new EmbedBuilder()
                {
                    Author = top,
                    Footer = bottom
                };
                embed.WithThumbnailUrl(playerIcon);
                embed.WithColor(37, 152, 255);
                embed.AddField("Quickplay Game Stats", $"Games Played: **{qpgames}**\nGames Won: **{qpwon}**", true);
                embed.AddField("Quickplay Medals", $"Total Medals: **{qpmedal}**\n:first_place: Gold Medals: **{qpmedalGold}**\n:second_place: Silver Medals: **{qpmedalSilver}**\n:third_place: Bronze Medals: **{qpmedalBronze}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nEndorsement Level: **{endorsement}**", true);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Oops! Are you sure that your Overwatch career profile is set to public and you typed in your username correctly?\n**w!owstatsqp <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owsqp Phytal-1427 pc**");
            }
        }

        [Command("owstatscomp")]
        [Summary("Get a Overwatch user's Competitive statistics.")]
        [Alias("owsc", "overwatchstatscomp", "owscompetitive")]
        [Remarks("w!owsc <Your Battle.net username and id> <platform (pc/xbl/psn)> Ex: w!owstatscomp Phytal-1427 pc")]
        [Cooldown(10)]
        public async Task GetOwCompStats(string username, string platform, string region)
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/profile");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string compcards = dataObject.competitiveStats.awards.cards.ToString();
                string compmedal = dataObject.competitiveStats.awards.medals.ToString();
                string compmedalGold = dataObject.competitiveStats.awards.medalsGold.ToString();
                string compmedalSilver = dataObject.competitiveStats.awards.medalsSilver.ToString();
                string compmedalBronze = dataObject.competitiveStats.awards.medalsBronze.ToString();
                string compgames = dataObject.competitiveStats.games.played.ToString();
                string compwon = dataObject.competitiveStats.games.won.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                string sr = dataObject.rating.ToString();
                string srIcon = dataObject.ratingIcon.ToString();

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
    }
}
