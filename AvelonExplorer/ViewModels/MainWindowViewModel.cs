using System.Collections.ObjectModel;
using System.ComponentModel;
using AvelonExplorer.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AvelonExplorer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFileSystemTabViewModelFactory tabViewModelFactory;
    private readonly IMessenger messenger;

    [ObservableProperty]
    private ObservableCollection<FileSystemTabViewModel> fileSystemTabs;

    [ObservableProperty]
    private ObservableCollection<QuickAccessGroupViewModel> quickAccessGroups;

    [ObservableProperty]
    private FileSystemTabViewModel? selectedTab;

    public MainWindowViewModel()
    {
        this.tabViewModelFactory = null!;
        this.messenger = null!;
        this.fileSystemTabs = new ObservableCollection<FileSystemTabViewModel>();
        this.quickAccessGroups = new ObservableCollection<QuickAccessGroupViewModel>();
    }

    public MainWindowViewModel(IFileSystemTabViewModelFactory tabViewModelFactory, IMessenger messenger)
    {
        this.tabViewModelFactory = tabViewModelFactory;
        this.messenger = messenger;
        this.fileSystemTabs = new ObservableCollection<FileSystemTabViewModel>();
        this.quickAccessGroups = new ObservableCollection<QuickAccessGroupViewModel>();

        // Register to receive NavigateToMessage
        messenger.Register<NavigateToMessage>(this, ReceiveNavigateToMessage);

        this.Initialize();
    }

    private void Initialize()
    {
        //FUTURE: Load existing tabs or load initial
        var initialTab = this.tabViewModelFactory.Create(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
        this.FileSystemTabs.Add(initialTab);
        this.SelectedTab = initialTab;

        //FUTURE: Load these from settings/config
        this.QuickAccessGroups.Add(
            new QuickAccessGroupViewModel()
            {
                Name = "Test 1"
            }
        );

        this.QuickAccessGroups.Add(
            new QuickAccessGroupViewModel()
            {
                Name = "Test 2"
            }
        );
    }
    
    partial void OnSelectedTabChanged(FileSystemTabViewModel? oldValue, FileSystemTabViewModel? newValue)
    {
        // Unsubscribe from old tab's property changes
        if (oldValue != null)
        {
            oldValue.PropertyChanged -= OnSelectedTabPropertyChanged;
        }

        // Subscribe to new tab's property changes
        if (newValue != null)
        {
            newValue.PropertyChanged += OnSelectedTabPropertyChanged;
        }
    }

    private void OnSelectedTabPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(FileSystemTabViewModel.CanNavigateBackward)
            or nameof(FileSystemTabViewModel.CanNavigateForward)
            or nameof(FileSystemTabViewModel.CanNavigateUp)
            or nameof(FileSystemTabViewModel.CanNavigate))
        {
            this.NavigateBackCommand.NotifyCanExecuteChanged();
            this.NavigateForwardCommand.NotifyCanExecuteChanged();
            this.NavigateUpCommand.NotifyCanExecuteChanged();
            this.RefreshCommand.NotifyCanExecuteChanged();
        }
    }

    private void ReceiveNavigateToMessage(object sender, NavigateToMessage message)
    {
        this.SelectedTab?.NavigateTo(message.Path);
    }

    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    private void NavigateBack()
    {
        this.SelectedTab?.NavigateBack();
    }
    
    [RelayCommand(CanExecute = nameof(CanNavigateForward))]
    private void NavigateForward()
    {
        this.SelectedTab?.NavigateForward();
    }

    [RelayCommand(CanExecute = nameof(CanNavigateUp))]
    private void NavigateUp()
    {
        this.SelectedTab?.NavigateToParent();
    }

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private void Refresh()
    {
        this.SelectedTab?.Refresh();
    }

    private bool CanNavigateBack() => this.SelectedTab?.CanNavigateBackward ?? false;

    private bool CanNavigateForward() => this.SelectedTab?.CanNavigateForward ?? false;

    private bool CanNavigateUp() => this.SelectedTab?.CanNavigateUp ?? false;

    private bool CanRefresh() => this.SelectedTab?.CanNavigate ?? false;
}