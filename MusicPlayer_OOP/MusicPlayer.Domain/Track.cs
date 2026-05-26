using System;
using System.Collections.Generic;

namespace MusicPlayer.Domain
{
    public sealed class Track : AudioEntity, IPlayable, IPlaylistComponent, IEquatable<Track>
    {
        private string _artist;
        private TimeSpan _duration;
        private int _year;

        public string Artist
        {
            get => _artist;
            set => _artist = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Artist cannot be empty.") : value;
        }

        public TimeSpan Duration
        {
            get => _duration;
            set => _duration = value.TotalSeconds <= 0 ? throw new ArgumentException("Duration must be greater than zero.") : value;
        }

        public int Year
        {
            get => _year;
            set => _year = value < 0 ? throw new ArgumentOutOfRangeException(nameof(Year), "Year cannot be negative.") : value;
        }

        public Track(string name, string artist, TimeSpan duration, int year) : base(name)
        {
            Artist = artist;
            Duration = duration;
            Year = year;
        }

        public void Play()
        {
            // minimal implementation
        }

        public override string ToString()
        {
            return $"{Name} - {Artist} ({Duration:c})";
        }

        public override bool Equals(object? obj) => Equals(obj as Track);

        public bool Equals(Track? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Name == other.Name && Artist == other.Artist && Duration == other.Duration && Year == other.Year;
        }

        public override int GetHashCode()
        {
            HashCode hc = new HashCode();
            hc.Add(Id);
            hc.Add(Name);
            hc.Add(Artist);
            hc.Add(Duration);
            hc.Add(Year);
            return hc.ToHashCode();
        }

        public static bool operator ==(Track? left, Track? right) => EqualityComparer<Track>.Default.Equals(left, right);
        public static bool operator !=(Track? left, Track? right) => !(left == right);
    }
}
