using Discord.Commands;

namespace Wsashi.Core.Modules
{
    public abstract class WsashiModule : ModuleBase<ShardedCommandContext>
    {
        public bool Enabled = true;
        public bool Disabled = false;
        public int Zero = 0;
    }
}
