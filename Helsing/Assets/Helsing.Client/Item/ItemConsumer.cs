using Helsing.Client.Item.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Item
{
    public class ItemConsumer : MonoBehaviour, IItemConsumer
    {
        IInventory inventory;

        [Inject]
        private void Inject(IInventory inventory) => this.inventory = inventory;

        public bool Consume(IItemData item, int quantity)
        {
            if (inventory.GetItemCount(item) >= quantity)
            {
                inventory.RemoveItem(item, quantity);
                return true;
            }

            return false;
        }
    }
}