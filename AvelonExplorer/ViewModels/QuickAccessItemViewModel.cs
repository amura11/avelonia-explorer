using CommunityToolkit.Mvvm.ComponentModel;

namespace AvelonExplorer.ViewModels;

public partial class QuickAccessItemViewModel : ViewModelBase
{
    [ObservableProperty] private string name = string.Empty;

    [ObservableProperty] private string path = string.Empty;
}