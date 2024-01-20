namespace SaveGame
{
    public class Utility
    {
        private static Timer? _timer;
        public static void Debounce(Action action, int millisecondsDelay)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer = new Timer(state =>
            {
                action.Invoke();
            }, null, millisecondsDelay, Timeout.Infinite);
        }

        public static int GetOffset()
        {
            Random random = new();
            return random.Next(0, 21) * 5;
        }

        public static string TimeOfDay()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 18)
                return "Evening";
            else if (hour >= 12)
                return "Afternoon";
            else
                return "Morning";
        }
    }
}
