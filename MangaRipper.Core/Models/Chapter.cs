using MangaRipper.Core.Extensions;

namespace MangaRipper.Core.Models
{
    public class Chapter
    {
        public string OriginalName { get; }
        public int Prefix { get; set; }
        public string Name => Prefix > 0 ? $"[{Prefix:000}] {OriginalName.RemoveFileNameInvalidChar()}" : OriginalName.RemoveFileNameInvalidChar();
        public string Url { get;}
        public Chapter(string originalName, string url)
        {
            OriginalName = originalName;
            Url = url;
        }
    }

    public class Title
    {
        public string OriginalName { get; }
        public string Url { get; }
        public string Latest { get; }
        public Title(string originalName, string url, string latest)
        {
            OriginalName = originalName;
            Url = url;
            Latest = latest;
        }
    }
}
