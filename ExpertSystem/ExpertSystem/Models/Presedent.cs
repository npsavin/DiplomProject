namespace ExpertSystem
{
    public class MainCreater
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Precedent: MainCreater
    {
       
    }

    public class StopWord : MainCreater
    {
        
    }

    public class KeyWord : MainCreater
    {
        public double weight { get; set; }
    }

    public class Centroid : MainCreater
    {
        public double weight { get; set; }
    }
    public class ClassPrecedent : MainCreater
    {
        public int countOfPrecedent { get; set; }
    }
    public class PrecedentFormVector
    {
        public double weight { get; set; }
        public string name { get; set; }
    }

}