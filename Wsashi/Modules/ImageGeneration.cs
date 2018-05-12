using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using NReco.ImageGenerator;
using System.IO;
using Wsashi.Handlers;
using Wsashi.Preconditions;

namespace Watchdog.Modules
{
    public class ImageGeneration : ModuleBase
    {
        [Command("captcha")]
        [Summary("Sends a captcha with what you inputed")]
        public async Task Hello([Remainder] string input)
        {
            string html = String.Format(input);
            var converter = new HtmlToImageConverter
            {
                Width = 800,
                Height = 500
            };
            var jpgBytes = converter.GenerateImageFromFile("https://i.imgur.com/dj58QLj.png" + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "captcha.jpg");
        }
    }
}
