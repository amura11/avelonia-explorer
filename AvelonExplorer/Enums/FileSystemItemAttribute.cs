using System;

namespace AvelonExplorer.ViewModels;

[Flags]
public enum FileSystemItemAttribute
{
    None = 0,
    ReadOnly = 1 << 0,
    Hidden = 1 << 1,
    System = 1 << 2,
    Archive = 1 << 3,
    Compressed = 1 << 4,
    Encrypted = 1 << 5
}