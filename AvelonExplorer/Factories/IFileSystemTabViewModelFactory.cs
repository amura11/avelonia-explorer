namespace AvelonExplorer.ViewModels;

public interface IFileSystemTabViewModelFactory
{
    FileSystemTabViewModel Create(string path);
}
