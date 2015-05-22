using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExpertSystem.Logic
{
    public static class GetForm
    {
        private static readonly object Monitor=0;
        private const string Uri = "http://morphology.ru/?word=";

        public static string GetNormalForm(string word)
        {
            lock (Monitor)
            {
                var url = Uri + word;
                var sb = new StringBuilder();
                var buf = new byte[8192];
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var resStream = response.GetResponseStream();
                var count = 0;
                do
                {
                    if (resStream != null) count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                } while (count > 0);
                return ParseMainFormHtmlPage(sb.ToString());
            }
            

        }

        public static List<string> GetAllForm(string word)
        {
            lock (Monitor)
            {
                var url = Uri + word;
                var sb = new StringBuilder();
                var buf = new byte[8192];
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var resStream = response.GetResponseStream();
                var count = 0;
                do
                {
                    if (resStream != null) count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                } while (count > 0);
                var listForReturn  = ParseAllFormHtmlPage(sb.ToString());
                listForReturn.Add(word);
                listForReturn.Add(ParseMainFormHtmlPage(sb.ToString()));
                return listForReturn;
            }

            
        }

        private static List<string> ParseAllFormHtmlPage(string page)
        {

            var list = new List<string>();
            var match = Regex.Match(page, @"<(\s*li\s*)>(.*?)<\s*(\\|\/)\1>", RegexOptions.IgnoreCase);
            while (match.Success)
            {
                var text = match.Groups[2].Value;
                list.Add(text);
                match = match.NextMatch();
            }


            return list;

        }

        private static string ParseMainFormHtmlPage(string page)
        {
            var timePage =  page.Substring(page.IndexOf("<div class=\"base\"", StringComparison.Ordinal) + 16, page.Length - page.IndexOf("<div class=\"base\"", StringComparison.Ordinal) - 17);


            return
               timePage.Substring(timePage.IndexOf(">", StringComparison.Ordinal) + 1,
                    timePage.IndexOf("<", StringComparison.Ordinal) - 2);

        }

      
    }
}