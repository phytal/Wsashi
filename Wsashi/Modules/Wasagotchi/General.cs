using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules.Wasagotchi
{
    public class General : WsashiModule
    {
        [Command("wasagotchi stats")]
        public async Task WasagotchiUser([Remainder]string arg = "")
        {
            SocketUser user = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            user = mentionedUser ?? Context.User;
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastStats = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else //show their Wasagotchi status
            {
                var thumbnailurl = Context.User.GetAvatarUrl();
                var auth = new EmbedAuthorBuilder()
                {
                    Name = $"{Context.User.Username}'s Wasagotchi",
                    IconUrl = thumbnailurl,
                };
                var embed = new EmbedBuilder()
                {
                    Author = auth
                };
                if (config.pfp == null)
                    embed.WithThumbnailUrl("https://i.imgur.com/6AaY08I.png");
                else
                    embed.WithThumbnailUrl(config.pfp);
                embed.WithColor(37, 152, 255);
                embed.AddInlineField("Owner", user);
                if (config.Name == null)
                    embed.AddInlineField("Name", "*(Name your wasagotchi!)*");
                else
                    embed.AddInlineField("Name", config.Name);
                embed.AddInlineField("Exp", config.XP);
                embed.AddInlineField("Level", config.LevelNumber);
                embed.AddInlineField("Room",  GetRooms(config.rLvl));
                embed.AddInlineField("Waste", config.Waste);
                embed.AddInlineField("Attention", config.Attention);
                embed.AddInlineField("Hunger", config.Hunger);
                embed.AddInlineField("Sick", config.Sick);
                embed.AddInlineField("Ran Away", config.RanAway);
                if (config.pfp == null)
                    embed.AddInlineField("Picture", "*Default*");
                else
                embed.AddInlineField("Picture", "*Custom*");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        [Command("wasagotchi help")]
        public async Task WasagotchiHelp()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastHelp = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            
            string[] footers = new string[]
{
                "Every 4 hours all Wasagotchis will have a time modifier, -1 hunger, -1 attention, and +1 waste. Make sure to check on your Wasagotchi often!",
                "If the living conditions you provide for your Wasagotchi are too low - never clean, never play, etc - it will run away! (Your room will remain the same)",
                "If your Wasagotchi is sick, buy it some medicine with w!buy.",
                "All Wasagotchi commands have a 8 second cooldown."
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle("<:wasagotchi:454535808079364106> Wasagotchi Command List");
            embed.AddInlineField("w!wasagotchi help", "Brings up the help commmand (lol)");
            embed.AddInlineField("w!wasagotchi stats", "Brings up the stats/info of your or someone else's Wasagotchi!");
            embed.AddInlineField("w!wasagotchi name", "Set the name of your Wasagotchi!");
            embed.AddInlineField("w!wasagotchi picture", "Set the picture of your Wasagotchi! (Note: It must be a direct link)");
            embed.AddInlineField("w!wasagotchi feed", "Feeds your Wasagtochi at the cost of Potatos! Otherwise it will starve!");
            embed.AddInlineField("w!wasagotchi clean", "Clean up your Wasagotchi's waste, Otherwise it'll get sick!");
            embed.AddInlineField("w!wasagotchi play", "Play with your wasagotchi! Your Wasagotchi must have high attention levels at all time!");
            embed.AddInlineField("w!wasagotchi train", "Train your Wasagotchi to earn Exp and level up!");
            embed.WithFooter(text);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("wasagotchi name")]
        public async Task WasagotchiName([Remainder] string name)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastName = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                config.Name = name;
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your <:wasagotchi:454535808079364106> Wasagotchi's name to **{name}**!");
            }
        }

        [Command("wasagotchi picture")]
        public async Task WasagotchiPfp([Remainder] string name)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastPicture = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                config.pfp = name;
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your <:wasagotchi:454535808079364106> Wasagotchi's picture to **{name}**!");
            }
        }


        [Command("wasagotchi feed")]
        public async Task WasagotchiFeed()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastFeed = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Hunger == 20)
                {
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, your <:wasagotchi:454535808079364106> Wasagotchi is full!");
                    return;
                }
                {
                    Random rnd = new Random();
                    int cost = rnd.Next(54, 113);
                    Random rand = new Random();
                    int much = rnd.Next(4, 8);
                    uint hungerr = Convert.ToUInt32(much);
                    config.Hunger += hungerr;
                    if (config.Hunger > 20)
                    {
                        config.Hunger = 20;
                    }
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, you fill your <:wasagotchi:454535808079364106> Wasagotchi's bowl with food. It looks happy! **(+{hungerr} food [-{cost} :potato:])**");
                }
            }
        }


        string[] cleanTexts = new string[]
{
                "you hold your nose and start cleaning up the mess. ",
                "you clean up your <:wasagotchi:454535808079364106> Wasagotchi's...business!",
};

        [Command("wasagotchi clean")]
        public async Task WasagotchiClean()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastClean = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Waste == 0)
                {
                    await Context.Channel.SendMessageAsync($":sparkles:  | **{Context.User.Username}, your <:wasagotchi:454535808079364106> Wasagotchi's room is squeaky clean!**");
                    return;
                }
                {
                    Random rand = new Random();
                    int randomIndex = rand.Next(cleanTexts.Length);
                    string text = cleanTexts[randomIndex];
                    int much = rand.Next(4, 8);
                    uint clean = Convert.ToUInt32(much);
                    config.Waste -= clean;
                    if (config.Waste > 20)
                    {
                        config.Waste = 0;
                    }
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":sparkles:  |  **{Context.User.Username}**, {text} **(-{clean} waste)**");
                }
            }
        }
        string[] playTexts = new string[]
    {
                "you entertain your <:wasagotchi:454535808079364106> Wasagotchi. It seems to like you!",
                "you throw a ball and your <:wasagotchi:454535808079364106> Wasagotchi fetches it!",
    };

        [Command("wasagotchi play")]
        public async Task WasagotchiPlay()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastPlay = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Attention == 20)
                {
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}, your <:wasagotchi:454535808079364106> Wasagotchi is bored of playing right now!**");
                    return;
                }
                {
                    Random rand = new Random();
                    Random randd = new Random();
                    int randomIndex = rand.Next(playTexts.Length);
                    string text = playTexts[randomIndex];
                    int much = randd.Next(4, 8);
                    uint clean = Convert.ToUInt32(much);
                    config.Attention += clean;
                    if (config.Attention > 20)
                    {
                        config.Attention = 20;
                    }
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}**, {text} **(+{clean} attention)**");
                }
            }
        }

        string[] yesTrainTexts = new string[]
{
                "Somehow, you managed to get your <:wasagotchi:454535808079364106> Wasagotchi to listen! It is making progress.",
                "Your <:wasagotchi:454535808079364106> Wasagotchi seems to respond well to the training! It looks happy.",
};

        string[] noTrainTexts = new string[]
{
                "Despite your best attempts, your <:wasagotchi:454535808079364106> Wasagotchi does not want to learn. Persistence is key!",
                "You wonder why your <:wasagotchi:454535808079364106> Wasagotchi won't listen. Maybe you should give it a few more tries.",
                "Your <:wasagotchi:454535808079364106> Wasagotchi sits down and looks excited, but doesn't do what you wanted.",
                "Your <:wasagotchi:454535808079364106> Wasagotchi doesn't quite understand what you are trying to do. Try again!",
};

        [Command("wasagotchi train")]
        public async Task WasagotchiTrain()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            config.LastTrain = now;
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                Random rand = new Random();
                int choice = rand.Next(1, 3);

                if (choice == 1)
                {
                    int much = rand.Next(20, 30);
                    uint attn = Convert.ToUInt32(much);
                    config.XP += attn;
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    int randomIndex = rand.Next(yesTrainTexts.Length);
                    string text = yesTrainTexts[randomIndex];
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Success!",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(0, 255, 0);
                    if (config.pfp == null)
                    embed.WithThumbnailUrl("https://i.imgur.com/6AaY08I.png");
                    else
                        embed.WithThumbnailUrl(config.pfp);
                    embed.WithDescription($"{text} \n**(+{attn} exp)**");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                if (choice == 2)
                {
                    int randomIndex = rand.Next(yesTrainTexts.Length);
                    string text = noTrainTexts[randomIndex];
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Try again!",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(255, 0, 0);
                    if (config.pfp == null)
                        embed.WithThumbnailUrl("https://i.imgur.com/6AaY08I.png");
                    else
                        embed.WithThumbnailUrl(config.pfp);
                    embed.WithDescription($"{text}");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }

            }
        }

        private string GetRooms(ulong value)
        {

            if (value == 10)
            {
                return $"Paradise Resort";
            }
            else if (value == 9)
            {
                return $"Castle";
            }
            else if (value == 8)
            {
                return $"Mansion";
            }
            else if (value == 7)
            {
                return $"Gingerbread House";
            }
            else if (value == 6)
            {
                return $"Villa";
            }
            else if (value == 5)
            {
                return $"Suburban House";
            }
            else if (value == 4)
            {
                return $"Apartment";
            }
            else if (value == 3)
            {
                return $"Cottage";
            }
            else if (value == 2)
            {
                return $"Tipi";
            }
            else if (value == 1)
            {
                return $"Shack";
            }
            else if (value == 0)
            {
                return $"Cave";
            }

            return $"The better the house, the better living conditions :)";
        }
    }
}
