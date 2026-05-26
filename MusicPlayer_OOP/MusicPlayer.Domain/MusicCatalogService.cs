using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain;

public class MusicCatalogService
{
    private readonly List<Track> _tracks = new();
    private readonly Dictionary<string, Track> _lookup = new(StringComparer.OrdinalIgnoreCase);

    public void AddTrack(Track track)
    {
        if (track is null) throw new ArgumentNullException(nameof(track));
        _tracks.Add(track);
        _lookup[track.Id.ToString()] = track;
        _lookup[track.Name] = track;
    }

    public bool RemoveTrack(Track track)
    {
        if (track is null) throw new ArgumentNullException(nameof(track));
        var removed = _tracks.Remove(track);
        _lookup.Remove(track.Id.ToString());
        _lookup.Remove(track.Name);
        return removed;
    }

    public IReadOnlyList<Track> GetAllTracks() => _tracks.ToList().AsReadOnly();

    public Track? GetById(string id) => id is null ? null : (_lookup.TryGetValue(id, out var t) ? t : null);
    public Track? GetByName(string name) => name is null ? null : (_lookup.TryGetValue(name, out var t) ? t : null);

    // Complex LINQ: GroupBy + Join to find tracks by a specific artist
    public IReadOnlyList<Track> FindTracksByArtist(string artist)
    {
        if (string.IsNullOrWhiteSpace(artist)) return Array.Empty<Track>();

        var grouped = _tracks.GroupBy(t => t.Artist);

        var artistGroups = grouped.Where(g => string.Equals(g.Key, artist, StringComparison.OrdinalIgnoreCase));

        var result = artistGroups
            .SelectMany(g => g.Join(_tracks,
                                    gTrack => gTrack.Id,
                                    t => t.Id,
                                    (gTrack, t) => t))
            .Distinct()
            .ToList();

        return result.AsReadOnly();
    }

    // Sort tracks by duration (ascending or descending)
    public IReadOnlyList<Track> GetTracksSortedByDuration(bool ascending = true)
    {
        var q = ascending ? _tracks.OrderBy(t => t.Duration) : _tracks.OrderByDescending(t => t.Duration);
        return q.ToList().AsReadOnly();
    }
}
