using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ImageMagick;
using System.IO;
using Wsashi.Handlers;
using Wsashi.Preconditions;

namespace Watchdog.Modules
{
    public class ImageGeneration : ModuleBase
    {


        [Command("captcha")]
        [Summary("Sends a captcha with what you inputed")]
        public async Task Captcha([Remainder] string input)
        {
            MagickReadSettings settings = new MagickReadSettings();
            settings.Width = 308;
            settings.Height = 81;
            using (MemoryStream memStream = new MemoryStream(@Desktop))
            {
                using (MagickImage image = new MagickImage(memStream))
                {
                    new Drawables()
                    .FontPointSize(72)
                    .Font("Monaco")
                    .StrokeColor(new MagickColor("black"))
                    .FillColor(MagickColors.Black)
                    .TextAlignment(TextAlignment.Left)
                    .Text(66, 37, input)
                    .Draw(image);
                    image.Format = MagickFormat.Png;
                    await Context.Channel.SendFileAsync(new MemoryStream(image.ToByteArray()), "captcha.png");
                }
            }
        }
    }
}
