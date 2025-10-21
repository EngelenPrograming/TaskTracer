using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TaskTrace.Models;

namespace TaskTrace.Services;

public static class ExportService
{
	public static async Task ExportAsync(List<TodoItem> items, string filePath)
	{
		var options = new JsonSerializerOptions { WriteIndented = true };
		var json = JsonSerializer.Serialize(items, options);
		await File.WriteAllTextAsync(filePath, json);
	}
}
