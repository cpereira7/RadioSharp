using NAudio.Wave;
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

            while (!streamPlayed && streamIndex < selectedRadio.Streams.Count())
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
                Console.WriteLine($"No valid streams found for: {selectedRadio.Name}");
                Thread.Sleep(4000);
            }
        }

        private static void DrawAnimation(RadioStation radio, int index, string url)
        {
            Console.WriteLine(" ( Press any key to ■ )\n\n");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!Console.KeyAvailable)
            {
                var time = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";
                Console.Write($"\r ► {index}. {radio.Name} ({url}) ({time})");
                Console.Title = $" ► {radio.Name} ({time})";
                Thread.Sleep(500);
            }

            stopwatch.Stop();
        }
    }
}
