using NAudio.Wave;
using RadioSharp.Service.Models;

namespace RadioSharp.Service.Player
{
    public class RadioPlayer : IRadioPlayer
    {
        public event EventHandler<PlaybackStateChangedEventArgs>? PlaybackStateChanged;

        public void PlayStream(RadioStation selectedRadio, CancellationToken cancellationToken)
        {
            int streamIndex = 0;

            while (!cancellationToken.IsCancellationRequested && streamIndex < selectedRadio.Streams.Length)
            {
                try
                {
                    var url = selectedRadio.Streams[streamIndex];

                    using var mf = new MediaFoundationReader(url);
                    using var wo = new WasapiOut();

                    wo.Init(mf);
                    wo.Play();

                    PlaybackStateChanged?.Invoke(this, new PlaybackStateChangedEventArgs(selectedRadio, url));
                }
                catch
                {
                    PlaybackStateChanged?.Invoke(this, new PlaybackStateChangedEventArgs(selectedRadio, PlayerStatus.InvalidStream));
                }

                streamIndex++;
            }

            if (!cancellationToken.IsCancellationRequested)
                PlaybackStateChanged?.Invoke(this, new PlaybackStateChangedEventArgs(selectedRadio, PlayerStatus.Error));
        }
    }
}
