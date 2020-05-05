
namespace CentralService.Storage
{
    public interface IFileStorage<T>
    {
        void SaveToStorage(T item);
    }
}
