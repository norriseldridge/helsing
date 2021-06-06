using Helsing.Client.Entity.Api;
using UniRx;
using Zenject;

namespace Helsing.Client.Item
{
    public class HealthItem : BaseItem
    {
        IMessageBroker broker;

        [Inject]
        private void Inject(IMessageBroker broker) =>
            this.broker = broker;

        public override void OnCollect()
        {
            broker.Publish(new HealthPickUpMessage(1));
            base.OnCollect();
        }
    }
}
