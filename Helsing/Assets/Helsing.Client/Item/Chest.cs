using Helsing.Client.Audio.Api;
using Helsing.Client.Item.Api;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Item
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Chest : MonoBehaviour, IItem
    {
        [SerializeField] BaseItem item;
        [SerializeField] Sprite opened;
        [SerializeField] AudioClip openSfx;

        SpriteRenderer spriteRenderer;
        new Collider2D collider;
        bool collected = false;
        IAudioPool audioPool;

        public IItemData ItemData => item.ItemData;

        [Inject]
        private void Inject(IAudioPool audioPool) =>
            this.audioPool = audioPool;

        public void OnCollect()
        {
            if (collected) return;

            audioPool.Next().PlayOneShot(openSfx);
            
            spriteRenderer.sprite = opened;
            enabled = false;
            item.OnCollect();
            collider.enabled = false;
            collected = true;
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            item.transform.position = transform.position;
            item.GetComponent<SpriteRenderer>().enabled = false;
            item.GetComponent<Collider2D>().enabled = false;
        }
#endif
    }
}
