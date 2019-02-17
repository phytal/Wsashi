using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;

namespace Wsashi.Modules.API.weebDotSh
{
    public class WebRequest
    {
        WeebClient weebClient = new WeebClient("Wsashi", Config.bot.Version);
        public static async Task<string> SendWebRequest(string requestUrl)
        {
            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Add("User-Agent", $"Wsashi/{Config.bot.Version}/beta");
                client.DefaultRequestHeaders.Add("Authorization: Wolke", Config.bot.wolkeToken);
                using (var response = await client.GetAsync(requestUrl))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return response.StatusCode.ToString();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public async Task<RandomData> GetTypesAsync(string type, IEnumerable<string> tags, FileType fileType, NsfwSearch nsfw, bool hidden)
        {
            var result = await weebClient.GetRandomAsync(type, tags, fileType, hidden, nsfw); //hidden and nsfw are always defaulted to false

            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}
