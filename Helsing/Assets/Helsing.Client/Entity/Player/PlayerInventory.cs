using Helsing.Client.Item.Api;
using UniRx;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Item;

namespace Helsing.Client.Entity.Player
{
    public class PlayerInventory : IInventory
    {
        readonly string DataPath = Path.Combine(UnityEngine.Application.dataPath, "player");
        readonly string FilePath = Path.Combine(UnityEngine.Application.dataPath, "player", "inventory.json");
        public IReadOnlyReactiveDictionary<IItemData, int> Items => items;
        readonly ReactiveDictionary<IItemData, int> items = new ReactiveDictionary<IItemData, int>();

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

        public async void Save()
        {
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            await Task.Run(() => {
                using var stream = File.Open(FilePath, FileMode.OpenOrCreate);
                using var writer = new StreamWriter(stream);

                var temp = items.Select(i => new InventoryItemRef(i.Key.ID, i.Value)).ToArray();
                string json = JsonConvert.SerializeObject(temp, Formatting.Indented);
                writer.Write(json);
            });
        }

        public void Load()
        {
            // TODO load the inventory
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                if (!string.IsNullOrEmpty(json))
                {
                    var tempItems = JsonConvert.DeserializeObject<InventoryItemRef[]>(json);
                    foreach (var itemRef in tempItems)
                    {
                        // find item data by id

                    }
                }
            }
        }
    }
}