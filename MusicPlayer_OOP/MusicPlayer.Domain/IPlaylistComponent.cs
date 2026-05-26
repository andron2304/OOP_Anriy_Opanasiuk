namespace MusicPlayer.Domain
{
    public interface IPlaylistComponent
    {
        Guid Id { get; }
        string Name { get; }
    }
}
