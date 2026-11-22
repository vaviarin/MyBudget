using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

public sealed class JsonExpenseStore(string path) : IExpenseStore
{
    private readonly string _path = path;
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public IReadOnlyList<Expense> Load()
    {
        if (!File.Exists(_path)) return [];
        string json = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(json)) return [];
        return JsonSerializer.Deserialize<List<Expense>>(json, Options) ?? [];
    }

    public void Save(IEnumerable<Expense> expenses)
    {
        string? directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        File.WriteAllText(_path, JsonSerializer.Serialize(expenses, Options));
    }
}
