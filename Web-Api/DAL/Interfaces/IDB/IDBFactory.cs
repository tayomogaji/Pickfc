namespace Pickfc.DAL.Interfaces.IDB
{
    public interface IDBFactory<T> : IDisposable
    {
        T Init();
    }
}
