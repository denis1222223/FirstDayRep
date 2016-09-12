using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmDmAnalogProject.Models.Entities
{
    public class Artist
    {
        public Artist()
        {
            Songs = new List<Song>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Biography { get; set; }
        public int SongsCount { get; set; }
        public int ViewsCount { get; set; }
        public string PageUrl { get; set; }
        public virtual List<Song> Songs { get; set; }
    }
}