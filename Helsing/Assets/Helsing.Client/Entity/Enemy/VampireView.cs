namespace Helsing.Client.Entity.Enemy
{
    public class VampireView : EntityView
    {
        private void Update()
        {
            switch (State)
            {
                case EntityState.Idle:
                    Play("Vampire_Idle");
                    break;

                case EntityState.Walk:
                    Play("Vampire_Float");
                    break;
            }
        }
    }
}