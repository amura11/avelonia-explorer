namespace AvelonExplorer.Messages;

public record NavigateToMessage(string Path) { }

public record NavigateUpMessage() { }

public record NavigateBackMessage() { }

public record NavigateForwardMessage() { }