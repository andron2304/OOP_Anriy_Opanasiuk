using System;

namespace MusicPlayer.Domain
{
    public static class AudioEntityFactory
    {
        public static Track CreateTrack(string name, string artist, TimeSpan duration, int year)
        {
            return new Track(name, artist, duration, year);
        }

        public static Playlist CreatePlaylist(string name)
        {
            return new Playlist(name);
        }
    }
}
