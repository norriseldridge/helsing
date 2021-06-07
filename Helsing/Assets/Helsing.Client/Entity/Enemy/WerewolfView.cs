namespace Helsing.Client.Entity.Enemy
{
    public class WerewolfView : EntityView
    {
        private void Update()
        {
            switch (State)
            {
                case EntityState.Idle:
                    Play("Werewolf_Idle");
                    break;

                case EntityState.Walk:
                    Play("Werewolf_Run");
                    break;
            }
        }
    }
}