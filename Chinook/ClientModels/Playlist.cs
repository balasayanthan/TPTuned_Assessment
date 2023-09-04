namespace Chinook.ClientModels;

public class Playlist
{
    public string Name { get; set; }
    public List<PlaylistTrackDto> Tracks { get; set; }
}