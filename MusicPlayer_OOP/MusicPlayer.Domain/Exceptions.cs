using System;

namespace MusicPlayer.Domain;

public sealed class TrackNotFoundException : Exception
{
    public TrackNotFoundException() { }
    public TrackNotFoundException(string message) : base(message) { }
    public TrackNotFoundException(string message, Exception inner) : base(message, inner) { }
}

public sealed class InvalidPlaylistOperationException : Exception
{
    public InvalidPlaylistOperationException() { }
    public InvalidPlaylistOperationException(string message) : base(message) { }
    public InvalidPlaylistOperationException(string message, Exception inner) : base(message, inner) { }
}
