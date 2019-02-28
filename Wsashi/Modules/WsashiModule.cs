using Discord.Addons.Interactive;

namespace Wsashi.Core.Modules
{
    public abstract class WsashiModule : InteractiveBase
    {
        public bool Enabled = true;
        public bool Disabled = false;
        public int Zero = 0;
    }
}
