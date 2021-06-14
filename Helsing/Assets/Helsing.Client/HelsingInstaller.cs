using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using Helsing.Client.Entity.Player;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.UI.Api;
using Helsing.Client.World;
using Helsing.Client.World.Api;
using UniRx;
using Zenject;
using Helsing.Client.Entity.Enemy.Api;
using Helsing.Client.Entity.Enemy;
using Helsing.Client.Fx.Api;
using Helsing.Client.Fx;

namespace Helsing.Client
{
    public class HelsingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // General
            Container.Bind<IMessageBroker>().FromInstance(MessageBroker.Default).AsSingle();

            // Map
            Container.Bind<IPathFinder>().FromInstance(new PathFinder()).AsSingle();
            Container.Bind<ITileMap>().FromComponentInHierarchy().AsSingle();

            // Enemy
            Container.Bind<IEnemyBlackboard>().FromInstance(new EnemyBlackboard()).AsSingle();
            Container.Bind<IFactory<IEnemyLogic>>().To<EnemyLogicFactory>().AsSingle();

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