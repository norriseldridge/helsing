using UnityEngine;

namespace Helsing.Client.Entity.Enemy
{
    public class VampireView : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer view;

        public bool FlipX
        {
            get => view.flipX;
            set => view.flipX = value;
        }

        [SerializeField]
        Animator animator;

        public EntityState State { get; set; } = EntityState.Idle;

        private void Update()
        {
            switch (State)
            {
                case EntityState.Idle:
                    animator.Play("Vampire_Idle");
                    break;

                case EntityState.Walk:
                    animator.Play("Vampire_Float");
                    break;
            }
        }
    }
}