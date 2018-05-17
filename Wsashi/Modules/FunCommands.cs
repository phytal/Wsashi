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
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class FunCommands : ModuleBase

    {
        [Command("normalhello")]
        [Summary("A simple hello command made with html")]
        public async Task Hello(string color = "blue")
        {
            string css = "<style>\n    h1{\n        background-color: " + color + ";\n    }\n</style>\n";
            string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);
            var converter = new HtmlToImageConverter
            {
                Width = 250,
                Height = 55
            };
            var jpgBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");
        }

        [Command("ping")]
        [Summary("Ping Pong!")]
        public async Task Ping()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":ping_pong:  | Pong! {(Context.Client as DiscordSocketClient).Latency}ms");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        string[] predictionsTexts = new string[]
            {
                ":8ball:  | It is certain",
                ":8ball:  | It is decidedly so",
                ":8ball:  | Without a doubt",
                ":8ball:  | Yes definitely",
                ":8ball:  | You may rely on it",
                ":8ball:  | As I see it, yes",
                ":8ball:  | Most likely",
                ":8ball:  | Outlook good",
                ":8ball:  | Yes",
                ":8ball:  | Signs point to yes",
                ":8ball:  | Reply hazy try again",
                ":8ball:  | Ask again later",
                ":8ball:  | Better not tell you now",
                ":8ball:  | Cannot predict now",
                ":8ball:  | Concentrate and ask again",
                ":8ball:  | Don't count on it",
                ":8ball:  | My reply is no",
                ":8ball:  | My sources say no",
                ":8ball:  | Outlook not so good",
                ":8ball:  | Very doubtful"
            };
        Random rand = new Random();

        [Command("8ball")]
        [Alias("eightball")]
        [Summary("Gives a prediction")]
        public async Task EightBall([Remainder] string input)
        {
            int randomIndex = rand.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(text + ", " + Context.User.Username);
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        string[] Roasts = new string[]
         {
             "You shouldn't play hide and seek, no one would look for you.",
             "You bring everyone a lot of joy, when you leave the room.",
             "I'd like to kick you in the teeth, but that would be an improvement!",
             "Why don't you check eBay and see if they have a life for sale.",
             "You're so ugly, you scared the crap out of the toilet.",
             "I may love to shop but I'm not buying your bullshit.",
             "There are more calories in your stomach than in the local supermarket!",
             "Maybe if you ate some of that makeup you could be pretty on the inside.",
             "You're the reason they invented double doors!",
             "Oh my God, look at you. Was anyone else hurt in the accident?",
             "Your parents hated you so much your bath toys were an iron and a toaster.",
             "Your family tree must be a cactus because everybody on it is a prick.",
             "I fart to make you smell better.",
             "Hey, you have somthing on your chin... no, the 3rd one down.",
             "I’m jealous of all the people that haven't met you!",
             "Oh my God, look at you. Was anyone else hurt in the accident?",
             "You have two brains cells, one is lost and the other is out looking for it.",
             "You're so fat, when you wear a yellow rain coat people scream: TAXI.",
             "If I were to slap you, it would be considered animal abuse!",
             "I bet your brain feels as good as new, seeing that you never use it.",
             "I don't exactly hate you, but if you were on fire and I had water, I'd drink it.",
             "You're so ugly you make blind kids cry.",
             "It's better to let someone think you are an Idiot than to open your mouth and prove it.",
             "You're as bright as a black hole, and twice as dense.",
             "You do realize makeup isn't going to fix your stupidity?",
             "You're so fat the only letters of the alphabet you know are KFC.",
             "Two wrongs don't make a right, take your parents as an example.",
             "You're so ugly you scare the shit back into people.",
             "Somewhere out there is a tree, tirelessly producing oxygen so you can breathe. I think you owe it an apology.",
             "You're not funny, but your life, now that's a joke.",
             "It's kinda sad watching you attempt to fit your entire vocabulary into a sentence.",
             "If I wanted to hear from an asshole, I'd fart.",
             "If you really want to know about mistakes, you should ask your parents.",
             "Do you know how long it takes for your mother to take a crap? Nine months.",
             "You are proof that evolution CAN go in reverse.",
             "I could eat a bowl of alphabet soup and shit out a smarter statement than that.",
             "Why don't you slip into something more comfortable -- like a coma.",
             "You're so fat, you could sell shade.",
             "You're so ugly, when you popped out the doctor said: Aww what a treasure. And your mom said: Yeah, lets bury it.",
             "I'm not saying I hate you, but I would unplug your life support to charge my phone.",
             "If you're gonna be a smartass, first you have to be smart. Otherwise you're just an ass.",
             "If laughter is the best medicine, your face must be curing the world.",
             "I can explain it to you, but I can't understand it for you.",
             "Roses are red violets are blue, God made me pretty, what happened to you?",
             "What are you doing here? Did someone leave your cage open?",
             "You're the best at all you do - and all you do is make people hate you.",
             "I bet your brain feels as good as new, seeing that you never use it.",
             "I love what you've done with your hair. How do you get it to come out of the nostrils like that?",
             "You're not funny, but your life, now that's a joke.",
             "Do you still love nature, despite what it did to you?",
             "I'm jealous of all the people that haven't met you!",
             "Shock me, say something intelligent.",
             "What are you going to do for a face when the baboon wants his butt back?",
             "If your brain was made of chocolate, it wouldn't fill an M&M.",
             "If you spoke your mind, you'd be speechless.",
             "The last time I saw a face like yours I fed it a banana.",
             "So you've changed your mind, does this one work any better?",
             "Your birth certificate is an apology letter from the condom factory.",
             "Shut up, you'll never be the man your mother is.",
             "I'd like to see things from your point of view but I can't seem to get my head that far up my ass.",
             "You're so ugly, the only dates you get are on a calendar",
             "Looks like you traded in your neck for an extra chin!",
             "Well I could agree with you, but then we'd both be wrong.",
             "I'd slap you, but shit stains.",
             "How many times do I have to flush to get rid of you?"

    };

        [Command("roast")]
        [Summary("Roasts @Username")]
        [Alias("burn")]
        public async Task Roast(SocketGuildUser user)
        {
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("You must mention a user");
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                int randomIndex = rand.Next(Roasts.Length);
                string text = Roasts[randomIndex];
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle(":fire:  | " + user.Username + ", " + text);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("pick")]
        [Summary("Makes the bot pick something. Divide your choices with the '|' symbol")]
        public async Task PickOne([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("I Choose...");
            embed.WithDescription(selection);
            embed.WithColor(new Color(37, 152, 255));
            embed.WithThumbnailUrl("https://emojipedia-us.s3.amazonaws.com/thumbs/160/facebook/92/thinking-face_1f914.png");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("GoodMorning")]
        [Summary("Says Good Morning")]
        [Alias("Morning", "Ohayo", "Ohayōgozaimasu")]
        public async Task GoodMorning()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Good Morning " + Context.User.Username + "!");
            embed.WithDescription(":rooster:  | Cock a Doodle Do!");
            embed.WithColor(new Color(30, 144, 255));
            embed.WithThumbnailUrl("https://cdn.pixabay.com/photo/2016/03/31/23/34/emote-1297695_960_720.png");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("GoodNight")]
        [Summary("Says Good Night")]
        [Alias("Night", "Oyasumi")]
        public async Task GoodNight()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Good Night " + Context.User.Username + "!");
            embed.WithDescription(":sleeping_accommodation:  | Don't let the Bed Bugs Bite!");
            embed.WithColor(new Color(0, 128, 0));
            embed.WithThumbnailUrl("https://cdn.shopify.com/s/files/1/1061/1924/products/Dark_Blue_Moon_Emoji_large.png?v=1480481043");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("echo")]
        [Summary("Make The Bot Say A Message")]
        public async Task Echo([Remainder] string message)
        {
            var LocalTime = DateTime.Now;
            var Sender = Context.Message.Author;

            var embed = new EmbedBuilder();
            embed.WithTitle(message);
            embed.WithFooter(LocalTime + " Message from " + Sender);
            embed.WithColor(37, 152, 255);

            await Context.Channel.SendMessageAsync("", false, embed);
            await Context.Message.DeleteAsync();
        }

        [Command("Hello")]
        [Summary("Says a formatted hello")]
        [Cooldown(5)]
        public async Task SayHello()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Hello!");
            embed.WithDescription("My name is Wsashi, a bot created by Phytal! (And possibly your Waifu..)");
            embed.WithColor(37, 152, 255);
            embed.WithImageUrl(Global.Client.CurrentUser.GetAvatarUrl());

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("lmgtfy")]
        [Summary("Sends an Let me Google that for you link")]
        public async Task lmgtfy([Remainder]string link = "enter+something")
        {
            link = link.Replace(' ', '+');
            await ReplyAsync("https://lmgtfy.com/?q=" + link);
        }

        [Command("google")]
        [Summary("Sends a Google link")]
        public async Task google([Remainder]string link = "enter+something")
        {
            link = link.Replace(' ', '+');
            await ReplyAsync("https://www.google.com/search?q=" + link);
        }

        [Command("YouTube"), Alias("Yt")]
        public async Task SearchYouTube([Remainder]string query)
        {
            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://www.freepnglogos.com/uploads/youtube-logo-hd-8.png");

            var url = "https://youtube.com/results?search_query=";
            var newQuery = query.Replace(' ', '+');
            embed.WithDescription(url + newQuery);

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Lenny")]
        public async Task LennyLol()
        {
            await Context.Channel.SendMessageAsync("( ͡° ͜ʖ ͡°)");
        }

        [Command("Prefix")]
        public async Task GetPrefixForServer()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            string prefix;
            switch (config)
            {
                case null:
                    prefix = "w!";
                    break;
                default:
                    prefix = config.CommandPrefix;
                    break;
            }

            await Context.Channel.SendMessageAsync($"The prefix for this server is {prefix}.");
        }
    }
}
