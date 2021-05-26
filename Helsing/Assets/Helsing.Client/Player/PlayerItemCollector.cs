using System.Collections.Generic;
using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using Helsing.Client.UI.Api;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Player
{
    public class PlayerItemCollector : MonoBehaviour, IItemCollector
    {
        [SerializeField]
        string pickUpMessage;

        [SerializeField]
        AudioClip pickUp;

        List<IItem> canCollect = new List<IItem>();
        IMessageBroker broker;
        IPromptMessage prompt;
        IInventory inventory;
        IAudioPool audioPool;

        [Inject]
        private void Inject(IInventory inventory,
            IMessageBroker broker,
            IPromptMessage prompt,
            IAudioPool audioPool) =>
            (this.inventory, this.broker, this.prompt, this.audioPool) =
            (inventory, broker, prompt, audioPool);

        private void Start()
        {
            inventory.Load();
        }

        public void Collect(IItem item)
        {
            canCollect.Remove(item);
            broker.Publish(new MessageData($"Picked up {item.ItemData.DisplayName}."));
            audioPool.Next().PlayOneShot(pickUp);
            item.OnCollect();
            inventory.AddItem(item.ItemData, (item.ItemData is IItemCollection collection) ? collection.Quantity : 1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                while (canCollect.Count > 0)
                {
                    var item = canCollect[0];
                    Collect(item);
                }
                canCollect.Clear();

                inventory.Save();
            }
            else
            {
                if (canCollect.Count > 0)
                    prompt.SetMessage("Press [SPACE] to collect item.");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var item = collision.gameObject.GetComponent<IItem>();
            if (item != null && !canCollect.Contains(item))
            {
                canCollect.Add(item);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var item = collision.gameObject.GetComponent<IItem>();
            if (item != null)
            {
                canCollect.Remove(item);
            }
        }
    }
}