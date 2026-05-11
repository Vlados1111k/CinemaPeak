public interface IDataStore<T>
{
    Task<IReadOnlyCollection<T>> LoadAsync(CancellationToken ct = default);
    Task SaveAsync(IReadOnlyCollection<T> items, CancellationToken ct = default);
}