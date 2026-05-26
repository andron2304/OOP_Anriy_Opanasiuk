using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain
{
    public class Playlist : AudioEntity, IPlaylistComponent, IEnumerable<Track>
    {
        private readonly List<Track> _tracks = new();
        private readonly List<Playlist> _childPlaylists = new();

        public IEnumerable<Track> Tracks => _tracks.AsReadOnly();
        public IEnumerable<Playlist> ChildPlaylists => _childPlaylists.AsReadOnly();

        public Playlist(string name) : base(name) { }

        public void Add(Track track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            _tracks.Add(track);
        }

        public void Remove(Track track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            _tracks.Remove(track);
        }

        public void AddPlaylist(Playlist playlist)
        {
            if (playlist == null) throw new ArgumentNullException(nameof(playlist));
            if (playlist == this) throw new ArgumentException("Cannot add playlist to itself");
            _childPlaylists.Add(playlist);
        }

        public void RemovePlaylist(Playlist playlist)
        {
            if (playlist == null) throw new ArgumentNullException(nameof(playlist));
            _childPlaylists.Remove(playlist);
        }

        public TimeSpan TotalDuration() => _tracks.Aggregate(TimeSpan.Zero, (acc, t) => acc + t.Duration);

        public IEnumerator<Track> GetEnumerator()
        {
            return new PlaylistIterator(_tracks);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => $"{Name} - {_tracks.Count} tracks, {TotalDuration():c}";
    }
}

