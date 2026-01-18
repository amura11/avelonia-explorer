using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using AvelonExplorer.Enums;
using AvelonExplorer.Services;

namespace AvelonExplorer.ViewModels;

public partial class FileSystemTabViewModel : ObservableObject
{
    private readonly IFileSystemService fileSystemService;

    [ObservableProperty]
    private string currentPath = string.Empty;

    [ObservableProperty]
    private string displayName = string.Empty;

    [ObservableProperty]
    private ObservableCollection<FileSystemItemViewModel> items = new ObservableCollection<FileSystemItemViewModel>();

    [ObservableProperty]
    private FileSystemTabViewMode viewMode;
    
    public FileSystemTabViewModel(string path, IFileSystemService fileSystemService)
    {
        this.fileSystemService = fileSystemService;
        this.viewMode = FileSystemTabViewMode.Details;

        CurrentPath = path;
        DisplayName = fileSystemService.GetDirectoryName(path);

        LoadItems();
    }

    partial void OnCurrentPathChanged(string value)
    {
        DisplayName = fileSystemService.GetDirectoryName(value);

        LoadItems();
    }

    private void LoadItems()
    {
        Items.Clear();
        var fileSystemItems = fileSystemService.GetFileSystemItems(CurrentPath);

        foreach (var item in fileSystemItems)
        {
            Items.Add(new FileSystemItemViewModel(item));
        }
    }

    [RelayCommand]
    private void SetDetailsView()
    {
        ViewMode = FileSystemTabViewMode.Details;
    }

    [RelayCommand]
    private void SetGridView()
    {
        ViewMode = FileSystemTabViewMode.Grid;
    }
}