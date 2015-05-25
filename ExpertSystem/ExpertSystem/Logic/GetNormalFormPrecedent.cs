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

        public IEnumerable<PrecedentFormVector> GetNormalForm()
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
                            if (timeName != null && stopword == timeName.ToLower())
                            {
                                flag = false;
                            }
                           
                        }
                        if (flag)
                        {
                            ListWithCount.Add(new PrecedentFormVector() { name = timeName.ToLower(), weight = timeWeight });    
                        }
                        
                    }
                    i++;
                }
                var listWithTf = MakeTF(ListWithCount.Distinct());
                var listWithTFIDF = MakeTFIDF(listWithTf);
                return listWithTFIDF;
            }
        }

        public IEnumerable<PrecedentFormVector> NormalFormAfterExpertEdit(string keyWordOfExpert, IEnumerable<PrecedentFormVector> analitic)
        {
            var keyWords = keyWordOfExpert.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            var vectorDictionary = new Dictionary<string, double>();
            foreach (var vector in analitic)
            {
                if (vectorDictionary.ContainsKey(vector.name))
                {
                    vectorDictionary[vector.name] += vector.weight;
                }
                else
                {
                    vectorDictionary.Add(vector.name, vector.weight);
                }

            }
            var sortDictionary = vectorDictionary.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            var maxWeith = sortDictionary.First().Value;
            //var listForReturn = keyWords.Where(keyWord => !vectorDictionary.ContainsKey(keyWord.ToLower())).Select(keyWord => new PrecedentFormVector() {name = keyWord.ToLower(), weight = maxWeith}).ToList();
            for (int i = 0; i < Math.Min(sortDictionary.Count, 10); i++)
            {
                if (!keyWords.Contains(sortDictionary.ElementAt(i).Key))
                {
                    sortDictionary[sortDictionary.ElementAt(i).Key] = 0;
                }
            }
            foreach (var el in keyWords)
            {
                if (!sortDictionary.ContainsKey(el))
                {
                    sortDictionary.Add(el, maxWeith);
                }
                else
                {
                    if (sortDictionary[el] < sortDictionary.ElementAt(Math.Max(10, sortDictionary.Count)).Value)
                    {
                        sortDictionary[el] = maxWeith;
                    }
                }
            }
            return sortDictionary.Select(el => new PrecedentFormVector() {name = el.Key, weight = el.Value}).ToList();
        }
        private static IEnumerable<PrecedentFormVector> MakeTF(IEnumerable<PrecedentFormVector> list)
        {
            var summ = list.Sum(el => el.weight);
            foreach (var el in list)
            {
                el.weight = el.weight/summ;
            }
            return list;
        }

        private static IEnumerable<PrecedentFormVector> MakeTFIDF(IEnumerable<PrecedentFormVector> list)
        {
            double countOfPrecedent = MyLibForNeo4J.GetCollectionByType("Precedent").Count;
            var listOfAllKeyWord = MyLibForNeo4J.GetCollectionByType("KeyWord");
            IEnumerable<PrecedentFormVector> listForReturn = list;
            
            foreach (var el in listForReturn)
            {
               var lower =  el.name.ToLower();
                double countOfPrecedentWithThisKeyWord = listOfAllKeyWord.FindAll(
                    st => st == lower).Count;
                if (countOfPrecedentWithThisKeyWord == 0.0)
                {
                    countOfPrecedentWithThisKeyWord = 1;
                }
                el.weight = el.weight * Math.Abs(Math.Log((countOfPrecedent/countOfPrecedentWithThisKeyWord)));
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