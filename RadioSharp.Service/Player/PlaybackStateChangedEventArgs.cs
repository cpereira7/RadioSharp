using RadioSharp.Service.Models;

namespace RadioSharp.Service.Player
{
    public class PlaybackStateChangedEventArgs : EventArgs
    {
        public PlayerStatus Status { get; set; }
        public RadioStation RadioStation { get; }
        public string? Stream { get; }

        public PlaybackStateChangedEventArgs(RadioStation radioStation, string stream)
        {
            Status = PlayerStatus.Playing;
            RadioStation = radioStation;
            Stream = stream;
        }

        public PlaybackStateChangedEventArgs(RadioStation radioStation, PlayerStatus playerStatus)
        {
            Status = playerStatus;
            RadioStation = radioStation;
        }
    }

    public enum PlayerStatus
    {
        Playing,
        Error,
        InvalidStream
    }
}
