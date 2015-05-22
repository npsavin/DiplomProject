using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpertSystem.Logic
{
    public static class Classification
    {
        public static List<int> ClassificationThisPrecedent(int idPrecedent)
        {
            var idOfThisClassPrecedent = 0;
            var listForReturn = new List<int>();
            var AllKeyWordOnThisPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idPrecedent, "Prcedent", "KeyWord", "KeyWordIn");
            while (idOfThisClassPrecedent!=default(int))
            {
                var allClassPrecedentConnectWithThisNode = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(idOfThisClassPrecedent, "ClassPrecedent", "ClassPrecedent", "Extension");
                if (!allClassPrecedentConnectWithThisNode.Any())
                {
                    listForReturn.Add(idOfThisClassPrecedent);
                    break;
                }
                var idClassAndDistantion = new Dictionary<int, double>();
                foreach (var classPrecedent in allClassPrecedentConnectWithThisNode)
                {
                    var centroidInThisClass = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(classPrecedent, "ClassPrecedent", "Centroid",
                        "CentroidIn");
                    var centroidAndKeyWord = centroidInThisClass.ToDictionary(centroid => MyLibForNeo4J.GetName(centroid, "Centroid"), MyLibForNeo4J.GetWeithCentroid);
                    foreach (var keyWord in AllKeyWordOnThisPrecedent)
                    {
                        var nameOfKeyWord = MyLibForNeo4J.GetName(keyWord, "KeyWord");
                        if (centroidAndKeyWord.ContainsKey(nameOfKeyWord))
                        {
                            centroidAndKeyWord[nameOfKeyWord] = Math.Abs(centroidAndKeyWord[nameOfKeyWord] - MyLibForNeo4J.GetWeithKeyWord(keyWord));
                        }
                        else
                        {
                            centroidAndKeyWord.Add(nameOfKeyWord, MyLibForNeo4J.GetWeithKeyWord(keyWord));
                        }
                    }
                    var distantion = centroidAndKeyWord.Values.Sum();
                    idClassAndDistantion.Add(classPrecedent, distantion);
                }
                var avarageDistantion = idClassAndDistantion.Values.Average();
                var i = 0;
                var sortDictionary = idClassAndDistantion.Where(x => x.Value == idClassAndDistantion.Values.Min());
                var firstCandidate = sortDictionary.ElementAt(i);
                var idOfCandidate = new List<int>();
               
                while (firstCandidate.Value < avarageDistantion)
                {
                    
                    idOfCandidate.Add(firstCandidate.Key);
                    i++;
                    if (sortDictionary.Count() <= i)
                    {
                        break;
                    }
                    firstCandidate = sortDictionary.ElementAt(i);
                    
                }

                if (idOfCandidate.Count == 1)
                {
                    idOfThisClassPrecedent = idOfCandidate.First();
                }else
                {
                    foreach (var candidate in idOfCandidate)
                    {
                        listForReturn.Add(candidate);
                        break;
                    }
                }
            }
            return listForReturn;

        }
    }
}