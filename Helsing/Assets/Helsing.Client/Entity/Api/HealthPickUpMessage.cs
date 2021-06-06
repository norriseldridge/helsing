namespace Helsing.Client.Entity.Api
{
    public readonly struct HealthPickUpMessage
    {
        public readonly int lives;

        public HealthPickUpMessage(int lives) => this.lives = lives;
    }
}
