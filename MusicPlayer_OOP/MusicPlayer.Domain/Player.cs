using System;

namespace MusicPlayer.Domain
{
    public class Player
    {
        public event EventHandler<Track>? TrackChanged;

        public void Play(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            track.Play();
            OnTrackChanged(track);
        }

        protected virtual void OnTrackChanged(Track track)
        {
            TrackChanged?.Invoke(this, track);
        }
    }
}
