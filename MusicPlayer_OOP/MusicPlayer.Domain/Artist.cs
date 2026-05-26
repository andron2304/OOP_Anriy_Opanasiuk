using System;
using System.Collections.Generic;

namespace MusicPlayer.Domain
{
    public class Artist : AudioEntity
    {
        private readonly List<Album> _albums = new();

        public IEnumerable<Album> Albums => _albums.AsReadOnly();

        public Artist(string name) : base(name) { }

        internal void AddAlbum(Album album)
        {
            if (album == null) throw new ArgumentNullException(nameof(album));
            _albums.Add(album);
        }
    }
}
