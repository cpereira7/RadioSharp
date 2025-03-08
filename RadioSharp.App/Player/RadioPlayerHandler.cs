using RadioSharp.App.Menus;
using RadioSharp.Service.Models;
using RadioSharp.Service.Player;
using RadioSharp.Service.Stations;
using System.Diagnostics;

namespace RadioSharp.App.Player
{
    public class RadioPlayerHandler : IRadioPlayerHandler
    {
        private readonly IRadioStationsService _radioStationsHandler;
        private readonly IRadioPlayer _radioPlayer;

        private CancellationTokenSource? _stopPlaying;
        private int radioIndex;

        public RadioPlayerHandler(IRadioStationsService radioStationsHandler, IRadioPlayer radioPlayer)
        {
            _radioStationsHandler = radioStationsHandler;
            _radioPlayer = radioPlayer;

            _radioPlayer.PlaybackStateChanged += RadioPlayer_PlaybackStateChanged;
        }

        private void RadioPlayer_PlaybackStateChanged(object? sender, PlaybackStateChangedEventArgs e)
        {
            if (e.Status == PlayerStatus.Playing)
                DrawAnimation(e.RadioStation, e.Stream ?? "");

            if (e.Status == PlayerStatus.Error)
                ConsoleHelpers.DisplayMessageWithDelay($"No valid streams found for: {e.RadioStation.Name}", 4000);
        }

        public void PlayStream(RadioStation selectedRadio, int choice)
        {
            _stopPlaying = new CancellationTokenSource();

            _radioStationsHandler.SaveLastPlayedRadio(selectedRadio);
            radioIndex = choice;

            _radioPlayer.PlayStream(selectedRadio, _stopPlaying.Token);
        }

        private void DrawAnimation(RadioStation radio, string url)
        {
            Console.WriteLine(" ( Press any key to ■ )\n\n");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                if (Console.KeyAvailable && !VolumeKeyPressed())
                {
                    _stopPlaying?.Cancel();
                    break;
                }

                var time = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";
                
                ConsoleHelpers.WriteMessageWithDelay($"\r ► {radioIndex}. {radio.Name} ({url}) ({time})", 500);
                Console.Title = $" ► {radio.Name} ({time})";
            }

            stopwatch.Stop();
        }

        private static bool VolumeKeyPressed()
        {
            var keyInfo = Console.ReadKey(intercept: true);

            return keyInfo.Key == ConsoleKey.VolumeUp || keyInfo.Key == ConsoleKey.VolumeDown;
        }
    }
}
