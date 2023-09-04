namespace Chinook.Models
{
    public class PlaylistTracks
    {
        public long PlaylistId { get; set; }
        public long TrackId { get; set; }
        public Track Track { get; set; }
        public Playlist Playlist { get; set; }

    }
}
