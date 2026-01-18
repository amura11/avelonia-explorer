using System;
using System.IO;
using AvelonExplorer.Enums;
using AvelonExplorer.ViewModels;

namespace AvelonExplorer.Models;

public class FileSystemItemModel
{
    public required string Name { get; set; } = string.Empty;
    
    public required string FullPath { get; set; } = string.Empty;
    
    public long? Size { get; set; }
    
    public required DateTime Modified { get; set; }
    
    public required FileSystemItemAttribute Attributes { get; set; }

    public required FileSystemItemType Type { get; set; }
}