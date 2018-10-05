using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Wsashi.Preconditions;

namespace Watchdog.Modules.API
{
    public class Weather : ModuleBase<SocketCommandContext>
    {
        [Command("weather")]
        [Summary("Displays the weather for specified major city")]
        [Remarks("w!weather <city> Ex: w!weather london")]
        [Cooldown(10)]
        public async Task GetWeather([Remainder] string message)
        {
           
            //string countryCode = message.Split(' ').Last();
            //string city = message.Remove(message.IndexOf(countryCode));

            var json = await Wsashi.Global.SendWebRequest("http://api.openweathermap.org/data/2.5/weather?q=" + message + "&APPID=8a016f744ee7cfd65de7c23abcacacc6&units=metric");

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string cityName = dataObject.name.ToString();
            string weather = dataObject.weather[0].main.ToString();
            //string weatherdescription = dataObject.weather[0].description.ToString();
            string icon = dataObject.weather[0].icon.ToString();
            string temp = dataObject.main.temp.ToString();
            string ltemp = dataObject.main.temp_max.ToString();
            string htemp = dataObject.main.temp_max.ToString();
            string humid = dataObject.main.humidity.ToString();
            //string winds = dataObject.wind.speed.ToString();
            //string windd = dataObject.wind.deg.ToString();
            //string cloud = dataObject.clouds.all.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle($"Weather for {cityName}");
            embed.WithThumbnailUrl($"http://openweathermap.org/img/w/{icon}.png");
            embed.AddField("City Name :cityscape:️:", cityName, true);
            embed.AddField("Weather :satellite::", weather, true);
            //embed.AddField("Weather Description", weatherdescription, true);
            embed.AddField("Temperature :thermometer::", temp + "°C", true);
            embed.AddField("Humidity :droplet::", humid, true);
            embed.AddField("Low Temperature :arrow_down::", ltemp + "°C", true);
            embed.AddField("High Temperature :arrow_up::", htemp + "°C", true);
            //embed.AddField("Wind Speed", winds, true);
            //embed.AddField("Wind Degree", windd, true);
            //embed.AddField("Clouds", cloud, true);
            embed.WithFooter("powered by OpenWeatherMap API // use w!weather <city>!");

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}