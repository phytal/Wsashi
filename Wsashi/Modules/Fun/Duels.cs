using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wsashi.Preconditions;

namespace Wsashi.Modules
{
    public class Duel : ModuleBase
    {
        static string SwitchCaseString = "nofight";

        static string player1;
        static string player2;

        static string whosTurn;
        static string whoWaits;
        static string placeHolder;

        static int health1 = 100;
        static int health2 = 100;

        [Command("duel")]
        [Alias("Duel", "dual")]
        [Summary("Starts a duel with the mentioned user")]
        [Remarks("w!duel <user you want to duel> Ex: w!duel @Phytal")]
        [Cooldown(20, true)]
        public async Task Pvp(IUser user)
        {

            if (Context.User.Mention != user.Mention && SwitchCaseString == "nofight")
            {
                SwitchCaseString = "fight_p1";
                player1 = Context.User.Mention;
                player2 = user.Mention;

                string[] whoStarts = new string[]
                {
                    Context.User.Mention,
                    user.Mention

                };

                Random rand = new Random();

                int randomIndex = rand.Next(whoStarts.Length);
                string text = whoStarts[randomIndex];

                whosTurn = text;
                if (text == Context.User.Mention)
                {
                    whoWaits = user.Mention;
                }
                else
                {
                    whoWaits = Context.User.Mention;
                }

                await ReplyAsync(":crossed_swords:  | " + Context.User.Mention + " challenged " + user.Mention + " to a duel" + "!\n\n" + player1 + " you have " + health1 + " health!\n" + player2 + " you have " + health2 + " health!\n\n" + text + " your turn!");

            }
            else
            {
                await ReplyAsync(":expressionless:  | " + Context.User.Mention + ", sorry but there is a fight going on right now, or you just tried to fight yourself...");
            }

        }


        [Command("giveup")]
        [Alias("GiveUp", "Giveup", "giveUp")]
        [Summary("Stops the fight and gives up.")]
        [Remarks("Ex: w!giveup")]
        [Cooldown(10, true)]
        public async Task GiveUp()
        {
            if (SwitchCaseString == "fight_p1")
            {
                await ReplyAsync(":flag_white:  | " + Context.User.Mention + " gave up. The fight stopped.");
                SwitchCaseString = "nofight";
                health1 = 100;
                health2 = 100;
            }
            else
            {
                await ReplyAsync(":neutral_face:  | There is no fight to stop.");
            }
        }

        [Command("Slash")]
        [Alias("slash")]
        [Summary("Slashes your foe with a sword. Good accuracy and medium damage")]
        [Remarks("Ex: w!Ssash")]
        [Cooldown(3)]
        public async Task Slash()
        {
            if (SwitchCaseString == "fight_p1")
            {
                if (whosTurn == Context.User.Mention)
                {
                    Random rand = new Random();

                    int randomIndex = rand.Next(1, 6);
                    if (randomIndex != 1)
                    {
                        Random rand2 = new Random();

                        int randomIndex2 = rand2.Next(7, 15);

                        if (Context.User.Mention != player1)
                        {
                            health1 = health1 - randomIndex2;
                            if (health1 > 0)
                            {
                                placeHolder = whosTurn;
                                whosTurn = whoWaits;
                                whoWaits = placeHolder;

                                await ReplyAsync(":dagger:  | " + Context.User.Mention + " You hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n\n" + whosTurn + " Your turn!");

                            }
                            else
                            {
                                await ReplyAsync(":skull_crossbones:  | " + Context.User.Mention + " You hit and did " + randomIndex2 + " damage!\n\n" + player1 + " died. " + player2 + " won!");
                                SwitchCaseString = "nofight";
                                health1 = 100;
                                health2 = 100;
                            }
                        }
                        else if (Context.User.Mention == player1)
                        {
                            health2 = health2 - randomIndex2;
                            if (health2 > 0)
                            {
                                placeHolder = whosTurn;
                                whosTurn = whoWaits;
                                whoWaits = placeHolder;

                                await ReplyAsync(":dagger:  | " + Context.User.Mention + " You hit and did " + randomIndex2 + " damage!\n\n" + player1 + " has " + health1 + " health left!\n" + player2 + " has " + health2 + " health left!\n\n" + whosTurn + " Your turn!");
                            }
                            else
                            {
                                await ReplyAsync(":skull_crossbones:  | " + Context.User.Mention + " You hit and did " + randomIndex2 + " damage!\n\n" + player2 + " died. " + player1 + " won!");
                                SwitchCaseString = "nofight";
                                health1 = 100;
                                health2 = 100;
                            }
                        }
                        else
                        {
                            await ReplyAsync(":sweat:  | Sorry it seems like something went wrong. Please type /giveup");
                        }


                    }
                    else
                    {
                        placeHolder = whosTurn;
                        whosTurn = whoWaits;
                        whoWaits = placeHolder;

                        await ReplyAsync(":shield:  | " + Context.User.Mention + " You missed!\n" + whosTurn + " Your turn!");
                    }
                }
                else
                {
                    await ReplyAsync(":octagonal_sign:  | " + Context.User.Mention + " it is not your turn.");
                }
            }
            else
            {
                await ReplyAsync("There is no fight at the moment. Sorry :/");
            }

        }
    }
}
