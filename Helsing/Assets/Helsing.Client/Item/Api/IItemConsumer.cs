namespace Helsing.Client.Item.Api
{
    public interface IItemConsumer
    {
        bool Consume(IItemData item, int quantity);
    }
}