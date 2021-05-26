using System.Collections.Generic;
using Helsing.Client.Item.Api;

namespace Helsing.Client.Player
{
    public class PlayerInventory : IInventory
    {
        Dictionary<IItemData, int> items = new Dictionary<IItemData, int>();

        public void AddItem(IItemData item, int quantity)
        {
            if (quantity <= 0) return;

            var current = GetItemCount(item);
            items[item] = current + quantity;
        }

        public void RemoveItem(IItemData item, int quantity)
        {
            if (quantity <= 0) return;

            var current = GetItemCount(item);
            if (current < quantity)
                throw new System.Exception($"The player does not have enough to remove. Owns {current} but removing {quantity}...");
            items[item] = current - quantity;
        }

        public int GetItemCount(IItemData item) =>
            items.ContainsKey(item) ? items[item] : 0;

        public override string ToString()
        {
            string result = "";
            if (items.Count > 0)
            {
                foreach (var slot in items)
                    result += $"{slot.Key.DisplayName}(ID: {slot.Key.ID}) x{slot.Value}\n";
            }
            else
            {
                result = "No items.";
            }
            return result;
        }

        public void Save()
        {
            // TODO save the inventory
        }

        public void Load()
        {
            // TODO load the inventory
        }
    }
}