using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmDmAnalogProject.Models.Entities;

namespace AmDmAnalogProject.Models.Entities
{
    public class Song
    {
        public Song()
        {
            Chords = new List<Chord>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int ViewsCount { get; set; }
        public string PageUrl { get; set; }
        public virtual List<Chord> Chords { get; set; }
    }
}