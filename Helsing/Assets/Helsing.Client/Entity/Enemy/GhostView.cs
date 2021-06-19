namespace Helsing.Client.Entity.Enemy
{
    public class GhostView : EntityView
    {
        private void Update()
        {
            switch (State)
            {
                case EntityState.Walk:
                case EntityState.Idle:
                    Play("Ghost");
                    break;
            }
        }
    }
}