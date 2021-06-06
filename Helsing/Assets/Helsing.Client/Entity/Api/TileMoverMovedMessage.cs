namespace Helsing.Client.Entity.Api
{
    public readonly struct TileMoverMovedMessage
    {
        public readonly ITileMover tileMover;

        public TileMoverMovedMessage(ITileMover tileMover) =>
            this.tileMover = tileMover;
    }
}
