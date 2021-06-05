using Helsing.Client.World.Api;
using UniRx;

namespace Helsing.Client.Entity.Api
{
    public interface ITileMover
    {
        IReadOnlyReactiveProperty<ITile> CurrentTile { get; }
        IReadOnlyReactiveProperty<ITile> NextTile { get; }
        bool CanMove(Direction direction);
    }
}
