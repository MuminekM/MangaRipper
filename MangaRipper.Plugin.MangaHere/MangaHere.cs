using MangaRipper.Core.Helpers;
using MangaRipper.Core.Interfaces;
using MangaRipper.Core.Models;
using MangaRipper.Core.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MangaRipper.Plugin.MangaHere
{
    /*a href="http://www.mangahere.co/manga/baka_to_test_to_shoukanjuu_dya/" class="manga_info name_one" rel="Baka to Test to Shoukanjuu‎ Dya">Baka to Test to Shoukanjuu‎ Dya</a>
				Latest Updated: <a class="name_two" href="http://www.mangahere.co/manga/baka_to_test_to_shoukanjuu_dya/c003/">Ch.3</a>*/
    /// <summary>
    /// Support find chapters and images from MangaHere
    /// </summary>
    public class MangaHere : MangaService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override async Task<IEnumerable<Title>> FindTitles(string keyword, CancellationToken cancellationToken)
        {
            var downloader = new DownloadService();
            var parser = new ParserHelper();
            var titles = new List<Title>();
            string input = await downloader.DownloadStringAsync("http://www.mangahere.co/search.php?name=" + keyword, cancellationToken);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);

            var htmlTitles = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"result_search\"]/dl/dt");
            foreach (HtmlNode item in htmlTitles)
            {
                var node = item.SelectSingleNode(".//a[@class=\"manga_info name_one\"]");
                string url = node.Attributes["href"].Value;
                string name = node.InnerText;
                string latest = item.SelectSingleNode(".//a[@class=\"name_two\"]").InnerText;
                Title title = new Title(name, url, latest);
                titles.Add(title);
                //Title title = new Title() string(/*/book[1]/title/@lang)
            }
            //var titles = parser.Parse("<a class=\"color_0077\" href=\"(?<Value>http://[^\"]+)\"[^<]+>(?<Name>[^<]+)</a>", input, "Name");
            return titles;
        }

        public override async Task<IEnumerable<Chapter>> FindChapters(string manga, IProgress<int> progress, CancellationToken cancellationToken)
        {
            var downloader = new DownloadService();
            var parser = new ParserHelper();
            progress.Report(0);
            // find all chapters in a manga
            string input = await downloader.DownloadStringAsync(manga, cancellationToken);
            var chaps = parser.ParseGroup("<a class=\"color_0077\" href=\"(?<Value>http://[^\"]+)\"[^<]+>(?<Name>[^<]+)</a>", input, "Name", "Value");
            progress.Report(100);
            return chaps;
        }
        
        public override async Task<IEnumerable<string>> FindImages(Chapter chapter, IProgress<int> progress, CancellationToken cancellationToken)
        {
            var downloader = new DownloadService();
            var parser = new ParserHelper();

            // find all pages in a chapter
            string input = await downloader.DownloadStringAsync(chapter.Url, cancellationToken);
            var pages = parser.Parse(@"<option value=""(?<Value>[^""]+)"" (|selected=""selected"")>\d+</option>", input, "Value");

            // transform pages link
            pages = pages.Select(p =>
            {
                var value = new Uri(new Uri(chapter.Url), p).AbsoluteUri;
                return value;
            }).ToList();

            // find all images in pages
            var pageData = await downloader.DownloadStringAsync(pages, new Progress<int>((count) =>
            {
                var f = (float)count / pages.Count();
                int i = Convert.ToInt32(f * 100);
                progress.Report(i);
            }), cancellationToken);
            var images = parser.Parse("<img src=\"(?<Value>[^\"]+)\" onload=", pageData, "Value");

            return images;
        }

        public override SiteInformation GetInformation()
        {
            return new SiteInformation(nameof(MangaHere), "http://www.mangahere.co", "English");
        }

        public override bool Of(string link)
        {
            var uri = new Uri(link);
            return uri.Host.Equals("www.mangahere.co");
        }
    }
}
