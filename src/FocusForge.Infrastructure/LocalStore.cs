using System.Text.Json;

namespace FocusForge.Infrastructure;

public sealed class LocalStore<T> where T : new()
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _path;

    public LocalStore(string fileName)
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FocusForge");
        Directory.CreateDirectory(folder);
        _path = Path.Combine(folder, fileName);
    }

    public async Task<T> LoadAsync()
    {
        if (!File.Exists(_path))
        {
            return new T();
        }

        await using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions).ConfigureAwait(false) ?? new T();
    }

    public async Task SaveAsync(T value)
    {
        var temporaryPath = _path + ".tmp";
        await using (var stream = File.Create(temporaryPath))
        {
            await JsonSerializer.SerializeAsync(stream, value, JsonOptions).ConfigureAwait(false);
        }

        File.Move(temporaryPath, _path, overwrite: true);
    }
}
