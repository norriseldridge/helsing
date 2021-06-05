using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using Helsing.Client.Entity.Player;
using Helsing.Client.Entity.Player.Api;
using Helsing.Client.UI.Api;
using Helsing.Client.World;
using Helsing.Client.World.Api;
using UniRx;
using Zenject;

namespace Helsing.Client
{
    public class HelsingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessageBroker>().FromInstance(MessageBroker.Default).AsSingle();
            Container.Bind<IPathFinder>().FromInstance(new PathFinder()).AsSingle();
            Container.Bind<IPromptMessage>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITileMap>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IAudioPool>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IInventory>().To<PlayerInventory>().AsSingle();
            Container.Bind<IPlayerController>().FromComponentInHierarchy().AsSingle();
        }
    }
}