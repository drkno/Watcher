using System;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

namespace Watcher
{
    public class TvdbTvWatcherTimer : WatcherTimer
    {
        private readonly uint _tvdbId;
        private uint _season, _episode;
        private readonly Tvdb _tvdb;
        private TvdbDetailedSeries _seriesInfo;
        private TvdbEpisode _currentEpisode;
        public TvdbTvWatcherTimer(uint tvdbId, uint startSeason, uint startEpisode, Tvdb tvdbInstance, uint fallbackWaitTime)
            : base(fallbackWaitTime)
        {
            _tvdbId = tvdbId;
            _season = startSeason;
            _episode = --startEpisode;
            _tvdb = tvdbInstance;
            NextEpisode();
        }

        public uint CurrentSeason { get { return _season; } }
        public uint CurrentEpisode { get { return _episode; } }

        public new DateTime Current()
        {
            return _currentEpisode == null ? base.Current() : _currentEpisode.FirstAiredDate;
        }

        public new DateTime MoveNext()
        {
            NextEpisode();
            if (_currentEpisode == null) base.MoveNext();
            return Current();
        }

        private void NextEpisode()
        {
            if (_tvdb.UpdateCache() || _seriesInfo == null)
            {
                _seriesInfo = _tvdb.LookupId(_tvdbId);
            }
            _episode++;
            _currentEpisode = _seriesInfo.GetEpisode(_season, _episode);
            if (_currentEpisode != null) return;
            _currentEpisode = _seriesInfo.GetEpisode(_season + 1, 1);
            if (_currentEpisode != null)
            {
                _season++;
                _episode = 1;
            }
            else
            {
                _episode--;
            }
        }
    }
}
