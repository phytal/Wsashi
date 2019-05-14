using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Wsashi.Preconditions;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;
using Wsashi.Helpers;

namespace Wsashi
{
    public class Information : WsashiModule
    {
        private CommandService _service;

        public Information(CommandService service)
        {
            _service = service;
        }

        [Command("info")]
        [Summary("Gets info for a user")]
        [Alias("Whois", "userinfo")]
        [Remarks("w!info <user you want to see> Ex: w!info @Phytal")]
        [Cooldown(10)]
        public async Task UserInfo(SocketGuildUser user)
        {
            var thumbnailurl = user.GetAvatarUrl();

            var auth = new EmbedAuthorBuilder()

            {
                Name = user.Username,
                IconUrl = thumbnailurl,
            };

            var bottom = new EmbedFooterBuilder()
            {
                Text = $"Requested by {Context.User.Username}#{Context.User.Discriminator}",
                IconUrl = (Context.User.GetAvatarUrl())
            };

            var embed = new EmbedBuilder()
            {
                Author = auth,
                Footer = bottom
            };

            embed.WithThumbnailUrl(user.GetAvatarUrl());
            if (user.Status == UserStatus.Online)
            {
                embed.WithColor(new Color(0, 255, 0));
            }
            if (user.Status == UserStatus.Idle)
            {
                embed.WithColor(new Color(255, 255, 0));
            }
            if (user.Status == UserStatus.DoNotDisturb)
            {
                embed.WithColor(new Color(255, 0, 0));
            }
            if (user.Status == UserStatus.AFK)
            {
                embed.WithColor(new Color(220, 20, 60));
            }
            if (user.Status == UserStatus.Invisible)
            {
                embed.WithColor(new Color(128, 128, 128));
            }
            if (user.Status == UserStatus.Offline)
            {
                embed.WithColor(new Color(192, 192, 192));
            }

            string nickname = user.Nickname;
            if (string.IsNullOrEmpty(nickname))
            {
                nickname = "None";
            }

            /*string game = user.Activity.ToString();
            if (string.IsNullOrEmpty(game))
            {
                game = "Currently not playing";
            }*/
            var roleNames = user.Roles.Select(r => r.Name); //return a collection of strings with only the role names
            var roleNamesMerged = String.Join(", ", roleNames); //separates every element of the collection with a comma in a single string, ready to display!
            
            embed.AddField("Name", $"**{user}**", true);
            embed.AddField("ID", $"**{user.Id}**", true);
            embed.AddField("Discriminator", $"**{user.Discriminator}**", true);
            embed.AddField("Is Bot", $"**{ConvertBool.ConvertBooleanYN(user.IsBot)}**", true);
            embed.AddField("Joined Discord at", $"**{user.CreatedAt}**", true);
            embed.AddField($"Joined {Context.Guild}", $"**{user.JoinedAt}**", true);
            embed.AddField("Nickname", $"**{nickname}**", true);
            //embed.AddField("Playing", $"**{game}**", true);
            embed.AddField("Status", $"**{user.Status}**", true);
            embed.AddField("Roles", $"**{roleNamesMerged}**", true);

            await ReplyAsync("", embed: embed.Build());
        }


        [Command("ServerInfo"), Alias("sinfo", "serveri", "si")]
        [Summary("Provides information for the current server")]
        [Remarks("Ex: w!server info")]
        [Cooldown(10)]
        public async Task ServerInformationCommand()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithTitle("Server Information");
            embed.AddField("Name", Context.Guild.Name, true);
            embed.AddField("Created", Context.Guild.CreatedAt.UtcDateTime, true);
            embed.AddField("Users", Context.Guild.Users.Count, true);
            embed.AddField("Text Channels", Context.Guild.TextChannels.Count, true);
            embed.AddField("Voice Channels", Context.Guild.VoiceChannels.Count, true);
            embed.AddField("Region", Context.Guild.VoiceRegionId, true);
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            embed.AddField("Roles", Context.Guild.Roles.Count, true);
            embed.AddField("Verification Level", Context.Guild.VerificationLevel, true);
            embed.AddField("Donator Guild", config.VerifiedGuild, true);

