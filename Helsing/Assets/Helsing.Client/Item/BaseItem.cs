using Helsing.Client.Item.Api;
using UnityEngine;

namespace Helsing.Client.Item
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaseItem : MonoBehaviour, IItem
    {
        public IItemData ItemData => itemData;

        [SerializeField]
        private ItemData itemData;

        [SerializeField]
        SpriteRenderer spriteRenderer;

        void Start() => UpdateItem();

        void OnValidate() => UpdateItem();

        void UpdateItem()
        {
            gameObject.name = (itemData != null) ? $"[Item] {itemData.DisplayName}" : "<Null Item>";
            spriteRenderer.sprite = (itemData != null) ? itemData.Sprite : null;
        }

        public virtual void OnCollect() => Destroy(gameObject);
    }
}