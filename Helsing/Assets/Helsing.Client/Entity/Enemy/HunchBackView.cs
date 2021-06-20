namespace Helsing.Client.Entity.Enemy
{
    public class HunchBackView : EntityView
    {
        private void Update()
        {
            switch (State)
            {
                case EntityState.Idle:
                case EntityState.Walk:
                    Play("HunchBack_Idle");
                    break;
            }
        }
    }
}
