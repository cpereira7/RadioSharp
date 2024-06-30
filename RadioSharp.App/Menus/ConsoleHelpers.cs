namespace RadioSharp.App.Menus
{
    public static class ConsoleHelpers
    {
        public static void DisplayMessageWithDelay(string message, int delayMilliseconds)
        {
            Console.WriteLine(message);
            Thread.Sleep(delayMilliseconds);
        }

        public static void WriteMessageWithDelay(string message, int delayMilliseconds)
        {
            Console.Write(message);
            Thread.Sleep(delayMilliseconds);
        }
    }
}
