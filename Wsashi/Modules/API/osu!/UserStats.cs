using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Preconditions;

namespace Wsashi.Modules.API.osu_
{
    public class UserStats : ModuleBase
    {
        [Command("osustats", RunMode = RunMode.Async)]
        [Summary("Get a osu! user's statistics. Usage: w!osustats Phytal")]
        [Cooldown(10)]

        public async Task GetOsuStats([Remainder] string user)
        {
            var json = await Global.SendWebRequest("https://osu.ppy.sh/api/get_user?k=2ce122dfe83fb6826b3f2dfe58336006db65c138&type=string&u=" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string UserId = dataObject[0].user_id.ToString();
            string Username = dataObject[0].username.ToString();
            string rs = dataObject[0].ranked_score.ToString();
            string ts = dataObject[0].total_score.ToString();
            string pprank = dataObject[0].pp_rank.ToString();
            string lvl = dataObject[0].level.ToString();
            string ppraw = dataObject[0].pp_raw.ToString();
            string acc = dataObject[0].accuracy.ToString();
            string country = dataObject[0].country.ToString();
            string ppcountryrank = dataObject[0].pp_country_rank.ToString();
            string playcount = dataObject[0].playcount.ToString();
            string ss = dataObject[0].count_rank_ss.ToString();
            string ssh = dataObject[0].count_rank_ssh.ToString();
            string s = dataObject[0].count_rank_s.ToString();
            string sh = dataObject[0].count_rank_sh.ToString();
            string a = dataObject[0].count_rank_a.ToString();
            string c5 = dataObject[0].count50.ToString();
            string c1 = dataObject[0].count100.ToString();
            string c3 = dataObject[0].count300.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://images.discordapp.net/avatars/421879566265614337/7035b241f838c0e1de3f0ab047352d0b.png?size=512");
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":video_game:  | **{Username}'s osu! Profile**");
            embed.AddInlineField("Username", Username + $" ({country})");
            embed.AddInlineField("UserId", UserId);
            embed.AddInlineField("Level", lvl);
            embed.AddInlineField("Ranked Score", rs);
            embed.AddInlineField("Total Score", ts);
            embed.AddInlineField("PP Rank", pprank + $" ({ppcountryrank} in {country})");
            embed.AddInlineField("PP Raw", ppraw);
            embed.AddInlineField("Play Count", playcount);
            embed.AddInlineField("Accuracy", acc);
            embed.AddInlineField("Country PP Rank", ppcountryrank);
            embed.WithDescription($"**SS+:** {ssh} | **SS:** {ss} | **S+:** {sh} | **S:** {s} | **A:** {a} \n**300s:** {c3} | **100s:** {c1} | **50s:** {c5}" );

            embed.WithFooter("Powered by the osu.ppy.sh API");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }
}

