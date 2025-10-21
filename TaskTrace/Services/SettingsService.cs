using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskTrace.Services
{
	internal class SettingsService
	{
		private readonly string _filePath;

		public SettingsService()
		{
			var appFolder = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"TaskTrace"
			);

			if (!Directory.Exists(appFolder))
				Directory.CreateDirectory(appFolder);

			_filePath = Path.Combine(appFolder, "user_settings.json");
		}
		public async Task SaveFolderPathAsync(string path)
		{
			var json = JsonSerializer.Serialize(new { FolderPath = path });
			await File.WriteAllTextAsync(_filePath, json);
		}

		public async Task<string> LoadFolderPathAsync()
		{
			if (!File.Exists(_filePath))
				return null;

			try
			{
				var json = await File.ReadAllTextAsync(_filePath);
				var data = JsonSerializer.Deserialize<FolderData>(json);
				return data?.FolderPath;
			}
			catch
			{
				return null;
			}
		}

		private class FolderData
		{
			public string FolderPath { get; set; }
		}
	}
}
