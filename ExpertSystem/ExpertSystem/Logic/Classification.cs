using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpertSystem.Logic
{
    public static class Classification
    {
        public static Util ClassificationThisPrecedent(int idPrecedent, int idClass)
        {
            var idOfThisClassPrecedent = idClass;
            var listForPositive = new List<int>();
            var listForNegative = new List<int>();
            var AllKeyWordOnThisPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idPrecedent,
                "Prcedent", "KeyWord", "KeyWordIn");
            while (idOfThisClassPrecedent != default(int))
            {
                var allClassPrecedentConnectWithThisNode =
                    MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeRight(idOfThisClassPrecedent, "ClassPrecedent",
                        "ClassPrecedent", "Extension");
                if (!allClassPrecedentConnectWithThisNode.Any())
                {
                    listForPositive.Clear();
                    listForPositive.Add(idOfThisClassPrecedent);
                    break;
                }
                var idClassAndDistantion = new Dictionary<int, double>();
                foreach (var classPrecedent in allClassPrecedentConnectWithThisNode)
                {
                    var centroidInThisClass = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(classPrecedent,
                        "ClassPrecedent", "Centroid",
                        "CentroidIn");
                    var centroidAndKeyWord =
                        centroidInThisClass.ToDictionary(centroid => MyLibForNeo4J.GetName(centroid, "Centroid"),
                            MyLibForNeo4J.GetWeithCentroid);
                    foreach (var keyWord in AllKeyWordOnThisPrecedent)
                    {
                        var nameOfKeyWord = MyLibForNeo4J.GetName(keyWord, "KeyWord");
                        if (centroidAndKeyWord.ContainsKey(nameOfKeyWord))
                        {
                            centroidAndKeyWord[nameOfKeyWord] =
                                Math.Abs(centroidAndKeyWord[nameOfKeyWord] - MyLibForNeo4J.GetWeithKeyWord(keyWord));
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
                var sortDictionary =
                    idClassAndDistantion.OrderByDescending(pair => pair.Value)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                var firstCandidate = sortDictionary.ElementAt(i);
                var idOfCandidate = new List<ClassForClassification>();

                foreach (var el in sortDictionary)
                {
                    if (firstCandidate.Value < avarageDistantion - sortDictionary.FirstOrDefault().Value)
                    {
                        idOfCandidate.Add(new ClassForClassification()
                        {
                            distance = firstCandidate.Value,
                            id = firstCandidate.Key
                        });
                    }
                    else
                    {
                        idOfCandidate.Add(new ClassForClassification()
                        {
                            distance = firstCandidate.Value,
                            id = -firstCandidate.Key
                        });
                    }
                    i++;
                    if (sortDictionary.Count() <= i)
                    {
                        break;
                    }
                    firstCandidate = el;
                }
                var listOfPositiveCandidate = idOfCandidate.Where(candidate => candidate.id >= 0).ToList();
                if (listOfPositiveCandidate.Count == 1)
                {
                    if (listOfPositiveCandidate.FirstOrDefault().distance <
                        MyLibForNeo4J.GetMaxDistanceInClass(listOfPositiveCandidate.FirstOrDefault().id))
                    {
                        idOfThisClassPrecedent = Math.Abs(idOfCandidate.First().id);
                        listForNegative.Clear();
                        listForNegative.AddRange(idOfCandidate.Select(el => Math.Abs(el.id)));
                    }
                    else
                    {
                        listForNegative.Clear();
                        listForPositive.Clear();
                        listForNegative.Add(Math.Abs(listOfPositiveCandidate.FirstOrDefault().id));
                        listForNegative.AddRange(idOfCandidate.Select(el => Math.Abs(el.id)));
                        break;
                    }
                }
                if (listOfPositiveCandidate.Count > 1)
                {
                    listForPositive.AddRange(listOfPositiveCandidate.Select(el => Math.Abs(el.id)));
                    break;
                }

            }
            return new Util() {positiveClass = listForPositive, negativeClass = listForNegative};

        }

        public static double GetDistance(int idPrcedent, int classPrecednt)
        {
            var AllKeyWordOnThisPrecedent = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(idPrcedent,
                "Prcedent", "KeyWord", "KeyWordIn");
            var centroidInThisClass = MyLibForNeo4J.GetAllIdNodesConnectionWithThisNodeLeft(classPrecednt,
                "ClassPrecedent", "Centroid",
                "CentroidIn");
            var centroidAndKeyWord =
                centroidInThisClass.ToDictionary(centroid => MyLibForNeo4J.GetName(centroid, "Centroid"),
                    MyLibForNeo4J.GetWeithCentroid);
            foreach (var keyWord in AllKeyWordOnThisPrecedent)
            {
                var nameOfKeyWord = MyLibForNeo4J.GetName(keyWord, "KeyWord");
                if (centroidAndKeyWord.ContainsKey(nameOfKeyWord))
                {
                    centroidAndKeyWord[nameOfKeyWord] =
                        Math.Abs(centroidAndKeyWord[nameOfKeyWord] - MyLibForNeo4J.GetWeithKeyWord(keyWord));
                }
                else
                {
                    centroidAndKeyWord.Add(nameOfKeyWord, MyLibForNeo4J.GetWeithKeyWord(keyWord));
                }
            }
            var distantion = centroidAndKeyWord.Values.Sum();
            return distantion;
        }

    }
}
