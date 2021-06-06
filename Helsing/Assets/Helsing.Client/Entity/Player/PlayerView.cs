using Helsing.Client.Audio.Api;
using Helsing.Client.Item;
using UnityEngine;
using Zenject;

namespace Helsing.Client.Entity.Player
{
    public class PlayerView : EntityView
    {
        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

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

        private void Update()
        {
            if (item != null)
                item.FlipX = FlipX;

            currentStepDelay -= Time.deltaTime;
            switch (State)
            {
                case EntityState.Idle:
                    Play("Player_Idle");
                    break;

                case EntityState.Walk:
                    Play("Player_Run");
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