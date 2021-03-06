using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using Helsing.Client.Entity.Player;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.Entity.Enemy;
using Helsing.Client.Fx.Api;
using Helsing.Client.UI.Api;
using Helsing.Client.World;
using Helsing.Client.World.Api;
using UniRx;
using Zenject;
using Helsing.Client.Entity.Enemy.Api;

namespace Helsing.Client
{
    public class HelsingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // General
            Container.Bind<IMessageBroker>().FromInstance(MessageBroker.Default).AsSingle();

            // Map
            Container.Bind<IPathFinder>().To<PathFinder>().AsTransient();
            Container.Bind<ITileMap>().FromComponentInHierarchy().AsSingle();

            // Enemy
            Container.Bind<IEnemyLogicFactory>().To<EnemyLogicFactory>().AsSingle();
            Container.Bind<IEnemyCoordinator>().To<EnemyCoordinator>().AsSingle();

            // Player
            Container.Bind<IInventory>().To<PlayerInventory>().AsSingle();
            Container.Bind<IPlayerController>().FromComponentInHierarchy().AsSingle();

            // UI
            Container.Bind<IPromptMessage>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IDeadPopup>().FromComponentInHierarchy().AsSingle();

            // Pooling
            Container.Bind<IAudioPool>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IParticlePool>().FromComponentInHierarchy().AsSingle();
        }
    }
}