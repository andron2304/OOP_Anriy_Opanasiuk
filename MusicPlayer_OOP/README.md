# MusicPlayer OOP

A comprehensive music player application demonstrating advanced Object-Oriented Programming principles and design patterns.

## Project Description

MusicPlayer_OOP is a multi-layered music management system built with C# and .NET 8.0. It showcases professional software architecture through clean code principles, design patterns, and comprehensive testing.

## How to Run

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code with C# extension

### Build and Run
```bash
cd MusicPlayer_OOP
dotnet build
dotnet run --project MusicPlayer.App
```

### Run Tests
```bash
dotnet test
```

## Design Patterns Implemented

- **Factory Pattern** - `AudioEntityFactory` for creating audio entities
- **Strategy Pattern** - `ShuffleStrategy` and `RepeatStrategy` for playback modes
- **Composite Pattern** - `Playlist` and `IPlaylistComponent` for hierarchical structures
- **Iterator Pattern** - `PlaylistIterator` for traversing collections
- **Repository Pattern** - `Repository` for data access abstraction
- **Extension Methods** - `TrackExtensions` for domain extensions
- **Singleton/Static** - `DataManager` for centralized data persistence
