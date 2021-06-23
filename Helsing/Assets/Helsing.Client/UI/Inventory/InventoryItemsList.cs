using System.Collections.Generic;
using Helsing.Client.Item;
using Helsing.Client.Item.Api;
using UnityEngine;
using UniRx;
using Zenject;

namespace Helsing.Client.UI.Inventory
{
    public class InventoryItemsList : MonoBehaviour
    {
        [SerializeField]
        InventoryItemView itemViewSource;

        [SerializeField]
        List<ItemData> itemsToDisplay;

        IInventory inventory;
        Dictionary<string, InventoryItemView> views;

        [Inject]
        private void Inject(IInventory inventory) => this.inventory = inventory;

        private void Start()
        {
            views = new Dictionary<string, InventoryItemView>();
            foreach (var item in itemsToDisplay)
            {
                var view = Instantiate(itemViewSource, transform);
                view.DisplayItem(item.Sprite, inventory.GetItemCount(item));
                views[item.DisplayName] = view;
            }

            inventory.Items.ObserveAdd()
                .Subscribe(p => views[p.Key.DisplayName].UpdateCount(p.Value))
                .AddTo(this);

            inventory.Items.ObserveRemove()
                .Subscribe(p => views[p.Key.DisplayName].UpdateCount(p.Value))
                .AddTo(this);

            inventory.Items.ObserveReplace()
                .Subscribe(p => views[p.Key.DisplayName].UpdateCount(p.NewValue))
                .AddTo(this);
        }
    }
}