            embed.WithColor(37, 152, 255);

            await ReplyAsync("", embed: embed.Build());
        }

        /*[Command("userinfo")]
        [Summary("Shows info about the requested user")]
        [Alias("whois")]
        public async Task UserIngfo(IGuildUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please include a name.");
            }
            else
            {
                var application = await Context.Client.GetApplicationInfoAsync();
                var thumbnailurl = user.GetAvatarUrl();
                var date = $"{user.CreatedAt.Month}/{user.CreatedAt.Day}/{user.CreatedAt.Year}";
                var auth = new EmbedAuthorBuilder()

                {

                    Name = user.Username,
                    IconUrl = thumbnailurl,

                };

                var embed = new EmbedBuilder()

                {
                    Color = new Color(37, 152, 255),
                    Author = auth
                };

                var us = user as SocketGuildUser;

                var username = us.Username;

                var discr = us.Discriminator;
                var id = us.Id;
                var dat = date;
                var stat = us.Status;
                var CC = us.JoinedAt;
                var game = us.Game;
                var nick = us.Nickname;
                embed.Title = $"**{us.Username}** Information";
                embed.Description = $"Username: **{username}**\n"
                   + $"Discriminator: **{discr}**\n"
                   + $"User ID: **{id}**\n"
                     + $"Created at: **{date}**\n"
                   + $"Current Status: **{stat}**\n"
                   + $"Joined server at: **{CC}**\n"
                       + $"Playing: **{game}**";

                await ReplyAsync("", embed: embed.Build());
            }
        }
        */

        [Command("info")]
        [Summary("Shows Wsashi's information")]
        [Remarks("Ex: w!info")]
        [Cooldown(10)]
        public async Task Info()
        {
            string version = Config.bot.Version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.AddField("Creator", "Phytal#8213", true);
            embed.AddField("Last Updated", "11/29/2018", true);
            embed.AddField("Bot version", $"Beta {version}", true);
            embed.WithImageUrl(Global.Client.CurrentUser.GetAvatarUrl());

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("help")]
        [Summary("Shows all possible Standard Commands for this bot")]
        [Remarks("Ex: w!help")]
        [Cooldown(30)]
        public async Task HelpMessage()
        {
            string helpMessage =
            "```cs\n" +
            "'Standard Command List'\n" +
            "```\n" +
            "Use `w!command [command]` to get more info on a specific command. Ex: `w!command xp`  `[Prefix 'w!']` \n " +
            "\n" +
            "**1. Core -** `help` `invite` `patreon` `ping` `dailyvote` \n" +
            "**2. Social -** `xp` `level` `stats` `topXP`\n" +
            "**3. Interaction -** `cuddle` `feed` `hug` `kiss` `pat` `poke` `tickle` `slap`\n" +
            "**4. Fun -** `8ball` `pick` `roast` `hello` `goodMorning` `goodNight` `fortune` `echo` `lenny` `rateWaifu` `reverse` `bigLetter` `playSong` `rps`\n" +
            "**5. Gambling -** `roll` `coinFlip` `newSlots` `slots` `showSlots`\n" +
            "**5. Economy -** `balance` `daily` `rank`\n" +
            "**6. Utilities -** `dm` `report` `voice`\n" +
            "**7. Calculator (w!calc <command) -** `add` `sub` `mult` `div` `sqrt` `power`\n" +
            "**8. Information -** `info` `userInfo` `command` `update`\n" +
            "**9. APIs -** `dog` `dogGif` `cat` `catGif` `catFact` `person` `birb` `define` `meme` `gif` `weather` `fortnite`\n" +
            "**10. Neko -** `neko` `catEmoticon` `foxGirl`\n" +
            "**11. Shibe -** `shiba` `bird`\n" +
            "**12. Overwatch -** `owStats` `owStatsComp` `owStatsQP` `myOwStats` `myOwStatsComp` `myOwStatsQP` `owHeroStats` `owHeroStatsComp` `owHeroStatsQP` `myOwHeroStats` `myOwHeroStatsComp` `myOwHeroStatsQP` `owAccount`\n" +
            "**11. osu! -** `osuStats` `maniaStats` `taikoStats` `ctbStats`\n" +
            "**14. Games -** `2048 start` `trivia`\n" +
            "**15. Blog (w!blog <command>) -** `create` `post` `subscribe` `unsubscribe`\n" +
            "**16. Self Roles -** `iAm` `iAmNot` `selfRoleList`\n" +
            "**17. Wasagotchi (w!wasagotchi <command>) -** `stats` `feed` `clean` `train` `play` `name` `buy` `picture` `help` `opencapsule` `inventory`\n" +
            "**18. Reminders (w!r <command>) -** `add` `remove` `list`\n" +
                        "**19. Personal Tags (w!ptag <command>)-** `new` `update` `remove` `list`\n" + //add gru and butterfly meme when done
                        "**20. Lootboxes -** `openLootBox` `lootBoxInventory` `giftLootbox`\n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view Moderator commands, use w!helpmod\n" +
            "# To view NSFW commands, use w!helpnsfw\n" +
            "# To view Dueling commands, use w!duelhelp\n" +
            "```";

            await ReplyAsync(helpMessage);
        }

