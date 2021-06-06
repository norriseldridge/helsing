
using UnityEngine;

namespace Helsing.Client.Entity
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer view;

        [SerializeField]
        Animator animator;

        public bool FlipX
        {
            get => view.flipX;
            set => view.flipX = value;
        }

        public EntityState State { get; set; } = EntityState.Idle;

        public void Play(string animation) =>
            animator.Play(animation);
    }
}
