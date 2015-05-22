using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Neo4jClient;
using Newtonsoft.Json.Linq;

namespace ExpertSystem
{
    public partial class Default : Page
    {
        private GraphClient _client;

        protected void Page_Load(object sender, EventArgs e)
        {
            _client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            _client.Connect();
        }


        protected void Translate(object sender, EventArgs e)
        {


            const string url =
                "https://translate.yandex.net/api/v1.5/tr/translate?key=trnsl.1.1.20141121T084319Z.baf98af8dbfbd466.611296bcbd54b8fa8215c237e78b9748749732b2";

            var strigBuilder = new StringBuilder();
            TextBox1.Text = TextBox1.Text.Replace("&", "%26");
            TextBox1.Text = TextBox1.Text.Replace(";", "");
            strigBuilder.Append("&text=" + TextBox1.Text);
            strigBuilder.Append("&lang=en-ru");
            strigBuilder.Append("&format=json");


            var req = WebRequest.Create(url + strigBuilder);
            var resp = req.GetResponse();
            var stream = resp.GetResponseStream();
            var sr = new StreamReader(stream);
            var Out = sr.ReadToEnd();
            sr.Close();
            TextBox7.TextMode = TextBoxMode.MultiLine;
            if (Out.Contains("<text>") && Out.Contains("</text>"))
            {
                TextBox7.Text = Out.Substring(Out.IndexOf("<text>") + 6,
                    (Out.IndexOf("</text>") - Out.IndexOf("<text>") - 6));
            }
            var textBox = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Симптом") + 7,
           (TextBox7.Text.IndexOf("Угроза") - TextBox7.Text.IndexOf("Симптом") - 10));
            
            FindKeyWord(textBox);
            Bayes(textBox, "Symptom", _client);
        }


        public static void Bayes(string text, string name, GraphClient client)
        {
            var nameForFindInDataBase = "(n:KeyWordOf" + name + ")";

            if (client == null) throw new ArgumentNullException("client");
            var keyWords1 = client.Cypher
                 .Match(nameForFindInDataBase)
                 .Return(n => n.As<string>())
                 .Results;

        }


    private void FindKeyWord(string text)
    {
        

                var dictionary = new Dictionary<string, int>();
       
        
        var words = text.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
        var stopWords =
            File.ReadAllText(@"C:\Users\npsav_000\SkyDrive\Документы\Study\Diplom\MyProject\ExpertSystem\ExpertSystem\StopWords.txt"
                ).Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);;

        IEnumerable<string> wordsFinal = FindAndDeleteStopWord(words, stopWords);
        foreach (var word in wordsFinal)
        {
            
            if (dictionary.ContainsKey(word))
            {
                dictionary[word]++;
            }
            else
            {
                dictionary.Add(word, 1);
            }
        }
        var counter = 0;
        var b = 0;
        var keyWords = new List<string>();
        var LimitCicle = Math.Min(words.Count(), 10); 
        while (b / (words.Length + 1) < 0.05 && counter < LimitCicle)
        {
            var key = dictionary.FirstOrDefault(x => x.Value == dictionary.Values.Max()).Key;
            keyWords.Add(key);
            var a = dictionary.FirstOrDefault(x => x.Value == dictionary.Values.Max()).Value;
            b = b + a;
            if (key != null) dictionary.Remove(key);
            counter++;
        }
        var sb = new StringBuilder();
       
        foreach (var keyWord in keyWords)
        {
            sb.AppendLine(keyWord);
        }
        StringBuilder keyWordsWithoutRepet = new StringBuilder();
        var finelKeyWords = sb.ToString().Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries).Distinct();
        foreach (var VARIABLE in finelKeyWords)
        {
            keyWordsWithoutRepet.AppendLine(VARIABLE);
        }
        TextBox13.TextMode = TextBoxMode.MultiLine;
        TextBox13.Text = keyWordsWithoutRepet.ToString();
        keyWordsWithoutRepet.Clear();
        sb.Clear();
     

        }

        private static IEnumerable<string> FindAndDeleteStopWord(IEnumerable<string> words, IEnumerable<string> stopWords)
        {
            foreach (var word in words)
            {
                if (!stopWords.Contains(word))
                {
                    
                    yield return new string(word.Where(Char.IsLetter).ToArray()).ToLower();
                }
            }
        }
        


        protected void Button2_Click(object sender, EventArgs e)
        {
            
            var filename = FileUpload1.FileContent;
            StreamReader str = new StreamReader(filename);
            var text = str.ReadToEnd();
            if (filename == null) throw new ArgumentNullException("filename");
            var token = JObject.Parse(text);
            if (token == null) throw new ArgumentNullException("token");

           
          
            var name = ((string)token["result"][0]["name"]);
            var symptom = ((string)token["result"][0]["symptom"]);
            var threat = ((string)token["result"][0]["threat"]);
            var consequent = ((string)token["result"][0]["consequences"]);
            var countermeasure = ((string)token["result"][0]["countermeasures"]);
            var loose = ((string)token["result"][0]["looses"]);
            


           
            
                TextBox1.TextMode = TextBoxMode.MultiLine;
            
            var stringBuilder = new StringBuilder();
            
            
                
                stringBuilder.AppendLine("Name");
                stringBuilder.AppendLine(name);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Symptom");
                stringBuilder.AppendLine(symptom);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Threat");
                stringBuilder.AppendLine(threat);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Consequences");
                stringBuilder.AppendLine(consequent);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Countermeasures");
                stringBuilder.AppendLine(countermeasure);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Looses");
                stringBuilder.AppendLine(loose);
                TextBox1.Text = stringBuilder.ToString();
                stringBuilder.Clear();
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
           
            

            const string name = "Name";
            const string simptom = "Symptom";
            const string consequences = "Consequences";
            const string countermeasures = "Countermeasures";
            const string threat = "Threat";
            const string looses = "Looses";

            var i = 0;
            var filename = FileUpload1.FileName;
            var text = File.ReadAllText("C:\\Users\\npsav_000\\Documents\\Visual Studio 2013\\Projects\\MyProject\\Conteiners\\" + filename);
            var token = JObject.Parse(text);
            var s = TextBox1.Text;
            
                token["result"][0]["name"] = s.Substring(s.IndexOf(name) + 6, (s.IndexOf(simptom) - s.IndexOf(name) - 10));
                token["result"][0]["symptom"] = s.Substring(s.IndexOf(simptom) + 9 ,(s.IndexOf(threat) - s.IndexOf(simptom) - 13));
                token["result"][0]["threat"] = s.Substring(s.IndexOf(threat) + 9, (s.IndexOf(consequences) - s.IndexOf(threat) - 12));
                token["result"][0]["consequences"] = s.Substring(s.IndexOf(consequences) + 14, (s.IndexOf(countermeasures) - s.IndexOf(consequences) - 18));
                token["result"][0]["countermeasures"] = s.Substring(s.IndexOf(countermeasures) + 17, (s.IndexOf(looses) - s.IndexOf(countermeasures) - 21));
                token["result"][0]["looses"] = s.Substring(s.IndexOf(looses) + 8, (s.Length - s.IndexOf(looses)) - 10);
               
            
            


            File.AppendAllText("C:\\Users\\npsav_000\\Documents\\Visual Studio 2013\\Projects\\MyProject\\Conteiners\\new" + filename, token.ToString());
            
        
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            
            Server.Transfer("Ontology.aspx", false);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
                File.AppendAllText(@"E:\Users\Nikita\OneDrive\Documents\Study\Diplom\MyProject\ExpertSystem\ExpertSystem\StopWords.txt", " " + TextBox14.Text.ToLower());
        }

    }
}