using System;
using System.Collections.Generic;
using MusicPlayer.Domain;

Console.WriteLine("Програма MusicPlayer\n");

// Singleton
var settings = PlayerSettings.Instance;
Console.WriteLine($"Сінглтон - Налаштування: Гучність: {settings.Volume}, Вимкнено: {settings.IsMuted}\n");

// Factory
var track1 = AudioEntityFactory.CreateTrack("Song 1", "Artist A", TimeSpan.FromMinutes(3), 2020);
var track2 = AudioEntityFactory.CreateTrack("Song 2", "Artist B", TimeSpan.FromMinutes(4), 2021);
var track3 = AudioEntityFactory.CreateTrack("Song 3", "Artist C", TimeSpan.FromMinutes(3.5), 2022);
var playlist = AudioEntityFactory.CreatePlaylist("My Playlist");

playlist.Add(track1);
playlist.Add(track2);
playlist.Add(track3);

Console.WriteLine("Фабрика - Створено треки та плейлист:");
Console.WriteLine(playlist);
Console.WriteLine();

// Composite
var subPlaylist = AudioEntityFactory.CreatePlaylist("Sub Playlist");
subPlaylist.Add(AudioEntityFactory.CreateTrack("SubTrack 1", "Artist D", TimeSpan.FromMinutes(2), 2023));
playlist.AddPlaylist(subPlaylist);
Console.WriteLine($"Композиція - Додано підплейлист: {subPlaylist.Name}\n");

// Iterator
Console.WriteLine("Ітератор - Перебір треків у плейлисті:");
foreach (var track in playlist)
{
    Console.WriteLine($"  - {track}");
}
Console.WriteLine();

// Strategy
var shuffleStrategy = new ShuffleStrategy();
var repeatStrategy = new RepeatStrategy();

Console.WriteLine("Стратегія - Отримання наступного треку зі стратегією Shuffle:");
var nextTrack = shuffleStrategy.GetNextTrack(new List<Track> { track1, track2, track3 });
Console.WriteLine($"  Random track: {nextTrack}\n");

// Observer with Event Subscription/Unsubscription
var player = new Player();

// Event handler
void OnTrackChanged(object? sender, Track track)
{
    Console.WriteLine($"Спостерігач - Подія спрацювала! Зараз грає: {track}");
}

// Subscribe
player.TrackChanged += OnTrackChanged;
Console.WriteLine("Спостерігач - Підписано на подію зміни треку\n");

// Play to trigger event
player.Play(track1);
player.Play(track2);

// Unsubscribe to prevent memory leak
player.TrackChanged -= OnTrackChanged;
Console.WriteLine("Спостерігач - Відписано від події зміни треку (запобігання витоку пам'яті)\n");

// Verify unsubscription - event won't fire
player.Play(track3);
Console.WriteLine("(Повідомлень про подію немає вище - відписка успішна)\n");

Console.WriteLine("Усі шаблони проектування успішно реалізовано!");