        [Command("helpmod")]
        [Summary("Shows all possible Moderator Commands for this bot")]
        [Remarks("Ex: w!helpmod")]
        [Cooldown(30)]
        public async Task HelpMessageMod()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                string helpMessageMod =
            "```cs\n" +
            "Moderator Command List\n" +
            "```\n" +
            "Use `w!command [command]` to get more info on a specific command. Ex: `w!command xp`  `[Prefix 'w!']`\n" +
            "\n" +
            "**Filters -** `antiLink` `filter` `pingChecks` `antiLinkIgnore` `filterIgnore` `blacklistAdd` `blacklistRemove` `blacklistClear` `blacklistList`\n" +
            "**User Management -** `ban` `kick` `mute` `unmute` `clear` `warn` `warnings` `clearWarnings` `say` `softban` `idBan` `promote` `demote` `rename`\n" +
            "**Bot Settings -** `serverprefix` `leveling` `levelingMsg` `config`\n" +
            "**Welcome Messages (w!welcome <command>) -** `channel` `add` `remove` `list`\n" +
            "**Leaving Messages (w!leave <command>) -** `channel` `add` `remove` `list`\n" +
            "**Announcements (w!announcements <command>) -** `setChannel` `unsetChannel` `announce`\n" +
            "**Server Management -** `serverLogging` `slowMode` `lockChannel` `unlockChannel`\n" +
            "**Roles -** `helperRole` `modRole` `adminRole` `selfRoleAdd` `selfRoleRem` `selfRoleClear`\n" +
                        "**Server Tags (w!tag <command>)-** `new` `update` `remove` `list`\n" +
            "**Fun Stuff -** `unFlip` `vote` `customCommandAdd` `customCommandRemove` `customCommandList`\n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view standard commands, use w!help\n" +
            "# To view NSFW commands, use w!helpnsfw\n" +
            "```";

