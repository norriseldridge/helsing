namespace Helsing.Client.Api
{
    public interface IPool<T>
    {
        T Next();
    }
}
