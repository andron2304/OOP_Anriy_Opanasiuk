using System;
using System.Collections.Generic;

namespace MusicPlayer.Domain
{
    public class ShuffleStrategy : IPlaybackStrategy
    {
        private readonly Random _random = new();

        public Track GetNextTrack(List<Track> playlist)
        {
            if (playlist == null || playlist.Count == 0)
                throw new ArgumentException("Playlist cannot be null or empty");

            int randomIndex = _random.Next(playlist.Count);
            return playlist[randomIndex];
        }
    }
}
