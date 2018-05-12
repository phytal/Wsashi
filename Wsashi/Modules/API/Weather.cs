using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Watchdog.Modules.API
{
    public class Weather : ModuleBase<SocketCommandContext>
    {
        [Command("weather")]
        [Summary("Displays the weather for specified city/location Ex: /weather london uk")]
        public async Task GetWeather([Remainder] string message)
        {
           
            string countryCode = message.Split(' ').Last();
            string city = message.Remove(message.IndexOf(countryCode));



            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + city +"," + countryCode + "&appid=%608a016f744ee7cfd65de7c23abcacacc6");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string cityName = dataObject.name.ToString();
            string weather = dataObject.weather.main.ToString();
            string weatherdescription = dataObject.weather.description.ToString();
            string icon = dataObject.weather.icon.ToString();
            string temp = dataObject.main.temp.ToString();
            string ltemp = dataObject.main.temp_max.ToString();
            string htemp = dataObject.main.temp_max.ToString();
            string winds = dataObject.wind.speed.ToString();
            string windd = dataObject.wind.deg.ToString();
            string cloud = dataObject.cloud.all.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(icon);
            embed.WithTitle("Weather for your location");
            embed.AddInlineField("City Name", cityName);
            embed.AddInlineField("Weather", weather);
            embed.AddInlineField("Weather Description", weatherdescription);
            embed.AddInlineField("Temperature", temp);
            embed.AddInlineField("Low Temperature", ltemp);
            embed.AddInlineField("High Temperature", htemp);
            embed.AddInlineField("Wind Speed", winds);
            embed.AddInlineField("Wind Degree", windd);
            embed.AddInlineField("Clouds", cloud);

            await Context.Channel.SendMessageAsync("", embed: embed);
        }
    }
}