using Avalonia.Controls;
using Avalonia.Input;
using AvelonExplorer.ViewModels;

namespace AvelonExplorer.Controls;

public partial class FileExplorerDetailsViewControl : UserControl
{
    public FileExplorerDetailsViewControl()
    {
        InitializeComponent();
    }

    private void OnDataGridDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (this.DataContext is FileSystemTabViewModel viewModel && e.Source is Control sourceControl && sourceControl.DataContext is FileSystemItemViewModel itemViewModel)
        {
            viewModel.NavigateToItemCommand.Execute(itemViewModel);
        }
    }
}