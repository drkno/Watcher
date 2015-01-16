using System;

namespace Watcher
{
    public class WatcherTimer : IWatcherTimer
    {
        private readonly uint _waitPeriod;
        private DateTime _nextWaitTime;

        public WatcherTimer(uint waitPeriod)
        {
            _waitPeriod = waitPeriod;
            MoveNext();
        }

        public DateTime Current()
        {
            return DateTime.Now > _nextWaitTime ? MoveNext() : _nextWaitTime;
        }

        public DateTime MoveNext()
        {
            _nextWaitTime = DateTime.Now.AddSeconds(_waitPeriod);
            return _nextWaitTime;
        }
    }
}
