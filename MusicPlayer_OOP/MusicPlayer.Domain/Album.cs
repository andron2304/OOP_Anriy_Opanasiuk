using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain
{
    public class Album : AudioEntity
    {
        private readonly List<Track> _tracks = new();

        public IEnumerable<Track> Tracks => _tracks.AsReadOnly();
        public int ReleaseYear { get; private set; }

        public Album(string name, int releaseYear) : base(name)
        {
            ReleaseYear = releaseYear < 0 ? throw new ArgumentOutOfRangeException(nameof(releaseYear)) : releaseYear;
        }

        public void AddTrack(Track track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            _tracks.Add(track);
        }

        public override string ToString() => $"{Name} ({ReleaseYear}) - {Tracks.Count()} tracks";
    }
}
