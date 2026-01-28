using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AvelonExplorer.Enums;
using AvelonExplorer.Services;

namespace AvelonExplorer.ViewModels;

public partial class FileSystemTabViewModel : ObservableObject
{
    private readonly IFileSystemService fileSystemService;
    private readonly Stack<string> backHistory = new Stack<string>();
    private readonly Stack<string> forwardHistory = new Stack<string>();

    public string Path
    {
        get => field;
        private set => SetProperty(ref field, value);
    } = null!;

    public string Title
    {
        get => field;
        private set => SetProperty(ref field, value);
    } = null!;

    public bool CanNavigateForward
    {
        get => field;
        private set => SetProperty(ref field, value);
    }

    public bool CanNavigateBackward
    {
        get => field;
        private set => SetProperty(ref field, value);
    }

    public bool CanNavigateUp
    {
        get => field;
        private set => SetProperty(ref field, value);
    }

    public bool CanNavigate
    {
        get => field;
        private set => SetProperty(ref field, value);
    }
    
    [ObservableProperty]
    private FileSystemItemViewModel? selectedItem;

    [ObservableProperty]
    private ObservableCollection<FileSystemItemViewModel> items = new ObservableCollection<FileSystemItemViewModel>();

    [ObservableProperty]
    private FileSystemTabViewMode viewMode;

    public FileSystemTabViewModel(string path, IFileSystemService fileSystemService)
    {
        this.fileSystemService = fileSystemService;
        this.viewMode = FileSystemTabViewMode.Details;

        PerformNavigation(path);
    }

    public void Refresh()
    {
        if (this.CanNavigate == false)
        {
            return;
        }
        
        LoadItems();
    }

    public void NavigateToParent()
    {
        if (this.CanNavigateUp == false)
        {
            return;
        }

        var parentPath = fileSystemService.GetParentDirectoryPath(Path);

        if (parentPath != null)
        {
            backHistory.Push(Path);
            forwardHistory.Clear();

            PerformNavigation(parentPath);
        }
    }

    public void NavigateBack()
    {
        if (this.CanNavigateBackward == false)
        {
            return;
        }

        forwardHistory.Push(this.Path);
        var previousPath = backHistory.Pop();

        PerformNavigation(previousPath);
    }

    public void NavigateForward()
    {
        if (this.CanNavigateForward == false)
        {
            return;
        }

        backHistory.Push(this.Path);
        var nextPath = forwardHistory.Pop();

        PerformNavigation(nextPath);
    }

    public void NavigateTo(string path)
    {
        if (this.CanNavigate == false)
        {
            return;
        }

        backHistory.Push(this.Path);
        forwardHistory.Clear();

        PerformNavigation(path);
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

    [RelayCommand]
    private void NavigateToItem(FileSystemItemViewModel item)
    {
        if (item.ItemType is FileSystemItemType.Directory or FileSystemItemType.Drive)
        {
            NavigateTo(item.FullPath);
        }
    }

    private void PerformNavigation(string path)
    {
        this.CanNavigate = false;
        this.CanNavigateBackward = false;
        this.CanNavigateForward = false;
        this.CanNavigateUp = false;

        this.Path = path;
        this.Title = fileSystemService.GetDirectoryName(path);

        LoadItems();

        this.CanNavigate = true;
        this.CanNavigateBackward = this.backHistory.Count > 0;
        this.CanNavigateForward = this.forwardHistory.Count > 0;
        this.CanNavigateUp = fileSystemService.IsRootDirectory(path) == false;
    }
    
    private void LoadItems()
    {
        Items.Clear();
        var fileSystemItems = fileSystemService.GetFileSystemItems(Path);

        foreach (var item in fileSystemItems)
        {
            Items.Add(new FileSystemItemViewModel(item));
        }
    }
}