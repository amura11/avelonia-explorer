using CommunityToolkit.Mvvm.ComponentModel;
using System;
using AvelonExplorer.Enums;
using AvelonExplorer.Models;

namespace AvelonExplorer.ViewModels;

public partial class FileSystemItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string fullPath = string.Empty;

    [ObservableProperty]
    private FileSystemItemType itemType = FileSystemItemType.File;

    [ObservableProperty]
    private long size = 0;

    [ObservableProperty]
    private DateTime modified = DateTime.MinValue;

    public FileSystemItemViewModel(FileSystemItemModel model)
    {
        Name = model.Name;
        FullPath = model.FullPath;
        ItemType = model.Type;
        Size = model.Size ?? 0;
        Modified = model.Modified;
    }
}