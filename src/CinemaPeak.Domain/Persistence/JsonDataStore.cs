using System.Text.Json;

namespace CinemaPeak.Infrastructure.Persistence;

public class JsonDataStore<T>
{
    private readonly string _filePath;

    public JsonDataStore(string fileName)
    {
        _filePath = Path.IsPathRooted(fileName) 
            ? fileName 
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }

    public async Task SaveAsync(IEnumerable<T> data)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[Error I/O]: Не вдалося записати дані у файл {_filePath}. Причина: {ex.Message}");
            throw;
        }
    }

    public async Task<List<T>> LoadAsync()
    {
        if (!File.Exists(_filePath)) return new List<T>();

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[Fault Handling]: Файл даних {_filePath} пошкоджений. Стан скинуто. Деталі: {ex.Message}");
            return new List<T>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[Error I/O]: Помилка доступу до файлу {_filePath}: {ex.Message}");
            return new List<T>();
        }
    }
}