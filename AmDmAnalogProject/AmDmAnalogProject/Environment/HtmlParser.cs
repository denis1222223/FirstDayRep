using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmDmAnalogProject.Models;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using AmDmAnalogProject.Models.Entities;

namespace AmDmAnalogProject.Environment
{
    public class HtmlParser
    {
        public static Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

        public void Parse(ApplicationDbContext db, string url)
        {
            List<Artist> artists = new List<Artist>();
            HtmlDocument html = GetHtmlDocument(url);
            artists = ParseArtists(html);

            db.Artists.AddRange(artists);
            db.SaveChanges();
        }

        private List<Artist> ParseArtists(HtmlDocument html)
        {
            List<Artist> artists = new List<Artist>();

            HtmlNode table = html.DocumentNode.SelectNodes("//table[@class='items']")[0];
            var rows = table.ChildNodes;
            rows.RemoveAt(0);

            foreach (var tr in rows)
            {
                var tdNodes = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                string name = tdNodes[1].InnerText;
                int songsCount = Int32.Parse(tdNodes[2].InnerText);
                int viewsCount = Int32.Parse(tdNodes[3].InnerText.Replace(",", ""));
                string url = "http:" + tdNodes[1].SelectSingleNode("a").GetAttributeValue("href", "");
                Artist currentArtist = new Artist()
                {
                    Name = name,
                    SongsCount = songsCount,
                    ViewsCount = viewsCount,
                    PageUrl = url
                };
                ParseArtistPage(currentArtist, url);
                artists.Add(currentArtist);
            }
            return artists;
        }

        private void ParseArtistPage(Artist currentArtist, string url)
        {
            HtmlDocument html = GetHtmlDocument(url);
            HtmlNode biographyNode = html.DocumentNode.SelectSingleNode("//div[@class='artist-profile__bio']");
            string biography = biographyNode.InnerText;
            HtmlNode pictureNode = html.DocumentNode.SelectSingleNode("//div[@class='artist-profile__photo debug1']");
            string pictureUrl = pictureNode.SelectSingleNode("img").GetAttributeValue("src", "");
            currentArtist.Biography = biography;
            currentArtist.PictureUrl = pictureUrl;
            ParseSongs(currentArtist, html);
        }

        private void ParseSongs(Artist currentArtist, HtmlDocument html)
        {
            List<Song> songs = new List<Song>();
            var songsNodes = html.GetElementbyId("tablesort").ChildNodes;
            songsNodes.RemoveAt(0);
            foreach (var tr in songsNodes)
            {
                var tdNodes = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                string name = tdNodes[0].InnerText;
                string url = "http:" + tdNodes[0].SelectSingleNode("a").GetAttributeValue("href", "");
                int viewsCount = Int32.Parse(tdNodes[2].InnerText.Replace(",", ""));

                Song currentSong = new Song()
                {
                    Name = name,
                    ViewsCount = viewsCount,
                    PageUrl = url
                };
                ParseSongPage(currentSong, url);
                songs.Add(currentSong);
            }
            currentArtist.Songs.AddRange(songs);
        }

        private void ParseSongPage(Song currentSong, string url)
        {
            HtmlDocument html = GetHtmlDocument(url);
            HtmlNode textNode = html.DocumentNode.SelectSingleNode("//div[@class='b-podbor__text']");
            string text = textNode.InnerText;
            currentSong.Text = text;
            ParseChords(currentSong, html);
        }

        private void ParseChords(Song currentSong, HtmlDocument html)
        {
            List<Chord> chords = new List<Chord>();
            var chordNodes = html.GetElementbyId("song_chords").ChildNodes;
            foreach (var img in chordNodes)
            {
                string url = img.GetAttributeValue("src", "");
                string name = img.GetAttributeValue("alt", "");
                chords.Add(new Chord()
                {
                    ChordName = name,
                    PictureUrl = url
                });
            }
            currentSong.Chords.AddRange(chords);
        }

        private HtmlDocument GetHtmlDocument(string url)
        {
            HtmlDocument html = new HtmlDocument();
            System.Threading.Thread.Sleep(500);
            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                webClient.Encoding = encode;
                html.LoadHtml(webClient.DownloadString(url));
            }
            return html;
        }
    }
}