namespace Helsing.Client.Item.Api
{
    public interface IItem
    {
        IItemData ItemData { get; }
        void OnCollect();
    }
}