using UniRx;

namespace Helsing.Client.Entity.Api
{
    public interface ILiving
    {
        int Lives { get; }
        IReadOnlyReactiveProperty<int> LivesAsObservable { get; }
        void DealDamage();
    }
}
