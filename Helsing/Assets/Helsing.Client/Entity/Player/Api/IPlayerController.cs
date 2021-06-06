using Helsing.Client.Api;
using Helsing.Client.Entity.Api;
using Helsing.Client.World.Api;

namespace Helsing.Client.Entity.Player.Api
{
    public interface IPlayerController : ITurnTaker
    {
        ILiving Living { get; }
        ITile CurrentTile { get; }
        bool Enabled { get; set; }
    }
}