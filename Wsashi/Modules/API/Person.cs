using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using Discord;
using System.Net;
using Wsashi.Preconditions;

namespace Watchdog.Modules.API
{
    public class Person : ModuleBase
    {
        [Command("person")]
        [Summary("Gets a random person with random credentials")]
        [Remarks("Ex: w!person")]
        [Cooldown(10)]
        public async Task GetRandomPerson()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?nat=US");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string firstName = dataObject.results[0].name.first.ToString();
            string lastName = dataObject.results[0].name.last.ToString();
            string gender = dataObject.results[0].gender.ToString();
            string avatarURL = dataObject.results[0].picture.large.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(avatarURL);
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Generated Person");
            embed.AddInlineField("Gender", gender);
            embed.AddInlineField("First Name", firstName);
            embed.AddInlineField("Last Name", lastName);

            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }
}
