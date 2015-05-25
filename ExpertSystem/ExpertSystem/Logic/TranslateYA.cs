using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using xNet.Text;

namespace ExpertSystem.Logic
{
    public static class TranslateYa
    {
        const string Url =
                "https://translate.yandex.net/api/v1.5/tr/translate?key=trnsl.1.1.20141121T084319Z.baf98af8dbfbd466.611296bcbd54b8fa8215c237e78b9748749732b2";


        public static string TranslateInYandex(string textOfPrecedent)
        {
            string textForOut = null;
            var text = textOfPrecedent.Substring(0, Math.Min(9000, textOfPrecedent.Length));
            var strigBuilder = new StringBuilder();
            
            
            text = text.Replace("&", "");
            text = text.Replace(";", "");
            text = text.Replace("*", "");
            text = text.Replace("#", "");
            if (text.Contains("http") && text.Contains("https"))
            {
                var fordelete = text.Substrings("http://", "\n");
                var fordelete2 = text.Substrings("https://", "\n");
                text = fordelete.Aggregate(text, (current, el) => current.Replace("http://" + el, ""));
                text = fordelete2.Aggregate(text, (current, el) => current.Replace("https://" + el, ""));
            }
            strigBuilder.Append("&text=" + text);
            strigBuilder.Append("&lang=en-ru");
            strigBuilder.Append("&format=json");


            var req = WebRequest.Create(Url + strigBuilder);
            var resp = req.GetResponse();
            var stream = resp.GetResponseStream();
            if (stream != null)
            {
                var sr = new StreamReader(stream);
                var Out = sr.ReadToEnd();
                sr.Close();
                if (Out.Contains("<text>") && Out.Contains("</text>"))
                {
                    textForOut = Out.Substring(Out.IndexOf("<text>", StringComparison.Ordinal) + 6,
                        (Out.IndexOf("</text>", StringComparison.Ordinal) - Out.IndexOf("<text>", StringComparison.Ordinal) - 6));
                }
            }
            return textForOut;
        }
        public static string TranslateInRu(string textOfPrecedent)
        {
            var text = textOfPrecedent.Substring(0, Math.Min(9000, textOfPrecedent.Length));
            string textForOut = null;
            var strigBuilder = new StringBuilder();
            text = text.Replace("&", "%26");
            text = text.Replace(";", "");
            strigBuilder.Append("&text=" + text);
            strigBuilder.Append("&lang=ru");
            strigBuilder.Append("&format=json");


            var req = WebRequest.Create(Url + strigBuilder);
            var resp = req.GetResponse();
            var stream = resp.GetResponseStream();
            if (stream != null)
            {
                var sr = new StreamReader(stream);
                var Out = sr.ReadToEnd();
                sr.Close();
                if (Out.Contains("<text>") && Out.Contains("</text>"))
                {
                    textForOut = Out.Substring(Out.IndexOf("<text>", StringComparison.Ordinal) + 6,
                        (Out.IndexOf("</text>", StringComparison.Ordinal) - Out.IndexOf("<text>", StringComparison.Ordinal) - 6));
                }
            }
            return textForOut;
        }
    }
}