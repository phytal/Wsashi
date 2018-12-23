
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Wsashi.Configuration;

namespace Wsashi
{
    internal class DiscordBotsList
    {
        internal static async Task<string> UpdateServerCount(DiscordSocketClient client)
        {
            var webclient = new HttpClient();
            var content = new StringContent($"{{ \"server_count\": {client.Guilds.Count} }}", Encoding.UTF8, "application/json");
            webclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxNzE2MDk1NzAxMDExNjYwOCIsImJvdCI6dHJ1ZSwiaWF0IjoxNTQ1NDU0NTY3fQ.YZmPvuEtQCu4ZYTcmikCEKSCOY0h0 - KB_fYfhXRmFDk");
            var resp = await webclient.PostAsync($"https://discordbots.org/api/bots/{client.CurrentUser.Id}/stats", content);
            return resp.Content.ReadAsStringAsync().ToString();
        }
    }
}