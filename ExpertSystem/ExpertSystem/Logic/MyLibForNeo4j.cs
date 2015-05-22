using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.FriendlyUrls;
using Neo4jClient;

namespace ExpertSystem.Logic
{
    public static class MyLibForNeo4J
    {
        private static readonly GraphClient Client = new GraphClient(new Uri("http://137.135.254.83:7474/db/data"));
        private static readonly object Monitor=0;
        private static void ClientConnect()
        {
            lock (Monitor)
            {
                Client.Connect();
            }
        }
        public static void CreateStopWord(string timeName)
        {
            ClientConnect();
            var forRequest = "(n:" + "StopWord" + ")";
            var timeId = Client.Cypher.Match(forRequest).Return(n => n.As<string>()).Results.Count();
            var mn = new StopWord() { id = timeId, name = timeName };
            var stringForCreate = "(n:" + "StopWord" + "{mn})";
            Client.Cypher
                  .Create(stringForCreate)
                  .WithParam("mn", mn)
                  .ExecuteWithoutResults();

        }
        public static List<string> GetCollectionByType(string type)
        {
            ClientConnect();
            var stringForType = "(node:" + type + ")";
            var listNodeOfType = Client.Cypher.Match(stringForType).Return(node => node.As<MainCreater>())
                .Results;
            return listNodeOfType.Select(el => el.name).ToList();
            
        }

        public static List<string> GetCollectionByTypeAndName(string type, string name)
        {
            ClientConnect();
            var stringForConnect = "(node:" + type + ")";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((MainCreater node) => node.name == name)
                .Return(node => node.As<MainCreater>())
                .Results;
            var listNodeOfTypeAndName = forReturn.Select(el => el.name).ToList();
            return listNodeOfTypeAndName;
        }
        public static List<string> GetCollectionByTypeAndId(string type, int id)
        {
            ClientConnect();
            var stringForConnect = "(node:" + type + ")";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((MainCreater node) => node.id == id)
                .Return(node => node.As<MainCreater>())
                .Results;
            var listNodeOfTypeAndName = forReturn.Select(el => el.name).ToList();
            return listNodeOfTypeAndName;
        }
        public static string GetName(int id, string type)
        {
            ClientConnect();
            var stringForConnect = "(node:" + type + ")";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((MainCreater node) => node.id == id)
                .Return(node => node.As<MainCreater>())
                .Results;
            var e = forReturn.FirstOrDefault();
            return e != null ? e.name : null;
        }

        public static double GetWeithCentroid(int id)
        {
            ClientConnect();
            var stringForConnect = "(node:Centroid)";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((Centroid node) => node.id == id)
                .Return(node => node.As<Centroid>())
                .Results;
            var e = forReturn.FirstOrDefault();
            return e.weight;
        }
        public static double GetWeithKeyWord(int id)
        {
            ClientConnect();
            var stringForConnect = "(node:KeyWord)";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((KeyWord node) => node.id == id)
                .Return(node => node.As<KeyWord>())
                .Results;
            var e = forReturn.FirstOrDefault();
            return e.weight;
        }
        public static int GetCountOfPrecedent(int id)
        {
            ClientConnect();
            var stringForConnect = "(node:ClassPrecedent)";
            var forReturn = Client.Cypher
                .Match(stringForConnect)
                .Where((ClassPrecedent node) => node.id == id)
                .Return(node => node.As<ClassPrecedent>())
                .Results;
            var e = forReturn.FirstOrDefault();
            return e.countOfPrecedent;
        }
            
        public static void ConnectTwoNodes(int id1, string type1, int id2, string type2, string nameOfRealition)
        {
            ClientConnect();
            var a = "(" + type1.ToLower() + ":";
            var b = "(" + type2.ToLower() + ":";
            var forFind1 = a + type1 + "{id:" + id1 + "})";
            var forFind2 = b + type2 + "{id:" + id2 + "})";
            var forConnect = type1.ToLower() + "-[:" + nameOfRealition + "]->" + type2.ToLower();
            Client.Cypher
                .Match(forFind1, forFind2)
                .CreateUnique(forConnect)
                .ExecuteWithoutResults();
        }
        public static int CreatePrecedent(string timeName)
        {
            ClientConnect();
            var forRequest = "(n:" + "Precedent" + ")";
            var timeId = Client.Cypher.Match(forRequest).Return(n => n.As<string>()).Results.Count();


            var mn = new Precedent(){id = timeId, name = timeName};
            var stringForCreate = "(n:" + "Precedent" + "{mn})";
            Client.Cypher
                  .Create(stringForCreate)
                  .WithParam("mn", mn)
                  .ExecuteWithoutResults();

            return timeId;
        }
        public static int CreateKeyWord(string timeName, double weight)
        {
            ClientConnect();
            var forRequest = "(n:" + "KeyWord" + ")";
            var timeId = Client.Cypher.Match(forRequest).Return(n => n.As<string>()).Results.Count();



            var mn = new KeyWord(){id = timeId, name = timeName, weight = weight};
            var stringForCreate = "(n:" + "KeyWord" + "{mn})";
            Client.Cypher
                  .Create(stringForCreate)
                  .WithParam("mn", mn)
                  .ExecuteWithoutResults();

            return timeId;
        }

