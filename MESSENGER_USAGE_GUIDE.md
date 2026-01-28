# Messenger Service Usage Guide

## Overview
The `MessengerService` is a wrapper around the MVVM Community Toolkit's `WeakReferenceMessenger`. It provides a centralized messaging system for pub/sub communication between ViewModels.

## Setup

The `MessengerService` is automatically registered as a singleton in the dependency injection container via `Registrar.cs`:

```csharp
services.AddSingleton<IMessengerService, MessengerService>();
```

## Basic Usage

### 1. Define a Message

Create message classes to represent the data you want to send:

```csharp
public class NavigationRequestMessage
{
    public string Path { get; set; }

    public NavigationRequestMessage(string path)
    {
        Path = path;
    }
}
```

### 2. Inject the Service

In your ViewModel, inject the `IMessengerService`:

```csharp
public class MyViewModel : ObservableObject
{
    private readonly IMessengerService _messengerService;

    public MyViewModel(IMessengerService messengerService)
    {
        _messengerService = messengerService;
    }
}
```

### 3. Register to Receive Messages

```csharp
public MyViewModel(IMessengerService messengerService)
{
    _messengerService = messengerService;
    
    // Register to receive NavigationRequestMessage
    _messengerService.Register<NavigationRequestMessage>(this, (recipient, message) =>
    {
        // Handle the message
        NavigateTo(message.Path);
    });
}
```

### 4. Send Messages

```csharp
[RelayCommand]
private void RequestNavigation()
{
    var message = new NavigationRequestMessage("/some/path");
    _messengerService.Send(message);
}
```

### 5. Unregister (Optional)

When your ViewModel is disposed:

```csharp
public void Cleanup()
{
    _messengerService.Unregister<NavigationRequestMessage>(this);
}
```

## Advanced Features

### Send to Specific Recipient

```csharp
_messengerService.SendTo(message, specificRecipient);
```

### Access the Underlying Messenger

If you need direct access to the MVVM Toolkit's IMessenger:

```csharp
var messenger = _messengerService.Messenger;
```

## Benefits

- **Decoupling**: ViewModels don't need direct references to each other
- **Flexibility**: Easy to add new communication paths without changing architecture
- **Memory Safety**: Uses `WeakReferenceMessenger` to prevent memory leaks
- **Testability**: Can be mocked for unit testing
- **Centralized**: Single point for managing cross-ViewModel communication

## Example: File Explorer Implementation

```csharp
public class FileSystemTabViewModel : ObservableObject
{
    private readonly IMessengerService _messengerService;

    public FileSystemTabViewModel(
        string path, 
        IFileSystemService fileSystemService,
        IMessengerService messengerService)
    {
        _messengerService = messengerService;
        
        // Subscribe to navigation messages
        _messengerService.Register<NavigationRequestMessage>(this, (r, msg) =>
        {
            NavigateTo(msg.Path);
        });

        CurrentPath = path;
        DisplayName = fileSystemService.GetDirectoryName(path);
        LoadItems();
    }

    [RelayCommand]
    private void OpenFile(FileSystemItemViewModel item)
    {
        // Notify other ViewModels
        _messengerService.Send(new FileSelectedMessage(item.Path));
    }
}
```