                await ReplyAsync(helpMessageMod);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("helpnsfw")]
        [Summary("Shows all possible NSFW Commands for this bot")]
        [Remarks("Ex: w!helpnsfw")]
        [Cooldown(30)]
        public async Task HelpMessageNSFW()
        {
            if (Context.Channel is ITextChannel text)
            {
                var nsfw = text.IsNsfw;
                if (nsfw)
                {
                    string helpMessageNSFW =
                "```cs\n" +
                "NSFW Command List (why did i make this)\n" +
                "```\n" +
                "Use `w!command [command]` to get more info on a specific command. Ex: `w!command xp`  `[Prefix 'w!']`\n" +
                "\n" +
                "**Neko -** `nekoLewd` `nekoNsfwGif`\n" +
                "**Hentai -** `anal` `boobs` `cum` `les` `pussy` `blowJob` `classic` `kuni` `overwatchNSFW`\n" +
                "\n" +
                "```\n" +
                "# Don't include the example brackets when using commands!\n" +
                "# To view Standard commands, use w!help\n" +
                "# To view Moderator commands, use w!helpmod\n" +
                "```";

                    await ReplyAsync(helpMessageNSFW);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You Need to be in a NSFW channel to do that {Context.User.Username}";
                    var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
        }

        [Command("command")]
        [Summary("Shows what a specific command does and the usage.")]
        [Remarks("w!command <command you want to search up> Ex: w!command xp")]
        [Cooldown(5)]
        public async Task CommandAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like {command}.");
                return;
            }

            var thumbnailurl = Context.User.GetAvatarUrl();

            var auth = new EmbedAuthorBuilder()
            {
                IconUrl = thumbnailurl,
            };
            var builder = new EmbedBuilder()
            {
                Author = auth,
                Title = ":book: Command Dictionary",
                Color = new Color(37, 152, 255),
                Description = $"Here are the aliases of **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = //$"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                                $"Description: {cmd.Summary}\n" +
                                $"Usage: {cmd.Remarks}";
                    x.IsInline = false;
                });
            }
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("wsashilink")]
        [Summary("Provides Wsashi's server invite link")]
        [Alias("serverinvitelink")]
        [Remarks("Ex: w!wsashilink")]
        [Cooldown(10)]
        public async Task SendAsync()
        {
            await ReplyAsync("https://discord.gg/NuUdx4h ~~ Here's my server! :blush: ");
        }

        [Command("dailyvote")]
        [Summary("Provides Wsashi's daily vote link")]
        [Alias("dv")]
        [Remarks("Ex: w!dv")]
        [Cooldown(10)]
        public async Task DailyVote()
        {
            await ReplyAsync("https://discordbots.org/bot/417160957010116608/vote ~~ Vote for me please! ");
        }

        [Command("invite")]
        [Summary("Invite Wsashi to your server!")]
        [Alias("Wsashiinvitelink")]
        [Remarks("Ex: w!invite")]
        [Cooldown(10)]
        public async Task InviteAsync()
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=417160957010116608&permissions=8&scope=bot ~~ Invite me to your servers! :blush: ");
        }

        [Command("patreon")]
        [Summary("Sends the Patreon link to help contribue to the efforts of Wsashi")]
        [Alias("donate")]
        [Remarks("Ex: w!patreon")]
        [Cooldown(10)]
        public async Task Patreon()
        {
            await ReplyAsync("https://www.patreon.com/phytal ~~ Help us out! :blush: ");
        }

        [Command("Update")]
        [Summary("Shows the latest update notes")]
        [Alias("updatenotes")]
        [Remarks("Ex: w!update")]
        [Cooldown(15)]
        public async Task Update()
        {
            string version = Config.bot.Version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Update Notes");
            embed.WithDescription($"`Bot version {version}` **<<Last Updated on 6/24>>**\n"
                + "`----- LAST UPDATE -----`\n"
                + "• Added rock paper scissors!Use `w!rps`!\n"
                + "• Made it so that you will now have a * seperate * account per server, money is carried over, but XP is different (the leveling system was also updated :D)!\n"
                + "• Aesthetically improved the `w!command` command!\n"
                + "• Squished a bugs and fixed typos :D..\n"
                + "`----- CURRENT UPDATE -----`\n"
                + " • Improved the old dueling system(IMPROVED IT SO MUCH) use `w!duelhelp` to see the commands!\n"
                + " • Added more stuff to the Dog and Cat API(Cat API broke so I got a new onw :D)!\n"
                + " • Fixed `w!help` as some commands were missing..\n"
                + " • Added Reminders!You can use `w!reminder add < reminder > in < time >`.\n"
                + " • Improved gambling(just `w!coinflip` i'm sorry xd)\n"
                );

            await ReplyAsync("", embed: embed.Build());
        }
    }
}
