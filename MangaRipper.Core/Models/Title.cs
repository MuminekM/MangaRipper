using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaRipper.Core.Models
{
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
