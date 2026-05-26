using System;
using System.Collections.Generic;

namespace MusicPlayer.Domain
{
    public class RepeatStrategy : IPlaybackStrategy
    {
        private Track? _currentTrack;

        public Track GetNextTrack(List<Track> playlist)
        {
            if (playlist == null || playlist.Count == 0)
                throw new ArgumentException("Playlist cannot be null or empty");

            if (_currentTrack == null || !playlist.Contains(_currentTrack))
            {
                _currentTrack = playlist[0];
            }

            return _currentTrack;
        }

        public void SetCurrentTrack(Track track)
        {
            _currentTrack = track;
        }
    }
}
