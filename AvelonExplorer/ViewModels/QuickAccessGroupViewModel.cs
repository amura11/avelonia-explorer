using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvelonExplorer.ViewModels;

public partial class QuickAccessGroupViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private ObservableCollection<QuickAccessItemViewModel> items = new ObservableCollection<QuickAccessItemViewModel>();
}