namespace Helsing.Client.Entity.Enemy.Api
{
    public readonly struct PlayerSpottedMessage
    {
        public readonly IEnemy spotter;

        public PlayerSpottedMessage(IEnemy spotter) =>
            this.spotter = spotter;
    }
}
