using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpertSystem.Logic;
using Neo4jClient;
using Newtonsoft.Json.Linq;

namespace ExpertSystem
{
    public partial class About : Page
    {

        public int PrecedentId { get; set; }
        public Util ThisClassPrecedent { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)

        {
            PrecedentId = PreviousPage.IdPrecedent;
            ThisClassPrecedent = Classification.ClassificationThisPrecedent(PrecedentId, 0);
            var textbox2 = new StringBuilder();
            TextBox2.TextMode = TextBoxMode.MultiLine;
            if (ThisClassPrecedent.positiveClass.Count == 1)
            {
                textbox2.AppendLine("Ваш прецедент был успешно классифифцирован к классу с именем: ");
                textbox2.AppendLine(MyLibForNeo4J.GetName(ThisClassPrecedent.positiveClass.FirstOrDefault(),
                    "ClassPrecedent"));
                textbox2.AppendLine("Были ещё варианты");
                foreach (var el in ThisClassPrecedent.negativeClass)
                {
                    textbox2.AppendLine(MyLibForNeo4J.GetName(el, "ClassPrecedent"));
                }
                TextBox2.Text = textbox2.ToString();
                textbox2.Clear();
            }else if (!ThisClassPrecedent.positiveClass.Any())
            {
                textbox2.AppendLine(
                    "Классификатор не смог определить класс, на определённом шаге" +
                    " Вам необходимо ввести название класса вручную.");
                textbox2.AppendLine("Возможные варианты:");
                foreach (var el in ThisClassPrecedent.negativeClass)
                {
                    textbox2.AppendLine(MyLibForNeo4J.GetName(el, "ClassPrecedent"));
                }
            }




        }

        

        protected void Yes_click(object sender, EventArgs e)
        {
            MyLibForNeo4J.ConnectTwoNodes(PrecedentId, "Precedent", ThisClassPrecedent.positiveClass.FirstOrDefault(), "ClassPrecedent", "In");
            UpdateCentroidAfterAddNewPrecedent.SetCentroidForList(PrecedentId);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var classId = MyLibForNeo4J.CreateClass(TextBox1.Text, 0, 1);
            PreviousPage.StartPrecedent = classId;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
           var classChosedExpert = MyLibForNeo4J.GetCollectionByTypeAndName("ClassPrecedent", TextBox3.Text);
           var distance = Classification.GetDistance(PrecedentId, classChosedExpert.FirstOrDefault());
           var maxDistance = MyLibForNeo4J.GetMaxDistanceInClass(classChosedExpert.FirstOrDefault());
            if (distance > maxDistance)
            {
                MyLibForNeo4J.SetMaxDistanceInClass(classChosedExpert.FirstOrDefault(), distance);
            }
           PreviousPage.StartPrecedent = classChosedExpert.FirstOrDefault();

        }

    }
}