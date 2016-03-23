using Data.Interfaces;
using Data.Models;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Repositories
{
    public class HorribleSubsRepository : ITorrentRepository
    {
        public Torrents GetTorrents(Configuration config)
        {
            Torrents torrents = new Torrents();

            foreach (string show in config.Shows)
            {
                //fetching search results html for each show in the list
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

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlText);
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class=\"episode\"]"))
                    {
                        var magnetUrl = node.LastChild.FirstChild.LastChild.ChildNodes[2].FirstChild.Attributes.First();
                        string pattern = @"\((.+)\)";
                        var thing = Regex.Match(node.FirstChild.InnerText, pattern);
                        DateTime date = DateTime.ParseExact(thing.Value.Trim('(', ')'), "MM/dd/yy", config.Provider);

                        if (date >= config.Date)
                        {
                            torrents.TorrentFileLinks.Add(magnetUrl.Value);
                        }
                    }
                }
            }

            return torrents;
        }

    }
}
