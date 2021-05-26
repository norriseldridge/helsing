using UnityEngine;

namespace Helsing.Client.Item.Api
{
    public interface IItemData
    {
        int ID { get; }
        string DisplayName { get; }
        Sprite Sprite { get; }
    }
}