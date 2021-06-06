using System.Collections.Generic;
using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using Helsing.Client.UI.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Player
{
    public class PlayerItemCollector : MonoBehaviour, IItemCollector
    {
        [SerializeField]
        string pickUpMessage;

        [SerializeField]
        AudioClip pickUp;

        IMessageBroker broker;
        IInventory inventory;
        IAudioPool audioPool;

        [Inject]
        private void Inject(IInventory inventory,
            IMessageBroker broker,
            IAudioPool audioPool) =>
            (this.inventory, this.broker, this.audioPool) =
            (inventory, broker, audioPool);

        private void Start() =>
            inventory.Load();

        public void Collect(IItem item)
        {
            broker.Publish(new MessageData($"Picked up {item.ItemData.DisplayName}."));
            audioPool.Next().PlayOneShot(pickUp);
            item.OnCollect();
            inventory.AddItem(item.ItemData, (item.ItemData is IItemCollection collection) ? collection.Quantity : 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var item = collision.gameObject.GetComponent<IItem>();
            if (item != null)
            {
                Collect(item);
                inventory.Save();
            }
        }
    }
}