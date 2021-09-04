using Newtonsoft.Json;

namespace Helsing.Client.Item
{
    public readonly struct InventoryItemRef
    {
        public int ID => id;
        public int Quantity => quantity;

        [JsonProperty]
        readonly int id;
        [JsonProperty]
        readonly int quantity;

        public InventoryItemRef(int id, int quantity)
            => (this.id, this.quantity) = (id, quantity);
    }
}