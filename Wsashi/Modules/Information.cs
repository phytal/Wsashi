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
        [Summary("Getting info for a user")]
        [Alias("Whois", "userinfo")]
        public async Task UserInfo(IGuildUser user)
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
                nickname = user.Username;
            }

            string game = user.Game.ToString();
            if (string.IsNullOrEmpty(game))
            {
                game = "Currently not playing";
            }
            embed.AddField("Name", $"**{user}**", true);
            embed.AddField("ID", $"**{user.Id}**", true);
            embed.AddField("Discriminator", $"**{user.Discriminator}**", true);
            embed.AddField("Joined Discord at", $"**{user.CreatedAt}**", true);
            embed.AddField($"Joined {Context.Guild}", $"**{user.JoinedAt}**", true);
            embed.AddField("Nickname", $"**{nickname}**", true);
            embed.AddField("Playing", $"**{game}**", true);
            embed.AddField("Status", $"**{user.Status}**", true);

            await ReplyAsync("", false, embed);
        }


        [Command("ServerInfo"), Alias("sinfo", "serveri", "si")]
        [Summary("Provides information for the current server")]
        public async Task ServerInformationCommand()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithTitle("Server Information");
            embed.AddField("Name", Context.Guild.Name);
            embed.AddField("Created", Context.Guild.CreatedAt.UtcDateTime);
            embed.AddField("Users", Context.Guild.Users.Count);
            embed.AddField("Text Channels", Context.Guild.TextChannels.Count);
            embed.AddField("Voice Channels", Context.Guild.VoiceChannels.Count);
            embed.AddField("Region", Context.Guild.VoiceRegionId);
            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            embed.AddField("Roles", Context.Guild.Roles.Count);
            embed.AddField("Donator Guild", config.VerifiedGuild);

            embed.WithColor(37, 152, 255);

            await ReplyAsync("", false, embed);
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

                await ReplyAsync("", false, embed.Build());
            }
        }
        */

        [Command("info")]
        [Summary("Shows Wsashi's information")]
        public async Task Info()
        {
            string version = Config.bot.Version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.AddInlineField("Creator", "Phytal#8213");
            embed.AddInlineField("Last Updated", "5/28/2018");
            embed.AddInlineField("Bot version", $"Beta {version}");
            embed.WithImageUrl(Global.Client.CurrentUser.GetAvatarUrl());

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("help")]
        [Summary("Shows all possible Standard Commands for this bot")]
        [Cooldown(15)]
        public async Task HelpMessage()
        {
            string helpMessage =
            "```cs\n" +
            "'Standard Command List'\n" +
            "```\n" +
            "Use `/command [command]` to get more info on a specific command. Ex: `/command xp`  `[Prefix 'w!']` \n " +
            "\n" +
            "**1. Core -** `help` `invite` `ping` \n" +
            "**2. Social -** `xp` `level` `stats` \n" +
            "**2.5. Interaction -** `cuddle` `feed` `hug` `kiss` `pat` `poke` `tickle`\n" +
            "**3. Fun -** `8ball` `pick` `roast` `hello` `normalhello` `goodmorning` `goodnight` `fortune` `echo` `lenny`\n" +
            "**4. Duels -** `slash` `giveup` `duel` \n" +
            "**5. Gambling -** `roll` `coinflip` `newslots` `slots`\n" +
            "**5. Economy -** `balance` `daily` `rank`\n" +
            "**6. Utilities -** `dm` `report`\n" +
            "**7. Calculator (Quik Mafs)-** `add` `minus` `multiply` `divide`\n" +
            "**8. Music (Under Development) -** `join` `leave` `play`\n" +
            "**8. Information -** `info` `userinfo` `command` `update`\n" +
            "**9. APIs -** `dog` `cat` `catfact` `person` `birb` `define` `meme`\n" +
            "**10. Neko API -** `neko` `catemoticon` `foxgirl`\n" +
            "**11. Shibe API -** `shiba` `bird`\n" +
            "**12. Overwatch API -** `owstats` `owstatscomp``owstatsqp``myowstats``myowstatscomp``myowstatsqp``owaccount`\n" +
            "**11. osu! API -** `osustats`\n" +
            "**14. Games -** `2048 start` `trivia`\n" +
            "**15. Blog (w!blog <command>) -** `create` `post` `subscribe` `unsubscribe`\n" +
            "**16. Self Roles -** `Iam` `Iamnot` \n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view Moderator commands, use w!helpmod\n" +
            "# To view NSFW commands, use w!helpnsfw\n" +
            "```";

            await ReplyAsync(helpMessage);
        }

        [Command("helpmod")]
        [Summary("Shows all possible Moderator Commands for this bot")]
        [Cooldown(15)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpMessageMod()
        {
            string helpMessageMod =
            "```cs\n" +
            "Moderator Command List\n" +
            "```\n" +
            "Use `/command [command]` to get more info on a specific command. Ex: `/command xp`  `[Prefix 'w!']`\n" +
            "\n" +
            "**Filters -** `antilink` `filter` `pingchecks` `antilinkignore`\n" +
            "**User Management -** `ban` `kick` `mute` `unmute` `clear` `warn` `say` `softban` `idban`\n" +
            "**Bot Settings -** `serverprefix` `leveling` `list` \n"+
            "**Welcome Messages (w!welcome <command>) -** `channel` `add` `remove` `list`\n" +
            "**Leaving Messages (w!leave <command>) -** `add` `remove` `list`\n" +
            "**Announcements (w!announcements <command>) -** `setchannel` `unsetchannel`\n" +
            "**Server Management -** `rename` `serverlogging` \n"+
            "**Roles -** `ModRole` `AdminRole` `selfroleadd` `selfrolerem` `selfroleclear`\n"+
            "**Promoting and Demoting (w!promote/demote <command>)-** `admin` `mod` `helper` \n"+
            "\n"+
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view standard commands, use w!help\n" +
            "# To view NSFW commands, use w!helpnsfw\n" +
            "```";

            await ReplyAsync(helpMessageMod);
        }

        [Command("helpnsfw")]
        [Summary("Shows all possible NSFW Commands for this bot")]
        [Cooldown(15)]
        [RequireNsfw]
        public async Task HelpMessageNSFW()
        {
            string helpMessageMod =
            "```cs\n" +
            "NSFW Command List (why did i make this)\n" +
            "```\n" +
            "Use `/command [command]` to get more info on a specific command. Ex: `/command xp`  `[Prefix 'w!']`\n" +
            "\n" +
            "**Neko -** `nekolewd` `nekonsfwgif`\n" +
            "**Hentai -** `anal` `boobs` `cum` `les` `pussy` `blowjob` `classic` `kuni`\n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view Standard commands, use w!help\n" +
            "# To view Moderator commands, use w!helpmod\n" +
            "```";

            await ReplyAsync(helpMessageMod);
        }
        // [Command("help")]
        //[Cooldown(15)]
        //public async Task Help()
        // {

        // await Context.Channel.SendMessageAsync("**Check your DMs!** :envelope_with_arrow:");

        // var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

        // var builder = new EmbedBuilder()
        // {
        //   Title = "Help",
        //   Description = "Bot prefix: '/' " +
        //    "These are the commands you can use:",
        //    Color = new Color(37, 152, 255)
        //   };

        // foreach (var module in _service.Modules)
        // {
        //     string description = null;
        //     var descriptionBuilder = new StringBuilder();
        //      descriptionBuilder.Append(description);
        //     foreach (var cmd in module.Commands)
        //     {
        //        var result = await cmd.CheckPreconditionsAsync(Context);
        //        if (result.IsSuccess)
        //          descriptionBuilder.Append($"{cmd.Aliases.First()}\n");
        //   }
        //    description = descriptionBuilder.ToString();

        //   if (!string.IsNullOrWhiteSpace(description))
        //   {
        //       builder.AddField(x =>
        //         {
        //           x.Name = module.Name;
        //          x.Value = description;
        //            x.IsInline = false;
        //    });
        //   }
        // }
        //   await dmChannel.SendMessageAsync("", false, builder.Build());
        // }

        [Command("command")]
        [Summary("Shows what a specific command does and what parameters it takes.")]
        [Cooldown(5)]
        public async Task CommandAsync(string command)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like {command}.");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = new Color(37, 152, 255),
                Description = $"Here are commands related to {command}"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                                $"Remarks/Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("wsashilink")]
        [Summary("Provides Wsashi's server invite link")]
        [Alias("serverinvitelink")]
        public async Task SendAsync()
        {
            await ReplyAsync("https://discord.gg/NuUdx4h");
        }

        [Command("invite")]
        [Summary("Invite Wsashi to your server!")]
        [Alias("Wsashiinvitelink")]
        public async Task InviteAsync()
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=417160957010116608&permissions=8&scope=bot");
        }

        [Command("Update")]
        [Summary("Shows the latest update notes")]
        [Alias("updatenotes")]
        public async Task Update()
        {
            string version = Config.bot.Version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Update Notes");
            embed.WithDescription($"`Bot version {version} **<<Last Updated on 5/28>>**\n"
                + "• Restored promoting/demoting. But now it relies on the Admin/Mod/Helper role you set for the server.\n"
                + "• Added a Helper role optiom, use `w!HelperRole` to set it.\n"
                + "• Added Reputation Points, use `w!rep <user>` to give them a point! It can be used once every 24 hours.\n"
                + "• Added a 'Master Config' panel, just use w!config to see the settings of Wsashi in this guild.\n"
                + "• Removed the 'Points' system, as it was replaced by Rep Points.\n"
                + "`----- CURRENT UPDATE -----`\n"
                + "• Added the osu! API to the bot! Use w!osustats <user> to start!\n"
                + "• Added the Overwatch APi to the bot! Simply find the OWAPI section in w!help!\n"
                + "• Added DMing! You can now DM mentioned users with a message!.\n"
                );

            await ReplyAsync("", embed: embed);
        }
    }
}
