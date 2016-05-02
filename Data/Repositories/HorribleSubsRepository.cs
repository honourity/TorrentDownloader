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
                
                torrentData.TorrentItems.Add(GetTorrentsByHorribleSubsOutput(item));
            }

            return torrentData;
        }

        private TorrentItem GetTorrentsByHorribleSubsOutput(HorribleSubsOutput data)
        {
            TorrentSource output = new TorrentItem();

            output.DateCreated = data.

            return output;
        }

        private HtmlDocument GetHtmlByShowName(string show)
        {
            HtmlDocument doc = null;

            //fetching search results html
            string url = @"http://horriblesubs.info/lib/search.php?value=" + Uri.EscapeDataString(show);
            string htmlText = File.ReadAllText(@"C:\Users\jtcgreyfox\Desktop\search.php.html");
            //string htmlText = null;

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

        private HorribleSubsOutput GetTorrentItemByShowName(string show)
        {
            HorribleSubsOutput output = new HorribleSubsOutput();
            output.DateRetrived = DateTime.Now;
            output.InputParams = show;

            output.Items = new List<HorribleSubsItem>();

            var html = GetHtmlByShowName(show);

            //get episode name from:
            //table class="release-info"
            //   tr id =[episode name]
            List<string> episodeNameList = new List<string>();

            var episodeNamesTable = html.DocumentNode.Descendants("table").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("release-info"));
            foreach(HtmlNode row in episodeNamesTable)
            {
                var episodeName = row.Descendants("tr").First().Attributes["id"].Value;
                char[] splitters = new char[2];
                splitters[0] = '(';
                splitters[1] = ')';                
                episodeNameList.Add(episodeName);

                HorribleSubsItem episodeItem = new HorribleSubsItem();
                episodeItem.Name = episodeName;
                episodeItem.DateCreated = DateTime.ParseExact(row.Descendants("tr").First().InnerText.Split(splitters)[1], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);

                var episodeLinkNodes = html.DocumentNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("release-links " + episodeName));
                foreach (HtmlNode node in episodeLinkNodes)
                {
                    var episodeFormatSize = node.Attributes["class"].Value.Replace("release-links " + episodeName + "-", "");
                    var episodeDownloadLink = node.Descendants().Where(a => a.Attributes.Contains("href") && a.Attributes["href"].Value.Contains("www.nyaa.se")).First().Attributes["href"].Value;

                    episodeItem.Links.Add(episodeFormatSize, episodeDownloadLink);
                }

                output.Items.Add(episodeItem);
            }

            ////foreach (string episode in episodeNameList)
            ////{
                
            ////}

            ////get list of all:
            ////div class="release-links"

            ////get:
            ////subset of list, where div class="[episode name]-[size]"

            ////get:
            ////table class="release-table"
            ////tbody
            ////tr
            ////td class="dl-type"
            ////span class="dl-link"
            ////a href


            ////var tables = html.DocumentNode.Descendants("table").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("release-info"));

            ////foreach(HtmlNode table in tables)
            ////{
            ////    var episodeChildren = table.Descendants("tr").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("release-table"));

            ////    //<tr> where exists attribute "id"
            ////    //store id as episode name
            ////    //class="release-links naruto-shippuuden-455-480p"

            ////    //string episodeName = table.FirstChild.FirstChild;
            ////    //table.Attributes
            ////}

            ////foreach (HtmlNode node in html.DocumentNode.SelectNodes("//div[@class=\"episode\"]"))
            ////{
            ////    var magnetUrl = node.LastChild.FirstChild.LastChild.ChildNodes[2].FirstChild.Attributes.First();
            ////    string pattern = @"\((.+)\)";
            ////    var thing = Regex.Match(node.FirstChild.InnerText, pattern);
            ////    DateTime date = DateTime.ParseExact(thing.Value.Trim('(', ')'), "MM/dd/yy", _config.Provider);

            ////    if (date >= _config.Date)
            ////    {
            ////        //item.Links.Add();
            ////        //torrents.TorrentFileLinks.Add(magnetUrl.Value);
            ////    }
            ////}

            return output;
        }
    }
}
