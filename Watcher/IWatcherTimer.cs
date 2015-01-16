using System;

namespace Watcher
{
    public interface IWatcherTimer
    {
        DateTime Current();
        DateTime MoveNext();
    }
}