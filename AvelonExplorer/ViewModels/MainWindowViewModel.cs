using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvelonExplorer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFileSystemTabViewModelFactory tabViewModelFactory;

    [ObservableProperty] 
    private ObservableCollection<FileSystemTabViewModel> fileSystemTabs;

    [ObservableProperty]
    private ObservableCollection<QuickAccessGroupViewModel> quickAccessGroups;
    
    [ObservableProperty]
    private FileSystemTabViewModel? selectedTab;

    public MainWindowViewModel()
    {
        this.tabViewModelFactory = null!;
        this.fileSystemTabs = new ObservableCollection<FileSystemTabViewModel>();
        this.quickAccessGroups = new ObservableCollection<QuickAccessGroupViewModel>();
    }

    public MainWindowViewModel(IFileSystemTabViewModelFactory tabViewModelFactory)
    {
        this.tabViewModelFactory = tabViewModelFactory;
        this.fileSystemTabs = new ObservableCollection<FileSystemTabViewModel>();
        this.quickAccessGroups = new ObservableCollection<QuickAccessGroupViewModel>();
        
        this.Initialize();
    }

    private void Initialize()
    {
        //FUTURE: Load existing tabs or load initial
        var initialTab = this.tabViewModelFactory.Create(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
        this.FileSystemTabs.Add(initialTab);
        this.SelectedTab = initialTab;
        
        //FUTURE: Load these from settings/config
        this.QuickAccessGroups.Add(new QuickAccessGroupViewModel()
        {
            Name = "Test 1"
        });
        
        this.QuickAccessGroups.Add(new QuickAccessGroupViewModel()
        {
            Name = "Test 2"
        });
    }
}