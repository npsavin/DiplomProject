using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ExpertSystem.Logic
{
    public class GetNormalFormPrecedent
    {
        private readonly string _textOfPrecedent;
        private const string UrlForNormalForm = "http://tools.k50project.ru/lemma/";

        public GetNormalFormPrecedent(string textOfPrecedent)
        {
            _textOfPrecedent = textOfPrecedent;
        }

        public string GetNormalForm()
        {
            
        	HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://kbyte.ru");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            return null;



        }

        private string GetFormForWebServise()
        {
            var wordsInPrecednt = _textOfPrecedent.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in wordsInPrecednt)
            {
                sb.AppendLine(word);
            }
            return sb.ToString();
        }
    }
}