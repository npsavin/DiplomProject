using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem.Logic
{
    public static class Bayes
    {


        public static int? BayesForType(int id, string type2)
        {
            var timeElement = MyLibForNeo4J.GetAllNameNodesConnectionWithThisNodeRight(id, "Precedent", type2, "Parameter");
            var el =timeElement.FirstOrDefault();
            if (el != null)
            {
                var nameOfParameter =
                    el.Trim();
                var idOfParameter =
                    MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(id, "Precedent", type2, "Parameter")
                        .FirstOrDefault();
                var idClassOfType = RecoursiveAlgoritm(type2, 0, nameOfParameter);
                var claasType = "Class" + type2;

                MyLibForNeo4J.ConnectTwoNodes(idOfParameter, type2, (int)idClassOfType, claasType, "InClass");
                return idClassOfType;
            }
            return null;
        }

        private static int RecoursiveAlgoritm(string type, int idOfStart, string text)
        {
            while (true)
            {

                var typeOfClass = "Class" + type;
                var typeOfKeyWord = "KeyWordOf" + type;
                var collectionOfNodes = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idOfStart, typeOfClass,
                    typeOfClass, "IS_KIND_OF");

                if (!collectionOfNodes.Any())
                {
                    return idOfStart;
                }
                var dictionary = new Dictionary<int, int>();
                foreach (var el in collectionOfNodes)
                {
                    dictionary.Add(el, 0);
                    var keyWords = MyLibForNeo4J.GetAllNameNodesConnectionWithThisNodeLeft(el, typeOfClass,
                        typeOfKeyWord, "IN");
                    foreach (
                        var word in
                            keyWords.Select(GetForm.GetAllForm).SelectMany(collectionAllForm => collectionAllForm))
                    {
                        for (var i = 0; i <= text.Split(new[] {word}, StringSplitOptions.None).Length; i++)
                        {
                            dictionary[el]++;
                        }
                    }
                }
                var a = dictionary.Where(x => x.Value == dictionary.Values.Max());
                var first = new KeyValuePair<int, int>();
                foreach (var pair in a)
                {
                    first = pair;
                    break;
                }
                if (first.Value == 0)
                {
                    return -idOfStart;
                }

                idOfStart = first.Key;

                
            }
        }
    }
}

    
