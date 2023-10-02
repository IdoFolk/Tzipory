namespace Tzipory.Tools
{
    [System.Serializable]
    public class Ticker
    {
        int _laps;
        int _maxTicks;
        int _currentTicks;

        public void Init(int max, int laps)
        {
            _maxTicks = max;
            _laps = laps;
        }

        public bool IsDone => _laps <= 0;

        /// <summary>
        /// Perfoms Tick by adding 1 to the current tick.
        /// Returns true if currentTick == maxTicks (and 0's currentTicks).
        /// DoTick is both a statement and a question
        /// </summary>
        /// <returns></returns>
        public bool DoTick()
        {
            _currentTicks++;
            if (_currentTicks == _maxTicks)
            {
                _currentTicks = 0;
                _laps--;
                return true;
            }

            return false;
        }
    }
}