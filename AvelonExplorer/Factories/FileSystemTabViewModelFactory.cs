using AvelonExplorer.Services;

namespace AvelonExplorer.ViewModels;

public class FileSystemTabViewModelFactory : IFileSystemTabViewModelFactory
{
    private readonly IFileSystemService fileSystemService;

    public FileSystemTabViewModelFactory(IFileSystemService fileSystemService)
    {
        this.fileSystemService = fileSystemService;
    }

    public FileSystemTabViewModel Create(string path)
    {
        return new FileSystemTabViewModel(path, fileSystemService);
    }
}
