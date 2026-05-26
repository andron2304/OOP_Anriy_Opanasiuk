using Xunit;
using MusicPlayer.Domain;
using System;
using System.Collections.Generic;

namespace MusicPlayer.Tests
{
    public class PlaylistTests
    {
        [Fact]
        public void AddTrackToPlaylist_ShouldAddTrackSuccessfully()
        {
            // Arrange
            var playlist = new Playlist("My Playlist");
            var track = new Track("Song", "Artist", TimeSpan.FromSeconds(120), 2020);

            // Act
            playlist.Add(track);

            // Assert
            Assert.Contains(track, playlist.Tracks);
        }
    }

    public class ShuffleStrategyTests
    {
        [Fact]
        public void GetNextTrack_ShouldReturnTrackFromPlaylist()
        {
            // Arrange
            var strategy = new ShuffleStrategy();
            var tracks = new List<Track>
            {
                new Track("Song1", "Artist1", TimeSpan.FromSeconds(120), 2020),
                new Track("Song2", "Artist2", TimeSpan.FromSeconds(180), 2021),
                new Track("Song3", "Artist3", TimeSpan.FromSeconds(150), 2022)
            };

            // Act
            var result = strategy.GetNextTrack(tracks);

            // Assert
            Assert.Contains(result, tracks);
        }
    }

    public class TrackExtensionsTests
    {
        [Fact]
        public void GetTotalDuration_ShouldCalculateCorrectTotalDuration()
        {
            // Arrange
            var tracks = new List<Track>
            {
                new Track("Song1", "Artist1", TimeSpan.FromSeconds(120), 2020),
                new Track("Song2", "Artist2", TimeSpan.FromSeconds(180), 2021)
            };
            var expectedDuration = TimeSpan.FromSeconds(300);

            // Act
            var totalDuration = tracks.GetTotalDuration();

            // Assert
            Assert.Equal(expectedDuration, totalDuration);
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void TrackEqualityTest()
        {
            var t1 = new Track("Song", "Artist", TimeSpan.FromSeconds(120), 2020);
            var t2 = new Track("Song", "Artist", TimeSpan.FromSeconds(120), 2020);

            Assert.NotEqual(t1, t2); // different Ids
            Assert.False(t1 == t2);
            Assert.True(t1 != t2);
        }
    }
}
