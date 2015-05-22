using System;
using System.IO;
using System.Net;
using System.Text;

namespace ExpertSystem.Logic
{
    public static class TranslateYa
    {
        const string Url =
                "https://translate.yandex.net/api/v1.5/tr/translate?key=trnsl.1.1.20141121T084319Z.baf98af8dbfbd466.611296bcbd54b8fa8215c237e78b9748749732b2";

        public static string TranslateInYandex(string text)
        {
            string textForOut = null;
            var strigBuilder = new StringBuilder();
            text = text.Replace("&", "%26");
            text = text.Replace(";", "");
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
    }
}