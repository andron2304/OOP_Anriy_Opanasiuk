using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain;

public static class TrackExtensions
{
    public static TimeSpan GetTotalDuration(this IEnumerable<Track> tracks)
    {
        if (tracks is null) throw new ArgumentNullException(nameof(tracks));
        return tracks.Aggregate(TimeSpan.Zero, (acc, t) => acc + (t?.Duration ?? TimeSpan.Zero));
    }
}
