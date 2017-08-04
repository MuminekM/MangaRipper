using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaRipper.Core.Models
{
    public class Site
    {
       // public SiteInformation Info { get; }
        public string Name { get;}
        public IEnumerable<Title> Titles { get; }
        public Site(string name, IEnumerable<Title> titles)
        {
            Name = name;
            Titles = titles;
        }

    }
}
