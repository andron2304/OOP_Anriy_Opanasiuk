using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain
{
    public class PlaylistIterator : IEnumerator<Track>
    {
        private readonly List<Track> _tracks;
        private int _currentIndex = -1;

        public PlaylistIterator(IEnumerable<Track> tracks)
        {
            _tracks = tracks?.ToList() ?? new List<Track>();
        }

        public Track Current
        {
            get
            {
                if (_currentIndex < 0 || _currentIndex >= _tracks.Count)
                    throw new InvalidOperationException("Enumerator is not in a valid state");
                return _tracks[_currentIndex];
            }
        }

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _currentIndex++;
            return _currentIndex < _tracks.Count;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }

        public void Dispose()
        {
            // No resources to dispose
        }
    }
}
