using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskTrace.Models;

namespace TaskTrace.Services;
public static class FileScanner
{
	private static readonly string[] SupportedExtensions =
		[".cs", ".cpp", ".h", ".py", ".js", ".ts", ".gd"]; // can be expanded to others

	private static readonly Regex TodoRegex = new(
		@"(?<tag>TODO|FIXME|NOTE)[:\s-]*(?<text>.*)",
		RegexOptions.IgnoreCase | RegexOptions.Compiled
	);

	public static async Task<List<TodoItem>> ScanAsync(string rootPath)
	{
		var results = new ConcurrentBag<TodoItem>();

		var files = Directory
			.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
			.Where(f => SupportedExtensions.Contains(Path.GetExtension(f)))
			.ToList();

		var tasks = files.Select(async file =>
		{
			var lines = await File.ReadAllLinesAsync(file);
			for (int i = 0; i < lines.Length; i++)
			{
				var match = TodoRegex.Match(lines[i]);
				if (!match.Success) continue;

				results.Add(new TodoItem
				{
					FilePath = file,
					LineNumber = i + 1,
					Tag = match.Groups["tag"].Value.ToUpper(),
					Text = match.Groups["text"].Value.Trim()
				});
			}
		});

		await Task.WhenAll(tasks);
		return results.OrderBy(r => r.FilePath).ThenBy(r => r.LineNumber).ToList();
	}
}
