using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ExpertSystem.Logic;
using Newtonsoft.Json.Linq;

namespace ExpertSystem
{
    public partial class Default : Page
    {
        public int IdPrecedent { get; set; }

        // В четверг.
        // Связаться с Матвеем.
        // Презентация 5-7 минут....
        // 14 Апреля
        // 30 апреля. Текст сделать. 
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("http://www.microsoft.com");
            GetNormalFormPrecedent g = new GetNormalFormPrecedent("Привет rfr lfsafas fasfasfsa k;lfsakf;as fasfas" +
                                                                  "fsafasfas fasf asf");
            TextBox1.TextMode = TextBoxMode.MultiLine;
            TextBox1.Text = g.GetNormalForm();
        }


        protected void Translate(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            TextBox7.TextMode = TextBoxMode.MultiLine;
            foreach (var el in WorkKeyWord.FindKeyWord(TextBox1.Text))
            {
                sb.AppendLine(el);
            }

            TextBox7.Text = sb.ToString();
            sb.Clear();


        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            var filename = FileUpload1.FileContent;
            StreamReader str = new StreamReader(filename);
            var text = str.ReadToEnd();
            text = text.Trim().Remove(0, text.IndexOf("html", StringComparison.Ordinal));

            //if (filename == null) throw new ArgumentNullException("sender");
            //var token = JObject.Parse(text);
            //if (token == null) throw new ArgumentNullException("sender");



            //var name = ((string)token["result"][0]["name"]);
            //var symptom = ((string)token["result"][0]["symptom"]);
            //var threat = ((string)token["result"][0]["threat"]);
            //var consequent = ((string)token["result"][0]["consequences"]);
            //var countermeasure = ((string)token["result"][0]["countermeasures"]);
            //var loose = ((string)token["result"][0]["looses"]);





            TextBox1.TextMode = TextBoxMode.MultiLine;

            //var stringBuilder = new StringBuilder();



            //    stringBuilder.AppendLine("Name");
            //    stringBuilder.AppendLine(name);
            //    stringBuilder.AppendLine();
            //    stringBuilder.AppendLine("Symptom");
            //    stringBuilder.AppendLine(symptom);
            //    stringBuilder.AppendLine();
            //    stringBuilder.AppendLine("Threat");
            //    stringBuilder.AppendLine(threat);
            //    stringBuilder.AppendLine();
            //    stringBuilder.AppendLine("Consequences");
            //    stringBuilder.AppendLine(consequent);
            //    stringBuilder.AppendLine();
            //    stringBuilder.AppendLine("Countermeasures");
            //    stringBuilder.AppendLine(countermeasure);
            //    stringBuilder.AppendLine();
            //    stringBuilder.AppendLine("Looses");
            //    stringBuilder.AppendLine(loose);
            //    TextBox1.Text = stringBuilder.ToString();
            //    stringBuilder.Clear();
            TextBox1.Text = TextBox7.Text = TranslateYa.TranslateInYandex(text);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {



            //const string name = "Name";
            //const string simptom = "Symptom";
            //const string consequences = "Consequences";
            //const string countermeasures = "Countermeasures";
            //const string threat = "Threat";
            //const string looses = "Looses";

            var filename = FileUpload1.FileName;
            var text =
                File.ReadAllText(
                    "C:\\Users\\npsav_000\\Documents\\Visual Studio 2013\\Projects\\MyProject\\Conteiners\\" + filename);
            //var token = JObject.Parse(text);
            //var s = TextBox1.Text;

            //    token["result"][0]["name"] = s.Substring(s.IndexOf(name, StringComparison.Ordinal) + 6, (s.IndexOf(simptom, StringComparison.Ordinal) - s.IndexOf(name, StringComparison.Ordinal) - 10));
            //    token["result"][0]["symptom"] = s.Substring(s.IndexOf(simptom, StringComparison.Ordinal) + 9 ,(s.IndexOf(threat, StringComparison.Ordinal) - s.IndexOf(simptom, StringComparison.Ordinal) - 13));
            //    token["result"][0]["threat"] = s.Substring(s.IndexOf(threat, StringComparison.Ordinal) + 9, (s.IndexOf(consequences, StringComparison.Ordinal) - s.IndexOf(threat, StringComparison.Ordinal) - 12));
            //    token["result"][0]["consequences"] = s.Substring(s.IndexOf(consequences, StringComparison.Ordinal) + 14, (s.IndexOf(countermeasures, StringComparison.Ordinal) - s.IndexOf(consequences, StringComparison.Ordinal) - 18));
            //    token["result"][0]["countermeasures"] = s.Substring(s.IndexOf(countermeasures, StringComparison.Ordinal) + 17, (s.IndexOf(looses, StringComparison.Ordinal) - s.IndexOf(countermeasures, StringComparison.Ordinal) - 21));
            //    token["result"][0]["looses"] = s.Substring(s.IndexOf(looses, StringComparison.Ordinal) + 8, (s.Length - s.IndexOf(looses, StringComparison.Ordinal)) - 10);





            File.AppendAllText(
                "C:\\Users\\npsav_000\\Documents\\Visual Studio 2013\\Projects\\MyProject\\Conteiners\\new" + filename,
                text.ToString());


        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            //var idPrecedent = MyLibForNeo4J.CreateNode("Precedent", ChangeSubstring("Precedent"));
            //var idSymptom = MyLibForNeo4J.CreateNode("Symptom", ChangeSubstring("Symptom"));
            //var idThreat = MyLibForNeo4J.CreateNode("Threat", ChangeSubstring("Threat"));
            //var idConsequences = MyLibForNeo4J.CreateNode("Consequences", ChangeSubstring("Consequences"));
            //var idMeasure = MyLibForNeo4J.CreateNode("Measure", ChangeSubstring("Measure"));
            //var idLoss = MyLibForNeo4J.CreateNode("Loss", ChangeSubstring("Loss"));

            //MyLibForNeo4J.ConnectTwoNodes(idPrecedent, "Precedent", idSymptom, "Symptom", "Parameter");
            //MyLibForNeo4J.ConnectTwoNodes(idPrecedent, "Precedent", idThreat, "Threat", "Parameter");
            //MyLibForNeo4J.ConnectTwoNodes(idPrecedent, "Precedent", idConsequences, "Consequences", "Parameter");
            //MyLibForNeo4J.ConnectTwoNodes(idPrecedent, "Precedent", idMeasure, "Measure", "Parameter");
            //MyLibForNeo4J.ConnectTwoNodes(idPrecedent, "Precedent", idLoss, "Loss", "Parameter");

            //var keyWordSymptom = WorkKeyWord.FindKeyWord(ChangeSubstring("Symptom"));
            //var keyWordThreat = WorkKeyWord.FindKeyWord(ChangeSubstring("Threat"));
            //var keyWordConsequences = WorkKeyWord.FindKeyWord(ChangeSubstring("Consequences"));
            //var keyWordMeasure = WorkKeyWord.FindKeyWord(ChangeSubstring("Measure"));
            //var keyWordLoss = WorkKeyWord.FindKeyWord(ChangeSubstring("Loss"));

            //var idsKeySymptom = MyLibForNeo4J.CreateKeyWords(keyWordSymptom);
            //var idsKeyThreat = MyLibForNeo4J.CreateKeyWords(keyWordThreat);
            //var idsConsequences = MyLibForNeo4J.CreateKeyWords(keyWordConsequences);
            //var idsKeyMeasure = MyLibForNeo4J.CreateKeyWords(keyWordMeasure);
            //var idsKeyLoss = MyLibForNeo4J.CreateKeyWords(keyWordLoss);

            //MyLibForNeo4J.ConnectKeyWordsWithParametr(idsKeySymptom, idSymptom, "Symptom");
            //MyLibForNeo4J.ConnectKeyWordsWithParametr(idsKeyThreat, idThreat, "Threat");
            //MyLibForNeo4J.ConnectKeyWordsWithParametr(idsConsequences, idConsequences, "Consequences");
            //MyLibForNeo4J.ConnectKeyWordsWithParametr(idsKeyMeasure, idMeasure, "Measure");
            //MyLibForNeo4J.ConnectKeyWordsWithParametr(idsKeyLoss, idLoss, "Loss");

            //IdPrecedent = idPrecedent;
            //Server.Transfer("Ontology.aspx", true);

        }




        //private string ChangeSubstring(string type)
        //{
        //    var stringforReturn = type;
        //    switch (type)
        //    {
        //        case "Precedent":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Имя", StringComparison.Ordinal) + 4,
        //                (TextBox7.Text.IndexOf("Симптом", StringComparison.Ordinal) - TextBox7.Text.IndexOf("Имя", StringComparison.Ordinal) - 6));
        //            break;
        //        case "Symptom":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Симптом", StringComparison.Ordinal) + 7,
        //                (TextBox7.Text.IndexOf("Опасный", StringComparison.Ordinal) - TextBox7.Text.IndexOf("Симптом", StringComparison.Ordinal) - 10));
        //            break;
        //        case "Threat":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Опасный", StringComparison.Ordinal) + 7,
        //                (TextBox7.Text.IndexOf("Последствия", StringComparison.Ordinal) - TextBox7.Text.IndexOf("Угроза", StringComparison.Ordinal) - 10));
        //            break;
        //        case "Consequences":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Последствия", StringComparison.Ordinal) + 11,
        //                (TextBox7.Text.IndexOf("Контрмеры", StringComparison.Ordinal) - TextBox7.Text.IndexOf("Последствия", StringComparison.Ordinal) - 14));
        //            break;
        //        case "Measure":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Контрмеры", StringComparison.Ordinal) + 9,
        //                (TextBox7.Text.IndexOf("Теряет", StringComparison.Ordinal) - TextBox7.Text.IndexOf("Контрмеры", StringComparison.Ordinal) - 12));
        //            break;
        //        case "Loss":
        //            stringforReturn = TextBox7.Text.Substring(TextBox7.Text.IndexOf("Теряет", StringComparison.Ordinal) + 8,
        //                (FeatureForLossSubstring(TextBox7)));
        //            break;
        //    }
        //    return stringforReturn;
        protected void Button5_Click(object sender, EventArgs e)
        {
            File.AppendAllText(@"E:\Users\Nikita\OneDrive\Documents\Study\Diplom\MyProject\ExpertSystem\ExpertSystem\StopWords.txt", " " + TextBox20.Text.ToLower());
        }
    }

    //private static int FeatureForLossSubstring(ITextControl textBox)
    //{
    //    var returned = 0;
    //    var a = textBox.Text.Length - textBox.Text.IndexOf("Теряет", StringComparison.Ordinal) - 10;
    //    if(a >=0)
    //    {
    //        returned = a;
    //    }
    //    return returned;
    //}


   

    //protected void Button6_Click(object sender, EventArgs e)
    //{



}


