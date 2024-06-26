using NAudio.Wave;
using RadioSharp.App.Helpers;
using RadioSharp.App.Models;
using System.Diagnostics;

namespace RadioSharp.App.Player
{
    internal class RadioPlayer
    {
        protected RadioPlayer() { }

        public static void PlayStream(RadioStation selectedRadio, int choice)
        {
            int streamIndex = 0;
            bool streamPlayed = false;

            while (!streamPlayed && streamIndex < selectedRadio.Streams.Length)
            {
                try
                {
                    var url = selectedRadio.Streams[streamIndex];

                    using var mf = new MediaFoundationReader(url);
                    using var wo = new WasapiOut();

                    wo.Init(mf);
                    wo.Play();

                    while (wo.PlaybackState == PlaybackState.Playing && !Console.KeyAvailable)
                    {
                        DrawAnimation(selectedRadio, choice, url);
                        Task.Delay(100).Wait();
                        wo.Stop();
                    }

                    streamPlayed = true;
                }
                catch
                {
                    Console.WriteLine($"Error trying to play one of the streams.\n");
                }

                streamIndex++;
            }

            if (!streamPlayed)
            {
                ConsoleHelpers.DisplayMessageWithDelay($"No valid streams found for: {selectedRadio.Name}", 4000);
            }
        }

        private static void DrawAnimation(RadioStation radio, int index, string url)
        {
            Console.WriteLine(" ( Press any key to ■ )\n\n");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                if (Console.KeyAvailable && !VolumeKeyPressed())
                {
                    break;
                }

                var time = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";
                ConsoleHelpers.WriteMessageWithDelay($"\r ► {index}. {radio.Name} ({url}) ({time})", 500);
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
