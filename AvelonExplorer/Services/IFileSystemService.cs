using System.Collections.Generic;
using AvelonExplorer.Models;

namespace AvelonExplorer.Services;

public interface IFileSystemService
{
    IReadOnlyList<FileSystemItemModel> GetFileSystemItems(string path);
    string GetDirectoryName(string path);
}