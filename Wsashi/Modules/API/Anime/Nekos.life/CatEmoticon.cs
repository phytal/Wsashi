using System.Threading.Tasks;
using Discord.Commands;
using System.Net;
using Newtonsoft.Json;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;

namespace Wsashi.Modules.API
{
    public class CatEmoticon : WsashiModule
    {
        [Command("catemoticon")]
        [Summary("Displays an random cat emoticon :3")]
        [Alias("cate")]
        [Remarks("Ex: w!cate")]
        [Cooldown(5)]
        public async Task GetRandomNeko()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/cat");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.cat.ToString();

            await Context.Channel.SendMessageAsync(nekolink);
        }
    }
}
