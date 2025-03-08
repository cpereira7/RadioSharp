using RadioSharp.Service.Models;

namespace RadioSharp.Service.Player
{
    public interface IRadioPlayer
    {
        void PlayStream(RadioStation selectedRadio, CancellationToken cancellationToken);

        event EventHandler<PlaybackStateChangedEventArgs> PlaybackStateChanged;
    }
}
