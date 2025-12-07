using MusicCatalogue.Api.Entities;

namespace MusicCatalogue.Api.Interfaces
{
    public interface IBackgroundQueue<T> where T : BackgroundWorkItem
    {
        T? Dequeue();
        void Enqueue(T item);
    }
}