using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Wasagotchi
{
    public class General : WsashiModule
    {
        [Command("wasagotchi inventory"), Alias("w inventory")]
        [Summary("View your inventory for Wasagotchi")]
        [Remarks("Usage: w!inventory")]
        public async Task WasagotchiInventory()
        {
            var account = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username}'s Wasagotchi Inventory");

            embed.AddField("Common Capsules", $"**x{account.CommonCapsule}**");
            embed.AddField("Rare Capsules", $"**x{account.RareCapsule}**");
            embed.AddField("Epic Capsules", $"**x{account.EpicCapsule}**");
            embed.AddField("Legendary Capsules", $"**x{account.LegendaryCapsule}**");
            embed.WithFooter("You can get Wasagotchi Capsules from opening loot boxes!");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("wasagotchi stats"), Alias("w stats")]
        [Summary("Brings up the stats/info of your or someone else's Wasagotchi!")]
        [Remarks("w!w stats <specified user (will be yours if left empty)> Ex: w!w stats @Phytal")]
        public async Task WasagotchiUser([Remainder]string arg = "")
        {
            SocketUser user = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            user = mentionedUser ?? Context.User;
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastStats.AddSeconds(8) - now);
            if (now < config.LastStats.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastStats = now;
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
                    Name = $"{user.Username}'s Wasagotchi",
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
                embed.AddField("Owner", user, true);
                if (config.Name == null)
                    embed.AddField("Name", "*(Name your wasagotchi!)*", true);
                else
                    embed.AddField("Name", config.Name, true);
                if (config.Breed == null)
                    embed.AddField("Breed", "Breedless (You can get a wasagotchi with a breed from a wasagotchi capsule)", true);
                else
                embed.AddField("Breed", config.Breed, true);
                embed.AddField("Exp", config.XP, true);
                embed.AddField("Level", config.LevelNumber, true);
                embed.AddField("Room",  GetRooms(config.rLvl), true);
                embed.AddField("Waste", config.Waste, true);
                embed.AddField("Attention", config.Attention, true);
                embed.AddField("Hunger", config.Hunger, true);
                embed.AddField("Sick", config.Sick, true);
                embed.AddField("Ran Away", config.RanAway, true);
                if (config.pfp == null)
                    embed.AddField("Picture", "*Default*", true);
                else
                embed.AddField("Picture", "*Custom*", true);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        [Command("wasagotchi help"), Alias("w help")]
        [Summary("Displays all Wasagotchi commands with a description of what they do")]
        [Remarks("Ex: w!help")]
        public async Task WasagotchiHelp()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastHelp.AddSeconds(8) - now);
            if (now < config.LastHelp.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            config.LastHelp = now;
            string[] footers = new string[]
{
                "Every 4 hours all Wasagotchis will have a time modifier, -1 hunger, -1 attention, and +1 waste. Make sure to check on your Wasagotchi often!",
                "If the living conditions you provide for your Wasagotchi are too low - never clean, never play, etc - it will run away! (Your room will remain the same)",
                "If your Wasagotchi is sick, buy it some medicine with w!buy.",
                "All Wasagotchi commands have a 8 second cooldown.",
                "To get a direct link to a picture right click and open image in new tab. Then there's the URL! :)"
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle("<:wasagotchi:454535808079364106> Wasagotchi Command List");
            embed.AddField("w!wasagotchi help", "Brings up the help commmand (lol)", true);
            embed.AddField("w!wasagotchi shop", "Opens the Wasagotchi shop menu!", true);
            embed.AddField("w!wasagotchi stats", "Brings up the stats/info of your or someone else's Wasagotchi!", true);
            embed.AddField("w!wasagotchi name", "Set the name of your Wasagotchi!", true);
            embed.AddField("w!wasagotchi picture", "Set the picture of your Wasagotchi! (Note: It must be a direct link)", true);
            embed.AddField("w!wasagotchi feed", "Feeds your Wasagtochi at the cost of Potatoes! Otherwise it will starve!", true);
            embed.AddField("w!wasagotchi clean", "Clean up your Wasagotchi's waste, Otherwise it'll get sick!", true);
            embed.AddField("w!wasagotchi play", "Play with your wasagotchi! Your Wasagotchi must have high attention levels at all times!", true);
            embed.AddField("w!wasagotchi train", "Train your Wasagotchi to earn Exp and level up!", true);
            embed.WithFooter(text);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("wasagotchi name"), Alias("w name")]
        [Summary("Set the name of your Wasagotchi!")]
        [Remarks("w!w name <your desired name> Ex: w!w name Potato")]
        public async Task WasagotchiName([Remainder] string name)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastName.AddSeconds(8) - now);
            if (now < config.LastName.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastName = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                config.Name = name;
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your <:wasagotchi:454535808079364106> Wasagotchi's name to **{name}**!");
            }
        }

        [Command("wasagotchi picture"), Alias("w picture", "w pic")]
        [Summary("Set the picture of your Wasagotchi! (Note: It must be a direct link)")]
        [Remarks("w!w pic <direct URL to the picture> Ex: w!w pic https://cdn.shopify.com/s/files/1/1017/2183/t/2/assets/live-preview-potato-standing.png?4854792436625201403")]
        public async Task WasagotchiPfp([Remainder] string name)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastPicture.AddSeconds(8) - now);
            if (now < config.LastPicture.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastPicture = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                config.pfp = name;
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your <:wasagotchi:454535808079364106> Wasagotchi's picture to **{name}**!");
            }
        }


        [Command("wasagotchi feed"), Alias("w feed")]
        [Summary("Feeds your Wasagtochi at the cost of Potatoes! Otherwise it will starve!")]
        [Remarks("Ex: w!w feed")]
        public async Task WasagotchiFeed()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastFeed.AddSeconds(8) - now);
            if (now < config.LastFeed.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastFeed = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
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

        [Command("wasagotchi clean"), Alias("w clean")]
        [Summary("Clean up your Wasagotchi's waste, Otherwise it'll get sick!")]
        [Remarks("Ex: w!w clean")]
        public async Task WasagotchiClean()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User.Id);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastClean.AddSeconds(8) - now);
            if (now < config.LastClean.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastClean = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
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

        [Command("wasagotchi play"), Alias("w play")]
        [Summary("Play with your wasagotchi! Your Wasagotchi must have high attention levels at all times!")]
        [Remarks("Ex: w!w play")]
        public async Task WasagotchiPlay()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastPlay.AddSeconds(8) - now);
            if (now < config.LastPlay.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            config.LastPlay = now;
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
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

        [Command("wasagotchi train"), Alias("w train")]
        [Summary("Train your Wasagotchi to earn Exp and level up!")]
        [Remarks("Ex: w!w train")]
        public async Task WasagotchiTrain()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            DateTime now = DateTime.UtcNow;
            var timeSpanString = string.Format("{0:%s} seconds", config.LastTrain.AddSeconds(8) - now);
            if (now < config.LastTrain.AddSeconds(8))
            {
                await Context.Channel.SendMessageAsync($"**{Context.User.Username}, please cooldown! You may use this command in {timeSpanString}.**");
                return;
            }
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a <:wasagotchi:454535808079364106> Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                config.LastTrain = now;
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
