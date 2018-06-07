using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Wasagotchi
{
    public class General : WsashiModule
    {
        [Command("wasagotchi stats")]
        public async Task WasagotchiUser()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
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
                embed.WithThumbnailUrl("https://i.imgur.com/hXbmIOu.png");
                embed.WithColor(37, 152, 255);
                embed.AddInlineField("Name", config.Name);
                embed.AddInlineField("Exp", config.XP);
                embed.AddInlineField("Level", config.LevelNumber);
                embed.AddInlineField("Room",  GetRooms(config.rLvl));
                embed.AddInlineField("Waste", config.Waste);
                embed.AddInlineField("Attention", config.Attention);
                embed.AddInlineField("Hunger", config.Hunger);

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("wasagotchi name")]
        public async Task WasagotchiName([Remainder] string name)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                config.Name = name;
                GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your Wasagotchi's name to **{name}**!");
            }
        }

        [Command("wasagotchi feed")]
        public async Task WasagotchiFeed()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Hunger == 20)
                {
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, your Wasagotchi is full!");
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
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, you fill your Wasagotchi's bowl with food. It looks happy! **(+{hungerr} food [-{cost} :potato:])**");
                }
            }
        }

        string[] cleanTexts = new string[]
{
                "you hold your nose and start cleaning up the mess. ",
                "you clean up your Wasagotchi's...business!",
};

        [Command("wasagotchi clean")]
        public async Task WasagotchiClean()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Waste == 0)
                {
                    await Context.Channel.SendMessageAsync($":sparkles:  | **{Context.User.Username}, your Wasagotchi's room is squeaky clean!**");
                    return;
                }
                {
                    Random rand = new Random();
                    int much = rand.Next(4, 8);
                    uint clean = Convert.ToUInt32(much);
                    config.Waste -= clean;
                    if (config.Waste < 20)
                    {
                        config.Waste = 0;
                    }
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":sparkles:  |  **{Context.User.Mention}**, {cleanTexts} **(-{clean} waste)**");
                }
            }
        }
        string[] playTexts = new string[]
    {
                "you entertain your Wasagotchi. It seems to like you!",
                "you throw a ball and your Wasagotchi fetches it!",
    };

        [Command("wasagotchi play")]
        public async Task WasagotchiPlay()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
                return;
            }
            else
            {
                if (config.Attention == 20)
                {
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}, your Wasagotchi is bored of playing right now!**");
                    return;
                }
                {
                    Random rand = new Random();
                    int much = rand.Next(4, 8);
                    uint attn = Convert.ToUInt32(much);
                    config.Attention += attn;
                    if (config.Attention > 20)
                    {
                        config.Attention = 0;
                    }
                    GlobalWasagotchiUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}, {playTexts} **(+{attn} attention)**");
                }
            }
        }

        string[] yesTrainTexts = new string[]
{
                "Somehow, you managed to get your Wasagotchi to listen! It is making progress.",
                "Your Wasagotchi seems to respond well to the training! It looks happy.",
};

        string[] noTrainTexts = new string[]
{
                "Despite your best attempts, your Wasagotchi does not want to learn. Persistence is key!",
                "You wonder why your Wasagotchi won't listen. Maybe you should give it a few more tries.",
                "Your Wasagotchi sits down and looks excited, but doesn't do what you wanted.",
                "Your Wasagotchi doesn't quite understand what you are trying to do. Try again!",
};

        [Command("wasagotchi train")]
        public async Task WasagotchiTrain()
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);
            if (config.Have == false) //if they own a Wasagotchi or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a Wasagotchi! \n\nPurchase one with w!wasagotchi buy!");
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
                    embed.WithThumbnailUrl("https://i.imgur.com/hXbmIOu.png");
                    embed.WithDescription($"{text} \n**(+{attn} exp)**");
                    await Context.Channel.SendMessageAsync("", false, embed);
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
                    embed.WithThumbnailUrl("https://i.imgur.com/hXbmIOu.png");
                    embed.WithDescription($"{text}");
                    await Context.Channel.SendMessageAsync("", false, embed);
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
