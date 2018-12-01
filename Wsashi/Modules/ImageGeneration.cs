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

namespace Wsashi.Modules
{
    public class ImageGeneration : ModuleBase
    {
        [Command("gru")]
        [Summary("Sends a gru meme with what you inputed")]
        [Cooldown(20, true)]
        [Remarks("w!gru <front first box> <second box> <last boxes> Ex: w!gru f+d/l")]
        public async Task Gru([Remainder] string input)
        {
            string thirdf = input.Split('/').Last();
            string g = input.Remove(input.IndexOf(thirdf));
            string second = g.Split('+').Last();
            string first = input.Split('+').First();
            second = second.Replace('/', ' ');

            using (MagickImage image = new MagickImage(@Path.Combine(Constants.MemeFolder, "grumeme.jpg")))
            {
                new Drawables()
                .FontPointSize(18)
                .Font("Monaco")
                .StrokeColor(new MagickColor("black"))
                .FillColor(MagickColors.Black)
                .TextAlignment(TextAlignment.Left)
                .Text(307, 98, first) 
                .Text(816, 94, second)
                .Text(316, 434, thirdf)
                .Text(829, 434, thirdf)
                .Draw(image);
                image.Format = MagickFormat.Jpg;
                await Context.Channel.SendFileAsync(new MemoryStream(image.ToByteArray()), "grumeme.jpg");

            }
        }

        [Command("butterflyman"), Alias("bfm")]
        [Summary("Sends a butterfly man meme with what you inputed")]
        [Remarks("w!bfm <man> <butterfly> <caption> w!bfm f+d/l")]
        [Cooldown(10, true)]
        public async Task Bman([Remainder] string input)
        {
            string ann = input.Split('/').Last();
            string g = input.Remove(input.IndexOf(ann));
            string bfly = g.Split('+').Last();
            string man = input.Split('+').First();
            bfly = bfly.Replace('/', ' ');

            using (MagickImage image = new MagickImage(@Path.Combine(Constants.MemeFolder, "butterfly anime man idk meme tem.jpg")))
            {
                new Drawables()
                .FontPointSize(50)
                .Font("Impact")
                .StrokeColor(new MagickColor("black"))
                .StrokeWidth(3)
                .FillColor(MagickColors.White)
                .TextAlignment(TextAlignment.Center)
                .Text(431, 395, man)
                .Text(916, 203, bfly)
                .TextAlignment(TextAlignment.Left)
                .Text(607, 857, ann)
                .Draw(image);
                image.Format = MagickFormat.Jpg;
                await Context.Channel.SendFileAsync(new MemoryStream(image.ToByteArray()), "meme.jpg");

            }
        }
        /*[Command("merge")]
        [Summary("Sends a captcha with what you inputed")]
        public async Task Ca()
        {
            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Add the first image
                MagickImage first = new MagickImage("Snakeware.png");
                images.Add(first);

                var url = Context.User.GetAvatarUrl();
                // Add the second image
                MagickImage second = new MagickImage("Snakeware.png");
                images.Add(second);

                // Create a mosaic from both images
                using (IMagickImage result = images.Add())
                {
                    // Save the result
                    result.Write("Mosaic.png");
                }
            }
        }
        [Command("quant")]
        [Summary("Sends a captcha with what you inputed")]
        public async Task Cqu([Remainder] string input)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                // Add first image and set the animation delay (in 1/100th of a second) 
                collection.Add("Snakeware.png");
                collection[0].AnimationDelay = 100; // in this example delay is 1000ms/1sec

                // Add second image, set the animation delay (in 1/100th of a second) and flip the image
                collection.Add("Snakeware.png");
                collection[1].AnimationDelay = 100; // in this example delay is 1000ms/1sec
                collection[1].Flip();

                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);

                // Optionally optimize the images (images should have the same size).
                collection.Optimize();

                // Save gif
                collection.Write("Snakeware.Animated.gif");
            }
        }
        */
    }
}
