using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;

namespace HorribleSubsDownloader
{
  class Program
  {
    static void Main(string[] args)
    {
      //read last updated date from date file
      CultureInfo provider = CultureInfo.InvariantCulture;
      DateTime lastDate = DateTime.ParseExact(File.ReadAllText("Config\\date.txt"), "MM/dd/yy", provider);

      //read shows into a list
      List<string> shows = File.ReadAllLines("Config\\shows.txt").ToList();

      foreach (string show in shows)
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
            DateTime date = DateTime.ParseExact(thing.Value.Trim('(', ')'), "MM/dd/yy", provider);

            if (date >= lastDate)
            {
              Process.Start(magnetUrl.Value);
            }
          }
        }
      }
      File.WriteAllText("Config\\date.txt", DateTime.Now.ToString("MM/dd/yy").Replace('-', '/'));
    }
  }
}
