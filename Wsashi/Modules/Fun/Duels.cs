using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Wsashi.Preconditions;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules
{
    public class Duel : ModuleBase
    {
        [Command("duel help")]
        [Alias("duels help")]
        [Summary("Shows all possible commands for dueling")]
        [Remarks("Ex: w!duel help")]
        public async Task DuelHelp()
        {
            string[] footers = new string[]
{
                "If you want to gain health but still want to do damage, use w!absorb!",
                "You have a maximum of 6 Med Kits you can use in battle, use them wisely!",
                "If you are low on health, use your Med Kits and heal up!",
                                "If you want to deflect some damage from your opponent's next attack, use w!deflect!",
                                "Each duel command has a cooldown of 3 seconds.",
                                "Absorbing is powerful, but it is rare that it can hit the target."
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle(":crossed_swords:  Duel Command List");
            embed.AddField("w!duels help", "Brings up the help commmand (lol)", true);
            embed.AddField("w!duel", "Starts a duel with the specified user!", true);
            embed.AddField("w!giveup", "Stops the duel and gives up.", true);
            embed.AddField("w!slash", "Slashes your foe with a sword. Good accuracy and medium damage", true);
            embed.AddField("w!heal", "Heals you 14-30 health with one of your Med Kits, but your turn gets consumed.", true);
            embed.AddField("w!block", "Goes into blocking formation, 75% of the damage from the next attack is absorbed, the rest still inflicts damage. You cannot use this if you are already in deflecting formation. Your turn gets consumed.", true);
            embed.AddField("w!absorb", "Absorbs your enemey's health, Does little damage but your health gets partially regenerated. Low accuracy. Your turn gets consumed.", true);
            embed.AddField("w!deflect", "Goes into deflecting formation, 50% of the damage from the next attack is deflected back, the rest still inflicts damage. You cannot use this if you are already in blocking formation.", true);
            embed.WithFooter(text);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("duel")]
        [Alias("Duel", "dual")]
        [Summary("Starts a duel with the specified user!")]
        [Remarks("w!duel <user you want to duel> Ex: w!duel @Phytal")]
        public async Task Pvp(SocketGuildUser user)
        {
            var userr = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(user.Id);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == false && configg.Fighting == false)
            {
                configg.OpponentId = Context.User.Id;
                config.OpponentId = user.Id;
                configg.OpponentName = Context.User.Username;
                config.OpponentName = user.Username;
                config.Fighting = true;
                configg.Fighting = true;


                string[] whoStarts = new string[]
                {
                    Context.User.Mention,
                    user.Mention

                };

                Random rand = new Random();

                int randomIndex = rand.Next(whoStarts.Length);
                string text = whoStarts[randomIndex];

                config.whosTurn = text;
                configg.whosTurn = text;
                if (text == Context.User.Mention)
                {
                    config.whoWaits = user.Mention;
                    configg.whoWaits = user.Mention;
                }
                else
                {
                    config.whoWaits = Context.User.Mention;
                    configg.whoWaits = Context.User.Mention;
                }
                GlobalGuildUserAccounts.SaveAccounts();
                await ReplyAsync($":crossed_swords:  | {Context.User.Mention} challenged {user.Mention} to a duel!\n\n**{configg.OpponentName}** has **{config.Health}** health!\n**{config.OpponentName}** has **{configg.Health}** health!\n\n{text}, you go first!");
            }
            else
            {
                await ReplyAsync(":expressionless:  | " + Context.User.Mention + ", sorry, either you or your opponent are currently fighting or you just tried to fight yourself...");
            }
        }


        [Command("giveup")]
        [Alias("surrender")]
        [Summary("Stops the fight and gives up.")]
        [Remarks("Ex: w!giveup")]
        [Cooldown(10)]
        public async Task GiveUp()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true || configg.Fighting == true)
            {
                await ReplyAsync(":flag_white:  | " + Context.User.Mention + " gave up. The fight stopped.");
                config.Fighting = false;
                configg.Fighting = false;
                config.Health = 100;
                configg.Health = 100;
                config.OpponentId = 0;
                configg.OpponentId = 0;
                config.OpponentName = null;
                configg.OpponentName = null;
                config.whosTurn = null;
                config.whoWaits = null;
                config.placeHolder = null;
                configg.whosTurn = null;
                configg.whoWaits = null;
                configg.placeHolder = null;
                config.Meds = 6;
                configg.Meds = 6;
                config.Blocking = false;
                configg.Blocking = false;
                configg.Deflecting = false;
                config.Deflecting = false;
                GlobalGuildUserAccounts.SaveAccounts();
            }
            else
            {
                await ReplyAsync(":neutral_face:  | There is no fight to stop.");
            }
        }

        [Command("Slash")]
        [Alias("slash")]
        [Summary("Slashes your foe with a sword. Good accuracy and medium damage")]
        [Remarks("Ex: w!slash")]
        public async Task Slash()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true && configg.Fighting == true)
            {
                if (config.whosTurn == Context.User.Mention && configg.whosTurn == Context.User.Mention)
                {
                    Random rand = new Random();

                    int randomIndex = rand.Next(1, 8);
                    if (randomIndex > 1)
                    {
                        Random rand2 = new Random();

                        int randomIndex2 = rand2.Next(7, 15);

                        if (Context.User.Id == configg.OpponentId)
                        {
                            if (configg.Blocking == true)
                            {
                                int randomIndexBlock = (randomIndex2 / 4);
                                int Block = (int)Math.Round((decimal)randomIndexBlock, 0, MidpointRounding.AwayFromZero);
                                configg.Health = configg.Health - Block * 3;
                                if (configg.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    configg.Blocking = false;

                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** still died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            if (configg.Deflecting == true)
                            {
                                int randomIndexDeflect = (randomIndex2 / 2);
                                int Deflect = (int)Math.Round((decimal)randomIndexDeflect, 0, MidpointRounding.AwayFromZero);
                                config.Health = config.Health - Deflect;
                                configg.Health = configg.Health - Deflect;
                                if (configg.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    config.Deflecting = false;

                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage, but {Deflect} damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                if (config.Health <= 0)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because {Deflect} damage was deflected back towards you, you died from your own attack. **{configg.OpponentName}** won!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                if (configg.Health <= 0 && config.Health <= 0)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because **{Deflect}** damage was deflected back towards you, you died from your own attack. But **{configg.OpponentName}** also died from trying to endure the attack. ***Nobody*** won..");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did **{Deflect}** damage, but **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** still died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;
                                    configg.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            else
                            {
                                configg.Health = configg.Health - randomIndex2;
                                if (configg.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                }
                                else
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;
                                    GlobalGuildUserAccounts.SaveAccounts();

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did {randomIndex2} damage!\n\n**{configg.OpponentName}** died. **{config.OpponentName}** won!");
                                }
                            }
                        }
                        else if (Context.User.Id == configg.OpponentId)
                        {
                            if (config.Blocking == true)
                            {
                                int randomIndexBlock = (randomIndex2 / 4);
                                int Block = (int)Math.Round((decimal)randomIndexBlock, 0, MidpointRounding.AwayFromZero);
                                config.Health = config.Health - Block * 3;
                                if (config.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    config.Blocking = false;

                                    GlobalGuildUserAccounts.SaveAccounts();

                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    return;
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** still died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            if (config.Deflecting == true)
                            {
                                int randomIndexDeflect = (randomIndex2 / 2);
                                int Deflect = (int)Math.Round((decimal)randomIndexDeflect, 0, MidpointRounding.AwayFromZero);
                                configg.Health = configg.Health - Deflect;
                                config.Health = config.Health - Deflect;
                                if (config.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    config.Deflecting = false;

                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{Deflect}** damage, but **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                if (configg.Health <= 0)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because **{Deflect}* damage was deflected back towards you, you died from your own attack. **{configg.OpponentName}** won!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                if (configg.Health <= 0 && config.Health <= 0)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because **{Deflect}** damage was deflected back towards you, you died from your own attack. But **{config.OpponentName}** also died from trying to endure the attack. ***Nobody*** won..");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did **{Deflect}** damage, but **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** still died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            config.Health = config.Health - randomIndex2;
                            if (config.Health > 0)
                            {
                                config.placeHolder = config.whosTurn;
                                config.whosTurn = config.whoWaits;
                                config.whoWaits = config.placeHolder;
                                configg.placeHolder = configg.whosTurn;
                                configg.whosTurn = configg.whoWaits;
                                configg.whoWaits = configg.placeHolder;

                                await ReplyAsync($":dagger:  | **{Context.User.Username}**, You hit and did **{randomIndex2}** damage!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                GlobalGuildUserAccounts.SaveAccounts();
                            }
                            else
                            {
                                await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You hit and did {randomIndex2} damage!\n\n**{config.OpponentName}** died. **{configg.OpponentName}** won!");
                                config.Fighting = false;
                                configg.Fighting = false;
                                config.Health = 100;
                                configg.Health = 100;
                                config.OpponentId = 0;
                                configg.OpponentId = 0;
                                config.OpponentName = null;
                                configg.OpponentName = null;
                                config.whosTurn = null;
                                config.whoWaits = null;
                                config.placeHolder = null;
                                configg.whosTurn = null;
                                configg.whoWaits = null;
                                configg.placeHolder = null;
                                config.Meds = 6;
                                configg.Meds = 6;
                                config.Blocking = false;
                                configg.Blocking = false;
                                configg.Deflecting = false;
                                config.Deflecting = false;

                                GlobalGuildUserAccounts.SaveAccounts();
                            }
                        }
                        else
                        {
                            await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type w!giveup");
                        }
                    }
                    else
                    {
                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;
                        GlobalGuildUserAccounts.SaveAccounts();
                        await ReplyAsync($":dash:   | **{Context.User.Username}**, your attack missed!\n**{config.whosTurn}**, Your turn!");
                    }
                }
                else
                {
                    await ReplyAsync($":octagonal_sign:  | **{Context.User.Mention}**, it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }
        [Command("heal")]
        [Alias("med")]
        [Summary("Heals you 14-30 health with one of your Med Kits, but your turn gets consumed.")]
        [Remarks("Ex: w!heal")]
        public async Task Heal()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true || configg.Fighting == true)
            {
                if (config.whosTurn == Context.User.Mention && configg.whosTurn == Context.User.Mention)
                {

                    Random rand2 = new Random();

                    int randomIndex2 = rand2.Next(14, 30);

                    if (Context.User.Id == configg.OpponentId)
                    {
                        if (config.Meds == 0)
                        {
                            await ReplyAsync($"<:redcross:459549117488824320>  | You used all of your Med Kits, **{configg.OpponentName}**! Try Again!");
                        }
                        if (config.Health > 75)
                        {
                            await ReplyAsync($"<:redcross:459549117488824320>  | **{configg.OpponentName}**, You used can't use your Med Kit since the max amount you can heal to is 75 health, **{configg.OpponentName}**, Try Again!");
                        }
                        else
                        {
                            config.Health = config.Health + randomIndex2;
                            config.Meds = config.Meds - 1;

                            config.placeHolder = config.whosTurn;
                            config.whosTurn = config.whoWaits;
                            config.whoWaits = config.placeHolder;
                            configg.placeHolder = configg.whosTurn;
                            configg.whosTurn = configg.whoWaits;
                            configg.whoWaits = configg.placeHolder;

                            if (config.Health > 120) config.Health = 120;
                            await ReplyAsync($"<:redcross:459549117488824320>  | **{configg.OpponentName}**, You used your Med Kit and healed **{randomIndex2}** health!\n\n**{configg.OpponentName}** has **{config.Health}** health and has **{config.Meds}** Med Kits left!\n\n {config.whosTurn}, Your turn!");

                            GlobalGuildUserAccounts.SaveAccounts();
                        }
                    }
                    else if (Context.User.Id == config.OpponentId)
                    {
                        if (configg.Meds == 0)
                        {
                            await ReplyAsync($"<:redcross:459549117488824320>  | You used all of your Med Kits, **{config.OpponentName}**! Try Again!");
                            return;
                        }
                        if (configg.Health > 75)
                        {
                            await ReplyAsync($"<:redcross:459549117488824320>  | **{config.OpponentName}**, You used can't use your Med Kit since the max amount you can heal to is 75 health, **{config.OpponentName}**, Try Again!");
                        }
                        else
                        {
                            config.placeHolder = config.whosTurn;
                            config.whosTurn = config.whoWaits;
                            config.whoWaits = config.placeHolder;
                            configg.placeHolder = configg.whosTurn;
                            configg.whosTurn = configg.whoWaits;
                            configg.whoWaits = configg.placeHolder;

                            configg.Health = configg.Health + randomIndex2;
                            configg.Meds = configg.Meds - 1;

                            if (configg.Health > 120) configg.Health = 120;
                            await ReplyAsync($"<:redcross:459549117488824320>  | **{config.OpponentName}**, You used your Med Kit and healed **{randomIndex2}** health!\n\n**{config.OpponentName}** has **{configg.Health}** health and has **{configg.Meds}** Med Kits left!\n\n{config.whosTurn}, Your turn!");
                            GlobalGuildUserAccounts.SaveAccounts();
                        }
                    }
                    else
                    {
                        await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type w!giveup");
                    }
                }
                else
                {
                    await ReplyAsync($":octagonal_sign:  | **{Context.User.Mention}**, it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }
        [Command("block")]
        [Alias("shield")]
        [Summary("Goes into blocking formation, 75% of the damage from the next attack is absorbed, the rest still inflicts damage. Your turn gets consumed.")]
        [Remarks("Ex: w!block")]
        public async Task Block()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true || configg.Fighting == true)
            {
                if (config.whosTurn == Context.User.Mention && configg.whosTurn == Context.User.Mention)
                {
                    if (Context.User.Id == config.OpponentId)
                    {
                        if (config.Blocking == true)
                        {
                            await ReplyAsync("You are already in blocking formation! Try Again!");
                            return;
                        }
                        if (configg.Deflecting == true)
                        {
                            await ReplyAsync("You cannot block while already in deflecting formation! Try Again!");
                            return;
                        }
                        configg.Blocking = true;

                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;

                        await ReplyAsync($":shield:  | **{config.OpponentName}**, You are now in blocking formation!\n\n**{config.OpponentName}**'s shield will absorb 75% of the damage from the next attack\n\n {configg.whosTurn}, Your turn!");

                        GlobalGuildUserAccounts.SaveAccounts();
                        return;
                    }
                    if (Context.User.Id == configg.OpponentId)
                    {
                        if (config.Blocking == true)
                        {
                            await ReplyAsync("You are already in blocking formation! Try Again!");
                            return;
                        }
                        if (config.Deflecting == true)
                        {
                            await ReplyAsync("You cannot block while already in deflecting formation! Try Again!");
                            return;
                        }
                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;

                        config.Blocking = true;
                        GlobalGuildUserAccounts.SaveAccounts();
                        await ReplyAsync($":shield:  | **{configg.OpponentName}**, You are now in blocking formation! \n\n **{configg.OpponentName}**'s shield will absorb 75% of the damage from the next attack!\n\n{config.whosTurn}, Your turn!");
                        return;
                    }
                    else
                    {
                        await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type w!giveup");
                    }
                }
                else
                {
                    await ReplyAsync($":octagonal_sign:  | **{Context.User.Username}**, it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }

        [Command("deflect")]
        [Alias("shield")]
        [Summary("Goes into deflecting formation, 50% of the damage from the next attack is deflected back toward the enemy, the rest still inflicts damage. Your turn gets consumed.")]
        [Remarks("Ex: w!deflect")]
        public async Task Deflect()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true || configg.Fighting == true)
            {
                if (config.whosTurn == Context.User.Mention && configg.whosTurn == Context.User.Mention)
                {
                    if (Context.User.Id == config.OpponentId)
                    {
                        if (config.Blocking == true)
                        {
                            await ReplyAsync("You cannot deflect while already in blocking formation! Try Again!");
                            return;
                        }
                        if (config.Deflecting == true)
                        {
                            await ReplyAsync("You are already in deflecting formation! Try Again!");
                            return;
                        }
                        configg.Deflecting = true;

                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;

                        await ReplyAsync($":shield:  | **{config.OpponentName}**, You are now in deflecting formation!\n\n**{config.OpponentName}**'s shield will deflect 50% of the damage from the next attack\n\n {config.whosTurn}, Your turn!");

                        GlobalGuildUserAccounts.SaveAccounts();
                        return;
                    }
                    if (Context.User.Id == configg.OpponentId)
                    {
                        if (configg.Blocking == true)
                        {
                            await ReplyAsync("You cannot deflect while already in blocking formation! Try Again!");
                            return;
                        }
                        if (config.Deflecting == true)
                        {
                            await ReplyAsync("You are already in deflecting formation! Try Again!");
                            return;
                        }
                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;

                        config.Deflecting = true;

                        await ReplyAsync($":shield:  | **{configg.OpponentName}**, You are now in deflecting formation! \n\n **{configg.OpponentName}**'s shield will deflect 50% of the damage from the next attack!\n\n{config.whosTurn}, Your turn!");
                        GlobalGuildUserAccounts.SaveAccounts();
                        return;
                    }
                    else
                    {
                        await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type w!giveup");
                    }
                }
                else
                {
                    await ReplyAsync($":octagonal_sign:  | **{Context.User.Username}**, it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }

        [Command("absorb")]
        [Alias("leech")]
        [Summary("Absorbs your enemey's health, Does little damage but your health gets partially regenerated. Low Accuracy. Your turn gets consumed.")]
        [Remarks("Ex: w!block")]
        public async Task Absorb()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalGuildUserAccounts.GetUserID(player2);
            if (config.Fighting == true || configg.Fighting == true)
            {
                if (config.whosTurn == Context.User.Mention && configg.whosTurn == Context.User.Mention)
                {
                    Random rand = new Random();

                    int randomIndex = rand.Next(1, 5);
                    if (randomIndex == 1)
                    {
                        Random rand2 = new Random();

                        int randomIndex2 = rand2.Next(7, 15);

                        if (Context.User.Id == config.OpponentId)
                        {
                            if (config.Blocking == true)
                            {
                                int randomIndexAbsorb = randomIndex2 / 2;
                                int randomIndexBlock = randomIndexAbsorb / 4;
                                int Block = (int)Math.Round((decimal)randomIndexBlock, 0, MidpointRounding.AwayFromZero);
                                int Absorb = (int)Math.Round((decimal)randomIndexAbsorb, 0, MidpointRounding.AwayFromZero);
                                int Absorbed = Absorb * 2;
                                config.Health = config.Health - Block * 3;
                                configg.Health = configg.Health + Absorbed;
                                if (config.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    config.Blocking = false;
                                    if (configg.Health > 120) configg.Health = 120;

                                    await ReplyAsync($":comet:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** still died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            if (config.Deflecting == true)
                            {
                                int randomIndexAbsorb = randomIndex2 / 2;
                                int randomIndexDeflect = (randomIndexAbsorb / 2);
                                int Deflect = (int)Math.Round((decimal)randomIndexDeflect, 0, MidpointRounding.AwayFromZero);
                                int Absorb = (int)Math.Round((decimal)randomIndexAbsorb, 0, MidpointRounding.AwayFromZero);
                                int Absorbed = Absorb * 2;
                                config.Health = config.Health - Deflect;
                                configg.Health = configg.Health - Deflect;

                                if (config.Health > 0 && configg.Health > 0)
                                {
                                    configg.Health = configg.Health + Absorbed;
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    config.Deflecting = false;
                                    if (configg.Health > 120) configg.Health = 120;

                                    await ReplyAsync($":coment:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage, but **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                if (config.Health <= 0)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You did {Absorb} damage, but before you absorbed **{Absorbed}** health {Deflect} damage was deflected back towards you, you died from your own attack. **{configg.OpponentName}** won!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                if (configg.Health < 1 && config.Health < 1)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because {Deflect} damage was deflected back towards you, you died from your own attack. But **{configg.OpponentName}** also died from trying to endure the attack. ***Nobody*** won..");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You did {Absorb} damage, but before you absorbed **{Absorbed}** health {Deflect} damage was deflected back!\n\n**{configg.OpponentName}** still died though. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            else
                            {
                                int randomIndexAbsorb = randomIndex2 / 2;
                                int Absorb = (int)Math.Round((decimal)randomIndexAbsorb, 0, MidpointRounding.AwayFromZero);
                                int Absorbed = Absorb * 2;
                                config.Health = config.Health - Absorb;
                                configg.Health = configg.Health + Absorbed;
                                if (config.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;

                                    if (configg.Health > 120) configg.Health = 120;

                                    await ReplyAsync($":comet:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage!\n\n**{configg.OpponentName}** died. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                            }

                        }
                        else if (Context.User.Id == configg.OpponentId)
                        {
                            if (configg.Blocking == true)
                            {
                                int randomIndexAbsorbb = randomIndex2 / 2;
                                int randomIndexBlock = randomIndexAbsorbb / 4;
                                int Block = (int)Math.Round((decimal)randomIndexBlock, 0, MidpointRounding.AwayFromZero);
                                int Absorbb = (int)Math.Round((decimal)randomIndexAbsorbb, 0, MidpointRounding.AwayFromZero);
                                int Absorbedd = Absorbb * 2;
                                configg.Health = configg.Health - Block;
                                config.Health = config.Health + Absorbedd;
                                if (configg.Health > 0)
                                {
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    configg.Blocking = false;

                                    if (configg.Health > 120) configg.Health = 120;

                                    await ReplyAsync($":comet:  | **{Context.User.Username}**, You absorbed **{Absorbedd}** health and did **{Absorbb}** damage, but **{Block * 3}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You absorbed **{Absorbedd}** health and did **{Absorbb}** damage, but **{Block * 3}** damage was blocked!\n\n**{config.OpponentName}** still died. **{configg.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;
                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            if (configg.Deflecting == true)
                            {
                                int randomIndexAbsorbb = randomIndex2 / 2;
                                int randomIndexDeflect = (randomIndexAbsorbb / 2);
                                int Deflect = (int)Math.Round((decimal)randomIndexDeflect, 0, MidpointRounding.AwayFromZero);
                                int Absorbb = (int)Math.Round((decimal)randomIndexAbsorbb, 0, MidpointRounding.AwayFromZero);
                                int Absorbedd = Absorbb * 2;
                                config.Health = config.Health - Deflect;
                                configg.Health = configg.Health - Deflect;

                                if (configg.Health > 0 && config.Health > 0)
                                {
                                    config.Health = config.Health + Absorbedd;
                                    config.placeHolder = config.whosTurn;
                                    config.whosTurn = config.whoWaits;
                                    config.whoWaits = config.placeHolder;
                                    configg.placeHolder = configg.whosTurn;
                                    configg.whosTurn = configg.whoWaits;
                                    configg.whoWaits = configg.placeHolder;
                                    configg.Deflecting = false;
                                    if (config.Health > 120) config.Health = 120;

                                    await ReplyAsync($":dagger:  | **{Context.User.Username}**, You absorbed **{Absorbedd}** health and did **{Absorbb}** damage, but **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                if (config.Health < 1)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You did **{Absorbb}** damage, but before you absorbed **{Absorbedd}** health **{Deflect}** damage was deflected back towards you, you died from your own attack. **{configg.OpponentName}** won!");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                if (configg.Health < 1 && config.Health < 1)
                                {
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, Because **{Deflect}** damage was deflected back towards you, you died from your own attack. But **{config.OpponentName}** also died from trying to endure the attack. ***Nobody*** won..");
                                    GlobalGuildUserAccounts.SaveAccounts();
                                    return;
                                }
                                else
                                {
                                    await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You did **{Absorbb}** damage, but before you absorbed **{Absorbedd}** health **{Deflect}** damage was deflected back!\n\n**{configg.OpponentName}** still died though. **{config.OpponentName}** won!");
                                    config.Fighting = false;
                                    configg.Fighting = false;
                                    config.Health = 100;
                                    configg.Health = 100;
                                    config.OpponentId = 0;
                                    configg.OpponentId = 0;
                                    config.OpponentName = null;
                                    configg.OpponentName = null;
                                    config.whosTurn = null;
                                    config.whoWaits = null;
                                    config.placeHolder = null;
                                    configg.whosTurn = null;
                                    configg.whoWaits = null;
                                    configg.placeHolder = null;
                                    config.Meds = 6;
                                    configg.Meds = 6;
                                    config.Blocking = false;
                                    configg.Blocking = false;
                                    configg.Deflecting = false;
                                    config.Deflecting = false;

                                    GlobalGuildUserAccounts.SaveAccounts();
                                }
                                return;
                            }
                            int randomIndexAbsorb = randomIndex2 / 2;
                            int Absorb = (int)Math.Round((decimal)randomIndexAbsorb, 0, MidpointRounding.AwayFromZero);
                            int Absorbed = Absorb * 2;
                            configg.Health = configg.Health - Absorb;
                            config.Health = config.Health + Absorbed;
                            if (configg.Health > 0)
                            {
                                config.placeHolder = config.whosTurn;
                                config.whosTurn = config.whoWaits;
                                config.whoWaits = config.placeHolder;
                                configg.placeHolder = configg.whosTurn;
                                configg.whosTurn = configg.whoWaits;
                                configg.whoWaits = configg.placeHolder;

                                if (config.Health > 120) config.Health = 120;

                                await ReplyAsync($":comet:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!\n\n**{config.whosTurn}**, Your turn!");
                                GlobalGuildUserAccounts.SaveAccounts();
                            }
                            else
                            {
                                await ReplyAsync($":skull_crossbones:  | **{Context.User.Username}**, You absorbed **{Absorbed}** health and did **{Absorb}** damage!\n\n**{config.OpponentName}** died. **{configg.OpponentName}** won!");
                                config.Fighting = false;
                                configg.Fighting = false;
                                config.Health = 100;
                                configg.Health = 100;
                                config.OpponentId = 0;
                                configg.OpponentId = 0;
                                config.OpponentName = null;
                                configg.OpponentName = null;
                                config.whosTurn = null;
                                config.whoWaits = null;
                                config.placeHolder = null;
                                configg.whosTurn = null;
                                configg.whoWaits = null;
                                configg.placeHolder = null;
                                config.Meds = 6;
                                configg.Meds = 6;
                                config.Blocking = false;
                                configg.Blocking = false;
                                configg.Deflecting = false;
                                config.Deflecting = false;
                                GlobalGuildUserAccounts.SaveAccounts();
                            }
                        }
                        else
                        {
                            await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type w!giveup");
                        }
                    }
                    else
                    {
                        config.placeHolder = config.whosTurn;
                        config.whosTurn = config.whoWaits;
                        config.whoWaits = config.placeHolder;
                        configg.placeHolder = configg.whosTurn;
                        configg.whosTurn = configg.whoWaits;
                        configg.whoWaits = configg.placeHolder;
                        GlobalGuildUserAccounts.SaveAccounts();
                        await ReplyAsync($":dash:   | **{Context.User.Username}**, your attack missed!\n**{config.whosTurn}**, Your turn!");
                    }
                }
                else
                {
                    await ReplyAsync($":octagonal_sign:  | **{Context.User.Mention}**, it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }
        }
    }
}

