using System;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class WasagotchiInteractive : InteractiveBase
    {
        [Command("wasagotchi buy", RunMode = RunMode.Async), Alias("w shop")]
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

            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a <:wasagotchi:454535808079364106> Wasagotchi? (**900** :potato:)**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current Wasagotchi!**"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Money < config.RoomCost)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Potatos for that! **You require **{900 - configg.Money}** more Potatos!"; });
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
                //await Task.Delay(80000);
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Money < config.RoomCost)
                    {
                        await Context.Channel.SendMessageAsync($"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Potatos for that! **You require **{config.RoomCost - configg.Money}** more Potatos!");
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
                    await shop.ModifyAsync(m => { m.Content = $"<:no:453716729525174273>  |  **{Context.User.Username}**, the command menu has closed due to inactivity."; });
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
                await shop.ModifyAsync(m => { m.Content = $":house:  |  **Your current room is: {GetRooms(config.rLvl)}. Are you sure you want to downgrade to {GetRooms(config.rLvl - 1)}? This will not refund your Potatos!** \n\nType `confirm` to continue or `cancel` to cancel."; });
                //await Task.Delay(80000);
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
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Medicine - cures your Wasagotchi's sickness [{config.Waste * 30} :Potato:]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase."; });
                //await Task.Delay(80000);
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
        [Command("wasagotchi list")]
        public async Task Paginator()
        {
            var pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" };
            await PagedReplyAsync(pages);
        }
    }
}
