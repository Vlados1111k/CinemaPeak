using System.Text.Json;
using CinemaPeak.Domain.Interfaces;
namespace CinemaPeak.Infrastructure.Persistence;

public class JsonDataStore<T> : IDataStore<T>
{
    private readonly string _filePath;

    public JsonDataStore(string fileName) => _filePath = fileName;

    public async Task SaveAsync(IReadOnlyCollection<T> items, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json, ct);
    }

    public async Task<IReadOnlyCollection<T>> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath)) return new List<T>();
        var json = await File.ReadAllTextAsync(_filePath, ct);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}