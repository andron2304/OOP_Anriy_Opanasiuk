using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicPlayer.Domain
{
    public static class DataManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

        public static async Task SaveAsync(List<Track> tracks, string filePath = "data.json")
        {
            if (tracks == null) throw new ArgumentNullException(nameof(tracks));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty");

            var trackDtos = ConvertToDto(tracks);
            var json = JsonSerializer.Serialize(trackDtos, JsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task<List<Track>> LoadAsync(string filePath = "data.json")
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty");
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}");

            var json = await File.ReadAllTextAsync(filePath);
            var trackDtos = JsonSerializer.Deserialize<List<TrackDto>>(json) ?? new List<TrackDto>();
            return ConvertFromDto(trackDtos);
        }

        private static List<TrackDto> ConvertToDto(List<Track> tracks)
        {
            var dtos = new List<TrackDto>();
            foreach (var track in tracks)
            {
                dtos.Add(new TrackDto
                {
                    Name = track.Name,
                    Artist = track.Artist,
                    DurationSeconds = (long)track.Duration.TotalSeconds,
                    Year = track.Year
                });
            }
            return dtos;
        }

        private static List<Track> ConvertFromDto(List<TrackDto> dtos)
        {
            var tracks = new List<Track>();
            foreach (var dto in dtos)
            {
                tracks.Add(new Track(dto.Name, dto.Artist, TimeSpan.FromSeconds(dto.DurationSeconds), dto.Year));
            }
            return tracks;
        }

        private class TrackDto
        {
            public string Name { get; set; } = string.Empty;
            public string Artist { get; set; } = string.Empty;
            public long DurationSeconds { get; set; }
            public int Year { get; set; }
        }
    }
}
