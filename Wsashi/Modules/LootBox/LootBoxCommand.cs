using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.LootBox
{
    public class LootBoxCommand : WsashiModule
    {
        [Command("openLootBox")]
        [Alias("olb")]
        [Summary("Opens one of the loot boxes you have")]
        [Remarks("Usage: w!openLootBox <rarity (common/uncommon/rare/epic/legendary) Ex: w!openLootBox common")]
        public async Task OpenLootBoxCommand([Remainder] string arg)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            if (arg == "common")
            {
                if (config.LootBoxCommon > 0)
                {
                    config.LootBoxCommon -= 1;
                    await OpenLootBox.OpenCommonBox(Context.User, (ITextChannel)Context.Channel);
                }
                else
                {
                    await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Common Loot Boxes!");
                    return;
                }
            }
            if (arg == "uncommon")
            {
                if (config.LootBoxCommon > 0)
                {
                    config.LootBoxUncommon -= 1;
                    await OpenLootBox.OpenUncommonBox(Context.User, (ITextChannel)Context.Channel);
                }
                else
                {
                    await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Uncommon Loot Boxes!");
                    return;
                }
            }
            if (arg == "rare")
            {
                if (config.LootBoxRare > 0)
                {
                    config.LootBoxRare -= 1;
                    await OpenLootBox.OpenRareBox(Context.User, (ITextChannel)Context.Channel);
                }
                else
                {
                    await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Rare Loot Boxes!");
                    return;
                }
            }
            if (arg == "epic")
            {
                if (config.LootBoxEpic > 0)
                {
                    config.LootBoxEpic -= 1;
                    await OpenLootBox.OpenEpicBox(Context.User, (ITextChannel)Context.Channel);
                }
                else
                {
                    await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Epic Loot Boxes!");
                    return;
                }
            }
            if (arg == "legendary")
            {
                if (config.LootBoxLegendary > 0)
                {
                    config.LootBoxLegendary -= 1;
                    await OpenLootBox.OpenLegendaryBox(Context.User, (ITextChannel)Context.Channel);
                }
                else
                {
                    await Context.Channel.SendMessageAsync($":octagonal_sign:  |  **{Context.User.Username}**, you don't have any Legendary Loot Boxes!");
                    return;
                }
            }
            return;
        }

        [Command("lootBoxInventory"), Alias("lbi")]
        [Summary("View your loot box inventory")]
        [Remarks("Usage: w!inventory")]
        public async Task LootBoxInventory()
        {
            var account = GlobalUserAccounts.GetUserAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username}'s Loot Box Inventory");

            embed.AddField("Common Loot Boxes", $"**x{account.LootBoxCommon}**");
            embed.AddField("Uncommon Loot Boxes", $"**x{account.LootBoxUncommon}**");
            embed.AddField("Rare loot Boxes", $"**x{account.LootBoxRare}**");
            embed.AddField("Epic Loot Boxes", $"**x{account.LootBoxEpic}**");
            embed.AddField("Legendary Loot Boxes", $"**x{account.LootBoxLegendary}**");
            embed.WithFooter("You can get Loot Boxes from increasing your Wsashi Level (not server level) and winning duels!");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("addLootBox"), Alias("alb")]
        [Summary("Adds some loot boxes to a person")]
        [Remarks("Usage: w!alb <user>")]
        [RequireOwner]
        public async Task AddLootBox([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = GlobalUserAccounts.GetUserAccount(target);
            account.LootBoxCommon += 1;
            account.LootBoxUncommon += 1;
            account.LootBoxRare += 1;
            account.LootBoxEpic += 1;
            account.LootBoxLegendary += 1;
            GlobalUserAccounts.SaveAccounts();

            await Context.Channel.SendMessageAsync($"Successfully added one of every loot box to {target}");
        }
    }
}
