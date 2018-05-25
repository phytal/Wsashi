using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wsashi.Helpers;
using Wsashi.Core.Modules;
using Wsashi.Features.GlobalAccounts;

namespace Wsashi.Modules.Management
{
    public class MasterConfigCommand : WsashiModule
    {
        private DiscordSocketClient _client = Program._client;

        public static string ConvertBoolean(bool boolean)
        {
            return boolean == true ? "**On**" : "**Off**";
        }

        public static string ConvertList(List<string> list, int count)
        {
            return list.Count >= count ? "**On**" : "**Off**";
        }
        
        public static string ConvertList(List<ulong> list, int count)
        {
            return list.Count >= count ? "**On**" : "**Off**";
        }

        public static string ConvertDict(Dictionary<string, string> dict, int count)
        {
            return dict.Count >= count ? "**On**" : "**Off**";
        }

        
        [Command("Config")]
        [Summary("Displays all of the bot settings on this server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MasterConfig()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = MiscHelpers.CreateEmbed(Context, $"Server ID: {config.Id}\n" +
                                                     $"Owner: <@{config.GuildOwnerId}>");

            string modRole = "**Not set**";
            string adminRole = "**Not set**";
            if (config.ModRole != 0)
            {
                modRole = $"**{Context.Guild.Roles.First(role => role.Id == config.ModRole).Name}**";
            }
            else if (config.AdminRole != 0)
            {
                adminRole = $"**{Context.Guild.Roles.First(role => role.Id == config.ModRole).Name}**";
            }
            
            
            
            
            
            if (config.WelcomeChannel != 0)
            {
                embed.AddField("Welcome/Leaving", "On:\n" +
                                          $"- Channel: <#{config.WelcomeChannel}>\n" +
                                          $"- WelcomeMsg: {config.WelcomeMessage}\n" +
                                          $"- LeavingMsg: {config.LeavingMessage}");
            }
            else
            {
                embed.AddField("Welcome/Leaving", "Off");
            }

            embed.AddField("Other", $"Antilink: {ConvertBoolean(config.Antilink)}\n" +
                                    $"Mass Ping Checks: {ConvertBoolean(config.MassPingChecks)}\n" +
                                    $"Blacklist: {ConvertBoolean(config.Filter)}\n" +
                                    $"Autorole: {config.Autorole}\n" +
                                    $"Leveling: {config.Leveling}\n" +
                                    $"Server Logging: {ConvertBoolean(config.IsServerLoggingEnabled)}\n" +
                                    $"Mod Role: {modRole}\n" +
                                    $"Admin Role: {adminRole}\n");

            embed.WithThumbnailUrl(Context.Guild.IconUrl);

            await MiscHelpers.SendMessage(Context, embed);

        }
    }
}