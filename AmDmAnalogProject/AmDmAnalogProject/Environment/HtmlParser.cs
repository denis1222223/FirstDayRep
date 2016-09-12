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
            HtmlNode table = html.DocumentNode.SelectNodes("//table[@class='items']")[0];
            var rows = table.ChildNodes;
            rows.RemoveAt(0);

            foreach (var tr in rows)
            {
                var tdNodes = tr.ChildNodes.Where(x => x.Name == "td").ToArray();     
                string name = tdNodes[1].InnerText;
                int songsCount = Int32.Parse(tdNodes[2].InnerText);
                int viewsCount = Int32.Parse(tdNodes[3].InnerText.Replace(",",""));
                artists.Add(new Artist()
                {
                    Name = name,
                    SongsCount = songsCount,
                    ViewsCount = viewsCount
                });
            }
            db.Artists.AddRange(artists);
            db.SaveChanges();
        }

        private HtmlDocument GetHtmlDocument(string url)
        {
            HtmlDocument html = new HtmlDocument();
            WebClient webClient = new WebClient();
            webClient.Proxy = null;
            webClient.Encoding = encode;
            html.LoadHtml(webClient.DownloadString(url));
            return html;
        }
    }
}