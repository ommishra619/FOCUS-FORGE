using System.Text.Json;

namespace FocusForge.Infrastructure;

public sealed class LocalStore<T> where T : class, new()
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public LocalStore(string fileName)
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FocusForge");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, fileName);
    }

    public async Task<T> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
        {
            return new T();
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken) ?? new T();
    }

    public async Task SaveAsync(T value, CancellationToken cancellationToken = default)
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, value, _options, cancellationToken);
    }
}
