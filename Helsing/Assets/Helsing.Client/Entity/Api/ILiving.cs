namespace Helsing.Client.Entity.Api
{
    public interface ILiving
    {
        int Lives { get; }
        void DealDamage();
    }
}
