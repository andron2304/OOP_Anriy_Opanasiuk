using System;
using System.Collections.Generic;
using Xunit;
using MusicPlayer.Domain;

namespace MusicPlayer.Tests
{
    public class TrackTests
    {
        [Fact]
        public void Constructor_EmptyName_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Track("", "A", TimeSpan.FromSeconds(10), 2000));
        }

        [Fact]
        public void Constructor_EmptyArtist_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Track("Name", "  ", TimeSpan.FromSeconds(10), 2000));
        }

        [Fact]
        public void Constructor_NonPositiveDuration_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Track("Name", "Artist", TimeSpan.Zero, 2000));
        }

        [Fact]
        public void Constructor_NegativeYear_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Track("Name", "Artist", TimeSpan.FromSeconds(10), -1));
        }

        [Fact]
        public void ToString_IncludesNameArtistAndDuration()
        {
            var t = new Track("S", "A", TimeSpan.FromSeconds(65), 2021);
            var s = t.ToString();
            Assert.Contains("S", s);
            Assert.Contains("A", s);
            Assert.Contains("00:01:05", s);
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            var t = new Track("S", "A", TimeSpan.FromSeconds(10), 2020);
            Assert.False(t.Equals(null));
        }

        [Fact]
        public void Equality_Operator_SameReference_True()
        {
            var t = new Track("S", "A", TimeSpan.FromSeconds(10), 2020);
            Assert.True(t == t);
            Assert.False(t != t);
        }

        [Fact]
        public void GetHashCode_Consistent()
        {
            var t = new Track("S", "A", TimeSpan.FromSeconds(10), 2020);
            var h1 = t.GetHashCode();
            var h2 = t.GetHashCode();
            Assert.Equal(h1, h2);
        }
    }

    public class PlaylistEdgeTests
    {
        [Fact]
        public void Add_Null_Throws()
        {
            var p = new Playlist("P");
            Assert.Throws<ArgumentNullException>(() => p.Add(null!));
        }

        [Fact]
        public void Remove_Null_Throws()
        {
            var p = new Playlist("P");
            Assert.Throws<ArgumentNullException>(() => p.Remove(null!));
        }

        [Fact]
        public void AddPlaylist_Null_Throws()
        {
            var p = new Playlist("P");
            Assert.Throws<ArgumentNullException>(() => p.AddPlaylist(null!));
        }

        [Fact]
        public void AddPlaylist_Self_Throws()
        {
            var p = new Playlist("P");
            Assert.Throws<ArgumentException>(() => p.AddPlaylist(p));
        }

        [Fact]
        public void RemovePlaylist_Null_Throws()
        {
            var p = new Playlist("P");
            Assert.Throws<ArgumentNullException>(() => p.RemovePlaylist(null!));
        }

        [Fact]
        public void TotalDuration_SumsCorrectly()
        {
            var p = new Playlist("P");
            p.Add(new Track("A", "X", TimeSpan.FromSeconds(30), 2000));
            p.Add(new Track("B", "Y", TimeSpan.FromSeconds(45), 2001));
            Assert.Equal(TimeSpan.FromSeconds(75), p.TotalDuration());
        }

        [Fact]
        public void ToString_IncludesCountAndDuration()
        {
            var p = new Playlist("P");
            p.Add(new Track("A", "X", TimeSpan.FromSeconds(30), 2000));
            var s = p.ToString();
            Assert.Contains("1 tracks", s);
            Assert.Contains("00:00:30", s);
        }
    }

    public class PlaylistIteratorTests
    {
        [Fact]
        public void Enumerator_MoveNextAndCurrent_Works()
        {
            var tracks = new List<Track>
            {
                new Track("A","X",TimeSpan.FromSeconds(10),2000),
                new Track("B","Y",TimeSpan.FromSeconds(20),2001)
            };
            var it = new PlaylistIterator(tracks);
            Assert.True(it.MoveNext());
            Assert.Equal(tracks[0], it.Current);
            Assert.True(it.MoveNext());
            Assert.Equal(tracks[1], it.Current);
            Assert.False(it.MoveNext());
        }

        [Fact]
        public void Enumerator_CurrentBeforeMove_Throws()
        {
            var it = new PlaylistIterator(new List<Track>());
            Assert.Throws<InvalidOperationException>(() => { var c = it.Current; });
        }

        [Fact]
        public void Enumerator_Reset_Works()
        {
            var tracks = new List<Track> { new Track("A","X",TimeSpan.FromSeconds(5),2000) };
            var it = new PlaylistIterator(tracks);
            Assert.True(it.MoveNext());
            it.Reset();
            Assert.True(it.MoveNext());
        }
    }

    public class StrategyTests
    {
        [Fact]
        public void Shuffle_NullOrEmpty_Throws()
        {
            var s = new ShuffleStrategy();
            Assert.Throws<ArgumentException>(() => s.GetNextTrack(null!));
            Assert.Throws<ArgumentException>(() => s.GetNextTrack(new List<Track>()));
        }

        [Fact]
        public void Repeat_Behavior()
        {
            var r = new RepeatStrategy();
            var t1 = new Track("A","X",TimeSpan.FromSeconds(5),2000);
            var t2 = new Track("B","Y",TimeSpan.FromSeconds(6),2001);
            var list = new List<Track> { t1, t2 };
            var first = r.GetNextTrack(list);
            Assert.Equal(t1, first);
            r.SetCurrentTrack(t2);
            Assert.Equal(t2, r.GetNextTrack(list));
            // if current not in list, picks first
            var r2 = new RepeatStrategy();
            r2.SetCurrentTrack(new Track("Z","Z",TimeSpan.FromSeconds(1),1990));
            Assert.Equal(t1, r2.GetNextTrack(list));
        }
    }

    public class RepositoryTests
    {
        [Fact]
        public void Add_Null_Throws()
        {
            var repo = new Repository<Track>();
            Assert.Throws<ArgumentNullException>(() => repo.Add(null!));
        }

        [Fact]
        public void Remove_Null_Throws()
        {
            var repo = new Repository<Track>();
            Assert.Throws<ArgumentNullException>(() => repo.Remove(null!));
        }

        [Fact]
        public void Remove_ReturnsExpected()
        {
            var repo = new Repository<Track>();
            var t = new Track("A","X",TimeSpan.FromSeconds(1),2000);
            repo.Add(t);
            Assert.True(repo.Remove(t));
            Assert.False(repo.Remove(t));
        }

        [Fact]
        public void GetAll_ReturnsSnapshot()
        {
            var repo = new Repository<Track>();
            var t1 = new Track("A","X",TimeSpan.FromSeconds(1),2000);
            repo.Add(t1);
            var snapshot = repo.GetAll();
            var t2 = new Track("B","Y",TimeSpan.FromSeconds(2),2001);
            repo.Add(t2);
            Assert.DoesNotContain(t2, snapshot);
        }
    }

    public class PlayerTests
    {
        [Fact]
        public void Play_Null_Throws()
        {
            var p = new Player();
            Assert.Throws<ArgumentNullException>(() => p.Play(null!));
        }

        [Fact]
        public void Play_RaisesTrackChanged()
        {
            var p = new Player();
            var invoked = false;
            Track? received = null;
            p.TrackChanged += (s, t) => { invoked = true; received = t; };
            var t = new Track("A","X",TimeSpan.FromSeconds(3),2000);
            p.Play(t);
            Assert.True(invoked);
            Assert.Equal(t, received);
        }
    }

    public class TrackExtensionsExtraTests
    {
        [Fact]
        public void GetTotalDuration_Null_Throws()
        {
            IEnumerable<Track>? tracks = null;
            Assert.Throws<ArgumentNullException>(() => TrackExtensions.GetTotalDuration(tracks!));
        }

        [Fact]
        public void GetTotalDuration_HandlesNullElements()
        {
            var t = new Track("A","X",TimeSpan.FromSeconds(4),2000);
            Track?[] arr = new Track?[] { null, t };
            var total = ((IEnumerable<Track>)arr).GetTotalDuration();
            Assert.Equal(TimeSpan.FromSeconds(4), total);
        }
    }
}