        public static void SetClassPrecedentCountOfPrecedent(int idClassPrecedent, int count)
        {
            Client.Cypher
                .Match("(user:User)")
                .Where((ClassPrecedent node) => node.id == idClassPrecedent)
                .Set("node.weight = {weight}")
                .WithParam("weight", count)
                .ExecuteWithoutResults();
        }
        public static int CreateCentroid(string timeName, double weight)
        {
            ClientConnect();
            var forRequest = "(n:" + "Centroid" + ")";
            var timeId = Client.Cypher.Match(forRequest).Return(n => n.As<string>()).Results.Count();



            var mn = new Centroid() {id = timeId, name = timeName, weight = weight };
            var stringForCreate = "(n:" + "Centroid" + "{mn})";
            Client.Cypher
                  .Create(stringForCreate)
                  .WithParam("mn", mn)
                  .ExecuteWithoutResults();

            return timeId;
        }

        public static IEnumerable<int> GetAllIdNodesConnectionWithThisNodeRight(int idOfNode, string typeOfNode, string typeOfNodes, string nameOfConnect)
        {
            ClientConnect();
            var forFindConnect = "(node:" + typeOfNode + ")-[:" + nameOfConnect + "]->(friend:" + typeOfNodes + ")";
            var collection = Client.Cypher
                .OptionalMatch(forFindConnect)
                .
                Where((MainCreater node) => node.id == idOfNode)
                .Return(friend => new {Friends = friend.CollectAs<MainCreater>()})
                .Results;

            return from el in collection from element in el.Friends select element.Data.id;
        }

        public static IEnumerable<int> GetAllIdNodesConnectionWithThisNodeLeft(int idOfNode, string typeOfNode, string typeOfNodes, string nameOfConnect)
        {
            ClientConnect();
            var forFindConnect = "(friend:" + typeOfNodes + ")-[:" + nameOfConnect + "]->(node:" + typeOfNode + ")";
            var collection = Client.Cypher
                .OptionalMatch(forFindConnect)
                .
                Where((MainCreater node) => node.id == idOfNode)
                .Return(friend => new { Friends = friend.CollectAs<MainCreater>() })
                .Results;

            return from el in collection from element in el.Friends select element.Data.id;
        }

        public static IEnumerable<string> GetAllNameNodesConnectionWithThisNodeRight(int idOfNode, string typeOfNode, string typeOfNodes, string nameOfConnect)
        {
            ClientConnect();
            var forFindConnect = "(node:" + typeOfNode + ")-[:" + nameOfConnect + "]->(friend:" + typeOfNodes + ")";
            var collection = Client.Cypher
                .OptionalMatch(forFindConnect)
                .
                Where((MainCreater node) => node.id == idOfNode)
                .Return(friend => new { Friends = friend.CollectAs<MainCreater>() })
                .Results;

            return from el in collection from element in el.Friends select element.Data.name;
        }
        public static IEnumerable<string> GetAllNameNodesConnectionWithThisNodeLeft(int idOfNode, string typeOfNode, string typeOfNodes, string nameOfConnect)
        {
            ClientConnect();
            var forFindConnect = "(node:" + typeOfNode + ")<-[:" + nameOfConnect + "]-(friend:" + typeOfNodes + ")";
            var collection = Client.Cypher
                .OptionalMatch(forFindConnect)
                .
                Where((MainCreater node) => node.id == idOfNode)
                .Return(friend => new { Friends = friend.CollectAs<MainCreater>() })
                .Results;

            return from el in collection from element in el.Friends select element.Data.name;
        }


        public static void DeleteOldCentroid(int idOfClassPrecedent)
        {
            const string stringForMatch = "(node:Centroid)-[CentroidIn]->(friend:ClassPrecedent)";
            Client.Cypher
            .OptionalMatch(stringForMatch)
            .Where((ClassPrecedent friend) => friend.id == idOfClassPrecedent)
            .Delete("CentroidIn, node")
            .ExecuteWithoutResults();
        }

        public static void ConnectNewCentroidWithClass(List<int> idsCentroid, int idClassPrecedent)
        {
            lock (Monitor)
            {
                Client.Connect();
            }

            foreach (var el in idsCentroid)
            {
                ConnectTwoNodes(el, "Centroid", idClassPrecedent, "ClassPrecedent", "CentroidIn");
            }
        }

        public static List<double> CreateKeyWords(List<KeyWord> list)
        {
            ClientConnect();
            var listForReturn = list.Select(el => CreateKeyWord(el.name, el.weight));
            return (List<double>) listForReturn;
        }

        public static void ConnectKeyWordsWithParametr(List<int> idsKeyWord, int idPrecedent )
        {
            lock (Monitor)
            {
                Client.Connect();
            }

            foreach (var el in idsKeyWord)
            {
                ConnectTwoNodes(el,"KeyWord", idPrecedent, "Precedent" , "KeyWordIn");
            }
        }
    }
}