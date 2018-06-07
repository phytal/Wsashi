using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace Wsashi.Modules
{
    public class Interactive : InteractiveBase
    {
        [Command("delete")]
        public async Task<RuntimeResult> Test_DeleteAfterAsync()
        {
            await ReplyAndDeleteAsync("this message will delete in 10 seconds", timeout: TimeSpan.FromSeconds(10));
            return Ok();
        }

        [Command("next", RunMode = RunMode.Async)]
        public async Task Test_NextMessageAsync()
        {
            await ReplyAsync("What is 2+2?");
            var response = await NextMessageAsync();
            if (response == null || !response.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await ReplyAsync("You did not reply before the timeout");

                return;
            }
            await ReplyAsync($"You replied");
        }
        [Command("paginator")]
        public async Task Paginator()
        {
            var pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" };
            await PagedReplyAsync(pages);
        }
    }
}
