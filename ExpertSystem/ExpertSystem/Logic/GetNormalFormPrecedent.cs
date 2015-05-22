using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Ajax.Utilities;
using xNet.Text;

namespace ExpertSystem.Logic
{
    public class GetNormalFormPrecedent
    {
        private readonly string _textOfPrecedent;
        private const string UrlForNormalForm = "http://tools.k50project.ru/lemma/lemmatize.php";

        public GetNormalFormPrecedent(string textOfPrecedent)
        {
            _textOfPrecedent = textOfPrecedent;
        }

        public List<PrecedentFormVector> GetNormalForm()
        {
            var stopWords =
                MyLibForNeo4J.GetCollectionByType("StopWord");
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["forms"] = _textOfPrecedent;

                var response = client.UploadValues(UrlForNormalForm, values);

                var responseString = Encoding.UTF8.GetString(response);
                var forReturn1 = responseString.Substring(responseString.IndexOf("<th>доля</th>"),
                    responseString.Length - responseString.IndexOf("<th>доля</th>"));
                var forReturn2 = forReturn1.Substring(forReturn1.IndexOf("</tr>"),
                    forReturn1.IndexOf("</table>"));

                var a = forReturn2.Substrings("<td>", "</td>");
                var i = 0;
                var ListWithCount = new List<PrecedentFormVector>();
                string timeName = null;
                //var TimePrecedent = new PrecedentFormVector();
                foreach (var el in a)
                {
                    if (i == 0 || i%3 == 0)
                    {
                        timeName = el;
                    }
                    if (i - 1 == 0 || (i - 1)%3 == 0)
                    {
                        var timeWeight = Convert.ToInt32(el);
                        bool flag = !el.IsNullOrWhiteSpace();
                        foreach (var stopword in stopWords)
                        {
                            if (stopword == timeName.ToLower())
                            {
                                flag = false;
                            }
                            if (flag)
                            {
                                ListWithCount.Add(new PrecedentFormVector() { name = timeName.ToLower(), weight = timeWeight });
                            }
                        }
                        
                    }
                    i++;
                }
                var listWithTf = MakeTF(ListWithCount);
                var listWithTFIDF = MakeTFIDF(listWithTf);
                return listWithTFIDF;
            }
        }

        public List<PrecedentFormVector> NormalFormAfterExpertEdit(string keyWordOfExpert, List<PrecedentFormVector> analitic)
        {
            var keyWords = keyWordOfExpert.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            var vectorDictionary = analitic.ToDictionary(vector => vector.name, vector => vector.weight);
            var sortDictionary = vectorDictionary.Where(x => x.Value == vectorDictionary.Values.Max());
            var maxWeith = sortDictionary.First().Value;
            var listForReturn = keyWords.Where(keyWord => !vectorDictionary.ContainsKey(keyWord.ToLower())).Select(keyWord => new PrecedentFormVector() {name = keyWord.ToLower(), weight = maxWeith}).ToList();
            for (var i = 0; i <= Math.Min(analitic.Count, 10); i++)
            {
                listForReturn.AddRange(keyWords.Where(keyword => keyword.ToLower() == sortDictionary.ElementAt(i).Key).Select(keyword => new PrecedentFormVector() { name = sortDictionary.ElementAt(i).Key, weight = sortDictionary.ElementAt(i).Value }));
            }
            for (var i = Math.Min(analitic.Count, 10); i <= analitic.Count; i++)
            {
                listForReturn.Add(new PrecedentFormVector() { name = sortDictionary.ElementAt(i).Key, weight = sortDictionary.ElementAt(i).Value });
            }
            return listForReturn;
        }
        private static List<PrecedentFormVector> MakeTF(List<PrecedentFormVector> list)
        {
            var summ = list.Sum(el => el.weight);
            foreach (var el in list)
            {
                el.weight = el.weight/summ;
            }
            return list;
        }

        private static List<PrecedentFormVector> MakeTFIDF(List<PrecedentFormVector> list)
        {
            double countOfPrecedent = MyLibForNeo4J.GetCollectionByType("Precedent").Count;
            List<PrecedentFormVector> listForReturn = list;
            
            foreach (var el in listForReturn)
            {
               var lower =  el.name.ToLower();
               double countOfPrecedentWithThisKeyWord = MyLibForNeo4J.GetCollectionByTypeAndName("KeyWord", lower).Count;
                if (countOfPrecedentWithThisKeyWord == 0)
                {
                    countOfPrecedentWithThisKeyWord = 1;
                }
                el.weight = Math.Log((countOfPrecedent/countOfPrecedentWithThisKeyWord));
            }
            return listForReturn;
        }
            //string forReturn = null;
            //var th = new Thread(() =>
            //{

            //        WebBrowser wb = new WebBrowser();

            //        wb.Navigate(UrlForNormalForm);
            //        wb.AllowNavigation = true;
            //        wb.ScriptErrorsSuppressed = true;

            //        while (wb.ReadyState != WebBrowserReadyState.Complete)
            //        {
            //            Application.DoEvents();
            //        }




            //    wb.Document.GetElementsByTagName("textarea")[0].InnerText = _textOfPrecedent;
            //    while (wb.ReadyState != WebBrowserReadyState.Complete)
            //    {
            //        Application.DoEvents();
            //    }
            //    wb.DocumentText.Replace("target=\"_blank\"", "");
            //    var elc = wb.Document.GetElementsByTagName("input");
            //    foreach (HtmlElement el in elc)
            //    {
            //        if (el.GetAttribute("value").Equals("показать лемматизированный список и частотный словарь"))
            //        {
            //            el.InvokeMember("Click");


            //            while (wb.ReadyState != WebBrowserReadyState.Complete)
            //            {
            //                Application.DoEvents();
            //            }


            //        }
            //    }

            //    while (wb.ReadyState != WebBrowserReadyState.Complete)
            //    {
            //        Application.DoEvents();
            //    }
            //    forReturn = wb.DocumentText;
            //});

            //th.SetApartmentState(ApartmentState.STA);
            //th.Start();
            //th.Join();

            //return forReturn;
        }

        

      

       
      


  
}