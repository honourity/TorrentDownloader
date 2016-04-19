using Data.Interfaces;
using Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Repositories
{
    public class HorribleSubsRepository : ITorrentRepository
    {
        private Configuration _config;

        public TorrentSource GetTorrents(Configuration config)
        {
            this._config = config;
            TorrentSource torrentData = new TorrentSource();
            torrentData.TorrentItems = new List<TorrentItem>();

            torrentData.Name = "HorribleSubs";
            torrentData.DateRetrieved = DateTime.Now;

            foreach (string show in config.Shows)
            {
                var item = GetTorrentItemByShowName(show);
                torrentData.TorrentItems.Add(item);
            }

            return torrentData;
        }

        private HtmlDocument GetHtmlByShowName(string show)
        {
            HtmlDocument doc = null;

            //fetching search results html
            string url = @"http://horriblesubs.info/lib/search.php?value=" + Uri.EscapeDataString(show);
            string htmlText = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                htmlText = readStream.ReadToEnd();
                response.Close();
                readStream.Close();

                doc = new HtmlDocument();
                doc.LoadHtml(htmlText);
            }

            return doc;
        }

        private TorrentItem GetTorrentItemByShowName(string show)
        {
            //replace this implementation
            //HtmlAgilityPack
            //sizzle selectors
            //use jquery
            //worst case, use xpath

            var item = new TorrentItem();
            item.Links = new Dictionary<string, string>();

            var html = GetHtmlByShowName(show);

            var tables = html.DocumentNode.Descendants("table").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("release-info"));

            foreach(HtmlNode table in tables)
            {
                //<tr> where exists attribute "id"
                //store id as episode name
                //class="release-links naruto-shippuuden-455-480p"

                string episodeName = table.FirstChild.FirstChild;
                table.Attributes
            }

            foreach (HtmlNode node in html.DocumentNode.SelectNodes("//div[@class=\"episode\"]"))
            {
                var magnetUrl = node.LastChild.FirstChild.LastChild.ChildNodes[2].FirstChild.Attributes.First();
                string pattern = @"\((.+)\)";
                var thing = Regex.Match(node.FirstChild.InnerText, pattern);
                DateTime date = DateTime.ParseExact(thing.Value.Trim('(', ')'), "MM/dd/yy", _config.Provider);

                if (date >= _config.Date)
                {
                    item.Links.Add();
                    torrents.TorrentFileLinks.Add(magnetUrl.Value);
                }
            }

            return item;
        }
    }
}
