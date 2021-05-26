using Helsing.Client.World.Api;

namespace Helsing.Client.Player.Api
{
    public interface IPlayerController
    {
        ITile CurrentTile { get; }
    }
}