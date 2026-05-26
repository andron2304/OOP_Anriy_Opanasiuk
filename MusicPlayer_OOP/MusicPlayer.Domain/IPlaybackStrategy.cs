using System.Collections.Generic;

namespace MusicPlayer.Domain
{
    public interface IPlaybackStrategy
    {
        Track GetNextTrack(List<Track> playlist);
    }
}
