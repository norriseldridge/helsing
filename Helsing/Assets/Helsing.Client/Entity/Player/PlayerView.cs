using Helsing.Client.Audio.Api;
using Helsing.Client.Item;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Player
{
    public class PlayerView : MonoBehaviour
    {
        public bool FlipX
        {
            get => view.flipX;
            set
            {
                view.flipX = value;

                if (item != null)
                    item.FlipX = value;
            }
        }

        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        [SerializeField]
        SpriteRenderer view;

        [SerializeField]
        Animator animator;

        [SerializeField]
        AudioClip step;

        [SerializeField]
        float stepDelay;

        [SerializeField]
        ItemView item;

        IAudioPool audioPool;
        float currentStepDelay;

        int stepIndex;

        [Inject]
        private void Inject(IAudioPool audioPool) =>
            this.audioPool = audioPool;

        public EntityState State { get; set; } = EntityState.Idle;

        private void Update()
        {
            currentStepDelay -= Time.deltaTime;
            switch (State)
            {
                case EntityState.Idle:
                    animator.Play("Player_Idle");
                    break;

                case EntityState.Walk:
                    animator.Play("Player_Run");
                    if (currentStepDelay <= 0.0f)
                    {
                        stepIndex++;
                        var source = audioPool.Next();
                        source.pitch = stepIndex % 2 == 0 ? Random.Range(1.0f, 1.1f) : Random.Range(1.1f, 1.2f);
                        source.volume = Random.Range(0.2f, 0.3f);
                        source.PlayOneShot(step);
                        currentStepDelay = stepDelay;
                    }
                    break;
            }
        }
    }
}