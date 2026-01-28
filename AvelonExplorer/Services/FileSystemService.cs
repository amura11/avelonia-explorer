using System;
using System.Collections.Generic;
using AvelonExplorer.Models;
using System.IO;
using AvelonExplorer.Enums;
using AvelonExplorer.ViewModels;

namespace AvelonExplorer.Services;

public class FileSystemService : IFileSystemService
{
    public IReadOnlyList<FileSystemItemModel> GetFileSystemItems(string path)
    {
        var items = new List<FileSystemItemModel>();

        try
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                return items;
            }

            // Get directories
            foreach (var directory in directoryInfo.GetDirectories())
            {
                items.Add(new FileSystemItemModel
                {
                    Type = FileSystemItemType.Directory,
                    Name = directory.Name,
                    FullPath = directory.FullName,
                    Size = 0,
                    Modified = directory.LastWriteTime,
                    Attributes = MapAttributes(directory.Attributes)
                });
            }

            // Get files
            foreach (var file in directoryInfo.GetFiles())
            {
                items.Add(new FileSystemItemModel
                {
                    Type = FileSystemItemType.File,
                    Name = file.Name,
                    FullPath = file.FullName,
                    Size = file.Length,
                    Modified = file.LastWriteTime,
                    Attributes = MapAttributes(file.Attributes)
                });
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Return empty list if access is denied
        }
        catch (DirectoryNotFoundException)
        {
            // Return empty list if directory not found
        }

        return items;
    }

    private static FileSystemItemAttribute MapAttributes(FileAttributes attributes)
    {
        var result = FileSystemItemAttribute.None;

        if ((attributes & FileAttributes.ReadOnly) != 0)
        {
            result |= FileSystemItemAttribute.ReadOnly;
        }

        if ((attributes & FileAttributes.Hidden) != 0)
        {
            result |= FileSystemItemAttribute.Hidden;
        }

        if ((attributes & FileAttributes.System) != 0)
        {
            result |= FileSystemItemAttribute.System;
        }

        if ((attributes & FileAttributes.Archive) != 0)
        {
            result |= FileSystemItemAttribute.Archive;
        }

        if ((attributes & FileAttributes.Compressed) != 0)
        {
            result |= FileSystemItemAttribute.Compressed;
        }

        if ((attributes & FileAttributes.Encrypted) != 0)
        {
            result |= FileSystemItemAttribute.Encrypted;
        }

        return result;
    }

    public string GetDirectoryName(string path)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
            {
                return "Unknown";
            }

            var directory = new DirectoryInfo(path);
            return directory.Exists ? directory.Name : "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    public string? GetParentDirectoryPath(string path)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return Directory.GetParent(path)?.FullName;
        }
        catch
        {
            return null;
        }
    }

    public bool IsRootDirectory(string path)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Parent == null;
        }
        catch
        {
            return false;
        }
    }
}

