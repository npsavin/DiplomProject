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
        protected void Page_Load(object sender, EventArgs e)

        {
            PrecedentId = Convert.ToInt32(Request.QueryString["PrecedentId"]);
            TextBox1.Text = PrecedentId.ToString();

        }

        

        protected void Yes_click(object sender, EventArgs e)
        {
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            //var node = MyLibForNeo4J.CreateNode("Class" + _list[_counter], TextBox1.Text);
            //var timeElement = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(IdPrecedent, "Precedent", _list[_counter], "Parameter").FirstOrDefault(); ;
            //var collectKeyWord = MyLibForNeo4J.GetAllNameNodesConnectionWithThisNodeLeft(timeElement, _list[_counter], "KeyWord", "KeyWordIn");
            //var idList = collectKeyWord.Select(el => MyLibForNeo4J.CreateNode("KeyWordOf" + _list[_counter], el)).ToList();
            
            //foreach (var el in idList)
            //{
            //    MyLibForNeo4J.ConnectTwoNodes(el, "KeyWordOf" + _list[_counter], timeElement, "Class" + _list[_counter], "IN");
            //}

            //MyLibForNeo4J.ConnectTwoNodes((int)idClassOfType, "Class" + _list[_counter], node, "Class" + _list[_counter], "IS_KIND_OF");

            //_counter++;
            //LoadPage(_counter);
        }

        private void LoadPage(int count)
        {
        }
        
    }
}