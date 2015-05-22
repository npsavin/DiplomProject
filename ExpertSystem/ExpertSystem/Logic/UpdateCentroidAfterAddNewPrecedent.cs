using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpertSystem.Logic
{
    public static class UpdateCentroidAfterAddNewPrecedent
    {
        public static void SetCentroidForList(int idPrecedent)
        {
            var classOfPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(idPrecedent, "Precedent",
                "ClassPrecedent", "In").FirstOrDefault();
            var idOfThisNode = classOfPrecedent;
            
            var allPrecedentConnectWithThisClass = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idOfThisNode,
                "ClassPrecedent", "Precedent", "In");
            var countOfPrecedent = allPrecedentConnectWithThisClass.Count();
            MyLibForNeo4J.SetClassPrecedentCountOfPrecedent(idOfThisNode, countOfPrecedent);
            var idOfAllKeyWordInThisclass = allPrecedentConnectWithThisClass.SelectMany(precdent => MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(precdent, "Precedent", "KeyWord", "KeyWordIn")).ToList();
            var dictionaryForNewCentroid = new Dictionary<string, double>();
            foreach (var idKeyWord in idOfAllKeyWordInThisclass)
            {
                var nameOfKeyWord = MyLibForNeo4J.GetName(idKeyWord, "KeyWord");
                if (dictionaryForNewCentroid.ContainsKey(nameOfKeyWord))
                {
                    dictionaryForNewCentroid[nameOfKeyWord] += MyLibForNeo4J.GetWeithKeyWord(idKeyWord);
                }
                else
                {
                    dictionaryForNewCentroid.Add(nameOfKeyWord, MyLibForNeo4J.GetWeithKeyWord(idKeyWord));
                }
            }
          
            MyLibForNeo4J.DeleteOldCentroid(idOfThisNode);
            foreach (var idCentroid in dictionaryForNewCentroid.Select(centroid => MyLibForNeo4J.CreateCentroid(centroid.Key, centroid.Value / countOfPrecedent)))
            {
                MyLibForNeo4J.ConnectTwoNodes(idCentroid, "Centroid", idOfThisNode, "ClassPrecedent", "CentroidIn");
            }


            SetCentroidForNotList(MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idOfThisNode, "ClassPrecedent", "ClassPrecedent",
                "Extension").FirstOrDefault());
        }

        private static void SetCentroidForNotList(int idNode)
        {

            var idOfThisNode = idNode;
            while (idOfThisNode != default(int))
            {
                var allClassPrecedentExtensionThisClassPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(idOfThisNode, "ClassPrecedent", "ClassPrecedent",
                    "Extension");
                var newCountOfPrecedent = 0;
                var dictionaryForNewCentroid = new Dictionary<string, double>();
                
                foreach (var idOfClassPrecedent in allClassPrecedentExtensionThisClassPrecedent)
                {
                   newCountOfPrecedent += MyLibForNeo4J.GetCountOfPrecedent(idOfClassPrecedent);
                   var allCentroidOnThisPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idOfClassPrecedent, "ClassPrecedent",
                        "Centroid", "CentroidIn");
                    foreach (var centroid in allCentroidOnThisPrecedent)
                    {
                        var nameOfCentroid = MyLibForNeo4J.GetName(centroid, "Centroid");
                        if (dictionaryForNewCentroid.ContainsKey(nameOfCentroid))
                        {
                            dictionaryForNewCentroid[nameOfCentroid] += MyLibForNeo4J.GetWeithKeyWord(centroid) * MyLibForNeo4J.GetCountOfPrecedent(idOfClassPrecedent);
                        }
                        else
                        {
                            dictionaryForNewCentroid.Add(nameOfCentroid, MyLibForNeo4J.GetWeithKeyWord(centroid) * MyLibForNeo4J.GetCountOfPrecedent(idOfClassPrecedent));
                        }
                    }
                }
                MyLibForNeo4J.DeleteOldCentroid(idOfThisNode);
                foreach (var idCentroid in dictionaryForNewCentroid.Select(centroid => MyLibForNeo4J.CreateCentroid(centroid.Key, centroid.Value/newCountOfPrecedent)))
                {
                    MyLibForNeo4J.ConnectTwoNodes(idCentroid, "Centroid", idOfThisNode, "ClassPrecedent", "CentroidIn");
                }
                MyLibForNeo4J.SetClassPrecedentCountOfPrecedent(idOfThisNode, newCountOfPrecedent);
                
                idOfThisNode = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(idOfThisNode, "ClassPrecedent",
                    "ClassPrecedent", "Extension").FirstOrDefault();
            }
            
        }
    }
}