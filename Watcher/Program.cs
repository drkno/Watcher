using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaFileParser.MediaTypes.TvFile.Tvdb;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace Watcher
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*
             * Use of TitleCleaner persistent cache to prevent duplicate and redundant requests to same series.
             * 
             * THIS API KEY IS FOR TITLE CLEANER ONLY.
             * NOT FOR USE IN A DEPLOY OR PUBLICALLY ACCESSABLE SITUATION
             */
            var tvdb = new Tvdb("F9D98CE470B5ABAE", TvdbCacheType.PersistentMemory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TitleCleaner"));

            Console.Write("Series:\t");
            var seriesName = Console.ReadLine();
            Console.Write("Season:\t");
            var season = uint.Parse(Console.ReadLine());
            Console.Write("Episode:\t");
            var episode = uint.Parse(Console.ReadLine());

            TvdbSeries[] series = tvdb.Search(seriesName);
            int sect = 0;
            foreach (var ser in series)
            {
                Console.WriteLine(sect++ + ".");
                Console.WriteLine("### " + ser.SeriesName + " ###");
                Console.WriteLine(ser.Description);
                Console.WriteLine("---");
            }

            uint sel = 0;
            if (series.Length != 1)
            {
                Console.Write("Which one? ");
                sel = uint.Parse(Console.ReadLine());
            }

            var tvdbWatcher = new TvdbTvWatcherTimer(series[sel].TvdbId, season, episode, tvdb, 21600);

            while (tvdbWatcher.Current() < DateTime.Now)
            {
                Console.WriteLine("[" + tvdbWatcher.CurrentSeason + "x" + tvdbWatcher.CurrentEpisode + "] " + tvdbWatcher.Current());
                tvdbWatcher.MoveNext();
            }

            Console.WriteLine("Waiting for [" + tvdbWatcher.CurrentSeason + "x" + tvdbWatcher.CurrentEpisode + "] at " + tvdbWatcher.Current());
            Console.ReadKey();
        }
    }
}
