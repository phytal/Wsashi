using System;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Preconditions;

namespace Wsashi.Modules
{
    public class WasagotchiInteractive : InteractiveBase
    {
        [Command("wasagotchi buy"), Alias("w shop", "w buy")]
        [Summary("Opens the Wasagotchi shop menu!")]
        [Remarks("Ex: w!w shop")]
        public async Task WasagotchiBuy()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);
            string shoptext = ":department_store:  **|  Wasagotchi Shop** \n ```xl\nPlease select the purchase you would like to make.\n\n[1] Capsules\n[2] Room Upgrades\n[3] Room Downgrade\n[4] Boosts + Items\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
            var shop = await Context.Channel.SendMessageAsync(shoptext);
            var response = await NextMessageAsync();
            if (response == null)
            {
                await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                return;
            }
                if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a <:wasagotchi:454535808079364106> Wasagotchi? (**900** :potato:)**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current Wasagotchi!**"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Money < config.RoomCost)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Potatoes for that! **You require **{900 - configg.Money}** more Potatoes!"; });
                        return;
                    }
                    config.Have = true;
                    configg.Money -= config.RoomCost;
                    config.BoughtSince = DateTime.UtcNow;
                    //config.Owner = Context.User.Username;
                    GlobalWasagotchiUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought a <:wasagotchi:454535808079364106> Wasagotchi!");
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                if (response == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                return;
            }

            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":house:  |  Your current room is: **{GetRooms(config.rLvl)}**. Are you sure you want to upgrade to **{GetRooms(config.rLvl + 1)}**? (**{config.RoomCost}** :potato:) \n\nType `confirm` to continue or `cancel` to cancel."; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Money < config.RoomCost)
                    {
                        await Context.Channel.SendMessageAsync($"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Potatoes for that! **You require **{config.RoomCost - configg.Money}** more Potatoes!");
                        return;
                    }
                    else
                    {
                        configg.Money -= config.RoomCost;
                        config.rLvl += 1;
                        GlobalWasagotchiUserAccounts.SaveAccounts(user.Id);
                        await Context.Channel.SendMessageAsync($":house:  |  **{Context.User.Username}**, your room has been upgraded to **{GetRooms(config.rLvl)}**");
                        return;
                    }
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                if (response == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }

            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                if (config.RoomCost != 0)
                {
                    await shop.ModifyAsync(m => { m.Content = $":house:  |  **Your current room is: {GetRooms(config.rLvl)}. Are you sure you want to downgrade to {GetRooms(config.rLvl - 1)}? This will not refund your Potatoes!** \n\nType `confirm` to continue or `cancel` to cancel."; });
                    var newresponse = await NextMessageAsync();
                    if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        config.rLvl -= 1;
                        GlobalWasagotchiUserAccounts.SaveAccounts(user.Id);
                        await shop.ModifyAsync(m => { m.Content = $":house:  |  **{Context.User.Username}**, your room has been downgraded to **{GetRooms(config.rLvl)}**"; });
                        return;
                    }
                    if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
                    }
                    if (response == null)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    else
                    {
                        await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                        return;
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync(":octagonal_sign:  | You cannot downgrade your room any further, as you have the loewst possible room");
                }
                
            }
            if (response.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Medicine - cures your Wasagotchi's sickness [{config.Waste * 30} :potato:]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    config.Sick = false;
                    config.Waste = 0;
                    GlobalWasagotchiUserAccounts.SaveAccounts(user.Id);
                    await shop.ModifyAsync(m => { m.Content = $":pill:  |  **{Context.User.Username}**, your <:wasagotchi:454535808079364106> Wasagotchi has been cured of it's sickness!"; });
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                if (response == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            else
            {
                await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                return;
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

        public static Tuple<string, string> GetWasagotchiRarity(int value)
        {
            string wasagotchi = GetWasagotchi(value);
            string wasagotchiRarity = wasagotchi;
            switch (wasagotchi)
            {
                case "Neko-chan ~~":
                case "Hedgie the Hedgehog":
                    wasagotchiRarity = wasagotchi + " [LEGENDARY]";
                    break;
                case "Golden Eagle":
                case "Gothic Ragdoll":
                case "Ugandan Knuckles":
                case "Big Chungus":
                case "Bidoof":
                case "Berd":
                case "That One Anime Body Pillow":
                    wasagotchiRarity = wasagotchi + " [EPIC]";
                    break;
                case "Rainbow-tailed Parrot":
                case "White-shelled Tortise":
                case "Red Panda":
                case "Blue-eyed Canadian Cat":
                case "Winged Hussar":
                case "Blue Pegasus":
                    wasagotchiRarity = wasagotchi + " [RARE]";
                    break;
                case "Siberian Husky":
                case "Milk Snake":
                case "Albino Ferret":
                case "White Tabby Cat":
                case "Golden Retriever":
                case "Beagle":
                    wasagotchiRarity = wasagotchi + " [COMMON]";
                    break;
            }
            return new Tuple<string, string> (wasagotchi, wasagotchiRarity);
        }
            public static string GetWasagotchi(int value) //1-64
        {
            if (value == 64)
            {
                return $"Neko-chan ~~";
            }
            else if (value == 63)
            {
                return $"Hedgie the Hedgehog";
            }
            else if (60 < value && value <= 62)
            {
                return $"Golden Eagle";
            }
            else if (58 < value && value <= 60)
            {
                return $"Gothic Ragdoll";
            }
            else if (56 < value && value <= 58)
            {
                return $"Ugandan Knuckles";
            }
            else if (54 < value && value <= 56)
            {
                return $"Big Chungus";
            }
            else if (52 < value && value <= 54)
            {
                return $"That One Anime Body Pillow";
            }
            else if (50 < value && value <= 52)
            {
                return $"Bidoof";
            }
            else if (48 < value && value <= 50)
            {
                return $"Berd";
            }
            else if (45 < value && value <= 48)
            {
                return $"Rainbow-tailed Parrot";
            }
            else if (42 < value && value <= 45)
            {
                return $"White-shelled Tortise";
            }
            else if (39 < value && value <= 42)
            {
                return $"Red Panda";
            }
            else if (36 < value && value <= 39)
            {
                return $"Blue-eyed Canadian Cat";
            }
            else if (33 < value && value <= 36)
            {
                return $"Winged Hussar";
            }
            else if (30 < value && value <= 33)
            {
                return $"Blue Pegasus";
            }
            else if (25 < value && value <= 30)
            {
                return $"Siberian Husky";
            }
            else if (20 < value && value <= 25)
            {
                return $"Milk Snake";
            }
            else if (15 <value && value <= 20)
            {
                return $"Albino Ferret";
            }
            else if (10 < value && value <= 15)
            {
                return $"White Tabby Cat";
            }
            else if (5 < value && value <= 10)
            {
                return $"Golden Retriever";
            }
            else if (value <= 5)
            {
                return $"Beagle";
            }
            return string.Empty;
        }

        [Command("wasagotchi opencapsule"), Alias("w opencapsule")]
        [Summary("Opens one of the Wasagotchi capsules you have! Note: You must have the capsule you want to open.")]
        [Remarks("w!opencapsule <rarity (common, rare, epic, legendary)> Ex: w!opencapsule common")]
        [Cooldown(5)]
        public async Task OpenCapsule(string arg)
        {
            var config = GlobalWasagotchiUserAccounts.GetWasagotchiAccount(Context.User);

            if (config.Have == false) //if they own a Wasagotchi or not
            {
                int value = 0;
                if (arg == "common")
                {
                    if (config.CommonCapsule > 0)
                    {
                        config.CommonCapsule -= 1;
                        GlobalUserAccounts.SaveAccounts();
                        value = Global.Rng.Next(1, 64);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Common Wasagotchi Capsules!");
                        return;
                    }
                }
                if (arg == "rare")
                {
                    if (config.EpicCapsule > 0)
                    {
                        config.RareCapsule -= 1;
                        GlobalUserAccounts.SaveAccounts();
                        value = Global.Rng.Next(30, 64);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Rare Wasagotchi Capsules!");
                        return;
                    }
                }
                if (arg == "epic")
                {
                    if (config.EpicCapsule > 0)
                    {
                        config.EpicCapsule -= 1;
                        GlobalUserAccounts.SaveAccounts();
                        value = Global.Rng.Next(48, 64);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Epic Wasagotchi Capsules!");
                        return;
                    }
                }
                if (arg == "legendary")
                {
                    if (config.LegendaryCapsule > 0)
                    {
                        config.LegendaryCapsule -= 1;
                        GlobalUserAccounts.SaveAccounts();
                        value = Global.Rng.Next(63, 64);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Legendary Wasagotchi Capsules!");
                        return;
                    }
                }
                Tuple<string, string> wasagotchi = GetWasagotchiRarity(value);
                config.Breed = wasagotchi.Item1;
                GlobalUserAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync($":pill:  |  **{Context.User.Username}**, you got a **{wasagotchi.Item2}**! Would you like to name it? `yes/no` (If not, the name will be left empty)");

                var responsee = await NextMessageAsync();

                if (responsee.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && (responsee.Author.Equals(Context.User)))
                {
                    await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, what would you like to name it? (type the name you want to give it!)");
                    var responseee = await NextMessageAsync();
                    if (responseee.Content.Length > 1 && (responseee.Author.Equals(Context.User)))
                    {
                        await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, successfully named your **{config.Breed}** to **{responseee}**! You can use the command `w!wasagotchi help` for more Wasagotchi commands!");
                    }
                }
                if (responsee.Content.Equals("no", StringComparison.CurrentCultureIgnoreCase) && (responsee.Author.Equals(Context.User)))
                {
                    await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, your Wasagotchi is unnamed! You can change the name of it by using the command `w!wasagotchi name`");
                    return;
                }
            }
            else //starts the capsule opening process
            {
                await Context.Channel.SendMessageAsync($":warning:  |  **{Context.User.Username}**, you already own a <:wasagotchi:454535808079364106> Wasagotchi! Would you like to replace it? `yes/no` (If not, the capsule won't be opened)");

                var response = await NextMessageAsync();

                if (response.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    int value = 0;
                    if (arg == "common")
                    {
                        if (config.CommonCapsule > 0)
                        {
                            config.CommonCapsule -= 1;
                            GlobalUserAccounts.SaveAccounts();
                            value = Global.Rng.Next(1, 64);
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Common Wasagotchi Capsules!");
                            return;
                        }
                    }
                    if (arg == "rare")
                    {
                        if (config.RareCapsule > 0)
                        {
                            config.RareCapsule -= 1;
                            GlobalUserAccounts.SaveAccounts();
                            value = Global.Rng.Next(30, 64);
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Rare Wasagotchi Capsules!");
                            return;
                        }
                    }
                    if (arg == "epic")
                    {
                        if (config.EpicCapsule > 0)
                        {
                            config.EpicCapsule -= 1;
                            GlobalUserAccounts.SaveAccounts();
                            value = Global.Rng.Next(48, 64);
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Epic Wasagotchi Capsules!");
                            return;
                        }
                    }
                    if (arg == "legendary")
                    {
                        if (config.LegendaryCapsule > 0)
                        {
                            config.LegendaryCapsule -= 1;
                            GlobalUserAccounts.SaveAccounts();
                            value = Global.Rng.Next(63, 64);
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Legendary Wasagotchi Capsules!");
                            return;
                        }
                    }
                    Tuple<string, string> wasagotchi = GetWasagotchiRarity(value);
                    config.Breed = wasagotchi.Item1;
                    
                    await Context.Channel.SendMessageAsync($":pill:  |  **{Context.User.Username}**, you got a {wasagotchi.Item2}! Would you like to name it? `yes/no` (If not, the name will be left empty)");

                    var responsee = await NextMessageAsync();

                    if (responsee.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, what would you like to name it? (type the name you want to give it!)");
                        var responseee = await NextMessageAsync();
                        if (responseee.Content.Length > 1 && (response.Author.Equals(Context.User)))
                        {
                            string name = responseee.Content;
                            config.Name = name;
                            GlobalWasagotchiUserAccounts.SaveAccounts();
                            await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, successfully named your {config.Breed} to {name}! You can use the command `w!wasagotchi help` for more Wasagotchi commands!");
                        }
                    }
                    if (responsee.Content.Equals("no", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await Context.Channel.SendMessageAsync($"<:wasagotchi:454535808079364106>  |  **{Context.User.Username}**, your Wasagotchi is unnamed! You can change the name of it by using the command `w!wasagotchi name`");
                        return;
                    }
                }

                if (response.Content.Equals("no", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await Context.Channel.SendMessageAsync($":feet:   |  Capsule opening canceled.");
                    return;
                }
            }
        }

        [Command("test list")]
        public async Task Paginator()
        {
            PaginatedMessage pages = new PaginatedMessage { Pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" }, Content = "gay", Color = Color.Blue, Title = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" } };
            await PagedReplyAsync(pages);
        }
    }
}
