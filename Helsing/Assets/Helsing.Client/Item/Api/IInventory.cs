using Helsing.Client.Api;

namespace Helsing.Client.Item.Api
{
    public interface IInventory : IData
    {
        void AddItem(IItemData item, int quantity);
        void RemoveItem(IItemData item, int quantity);
        int GetItemCount(IItemData item);
    }
}