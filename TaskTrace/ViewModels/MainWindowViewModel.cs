using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using TaskTrace.Models;
using TaskTrace.Services;

namespace TaskTrace.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		private readonly SettingsService _settingsService = new SettingsService();
		private string _folderPath = "";
		public string FolderPath
		{
			get => _folderPath;
			set
			{
				this.RaiseAndSetIfChanged(ref _folderPath, value);
				_ = _settingsService.SaveFolderPathAsync(value); // save whenever it changes
			}
		}

		public ObservableCollection<TodoItem> Todos { get; }
		public ReactiveCommand<Unit, Unit> BrowseCommand { get; }
		public ReactiveCommand<Unit, Unit> ScanCommand { get; }
		public ReactiveCommand<Unit, Unit> ExportCommand { get; }

		public MainWindowViewModel()
		{
			Todos = new ObservableCollection<TodoItem>();
			// Ensure commands run on UI thread
			BrowseCommand = ReactiveCommand.CreateFromTask(BrowseFolderAsync, outputScheduler: AvaloniaScheduler.Instance);
			ScanCommand = ReactiveCommand.CreateFromTask(ScanFolderAsync, outputScheduler: AvaloniaScheduler.Instance);
			ExportCommand = ReactiveCommand.CreateFromTask(ExportTodosAsync, outputScheduler: AvaloniaScheduler.Instance);
			LoadLastFolder();
		}

		private async void LoadLastFolder()
		{
			var lastPath = await _settingsService.LoadFolderPathAsync();
			if (!string.IsNullOrWhiteSpace(lastPath) && Directory.Exists(lastPath))
			{
				FolderPath = lastPath;
			}
		}

		private async Task BrowseFolderAsync()
		{
			var window = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
			if (window == null) return;

			// Run dialog on UI thread
			string result = null;
			await Dispatcher.UIThread.InvokeAsync(async () =>
			{
				var dialog = new OpenFolderDialog { Title = "Select Folder" };
				result = await dialog.ShowAsync(window);
			});

			if (!string.IsNullOrEmpty(result))
				FolderPath = result;
		}


		private async Task ScanFolderAsync()
		{
			if (string.IsNullOrWhiteSpace(FolderPath) || !Directory.Exists(FolderPath))
				return;

			// Run the file scanning off the UI thread (background thread)
			var items = await Task.Run(() => FileScanner.ScanAsync(FolderPath));

			// Update Todos on the UI thread
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				Todos.Clear();
				foreach (var todo in items)
					Todos.Add(todo);
			});
		}


		private async Task ExportTodosAsync()
		{
			if (Todos.Count == 0) return;

			var window = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
			if (window == null) return;

			string path = null;
			await Dispatcher.UIThread.InvokeAsync(async () =>
			{
				var saveDialog = new SaveFileDialog
				{
					Title = "Export TODOs",
					InitialFileName = "todos.json",
					DefaultExtension = "json"
				};
				path = await saveDialog.ShowAsync(window);
			});

			if (!string.IsNullOrEmpty(path))
			{
				await ExportService.ExportAsync(Todos.ToList(), path);
			}
		}
	}
}
