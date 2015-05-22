using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExpertSystem.Logic
{
    public static class WorkKeyWord
    {
        public static List<string> FindKeyWord(string text)
        {


            var dictionary = new Dictionary<string, int>();


            var words = text.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            var stopWords =
                File.ReadAllText(@"E:\Users\Nikita\OneDrive\Documents\Study\Diplom\MyProject\ExpertSystem\ExpertSystem\StopWords.txt"
                    ).Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);


            foreach (var word in words.Select(word => new string((word).Where(char.IsLetter).ToArray()).ToLower())
                    .Where(wordForReturn => !stopWords.Contains(wordForReturn) && wordForReturn.Count() > 1))
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
            var limitCicle = Math.Min(words.Count(), 10);
            // ReSharper disable once PossibleLossOfFraction
            while (b / (words.Length + 1) < 0.05 && counter < limitCicle)
            {
                var first = new KeyValuePair<string, int>();
                foreach (var x in dictionary.Where(x => x.Value == dictionary.Values.Max()).Where(x => x.Value > 1))
                {
                    first = x;
                    break;
                }
                var key = first.Key;
                keyWords.Add(key);
                var first1 = new KeyValuePair<string, int>();
                foreach (var x in dictionary.Where(x => x.Value == dictionary.Values.Max()).Where(x => x.Value > 1))
                {
                    first1 = x;
                    break;
                }
                var a = first1.Value;
                b = b + a;
                if (key != null) dictionary.Remove(key);
                counter++;
            }
            var sb = new StringBuilder();

            foreach (var keyWord in keyWords)
            {
                sb.AppendLine(keyWord);
            }
            var keyWordsWithoutRepet = new StringBuilder();
            var finelKeyWords = sb.ToString().Split(default(char[]), StringSplitOptions.RemoveEmptyEntries).Distinct();
            foreach (var variable in finelKeyWords)
            {
                keyWordsWithoutRepet.AppendLine(variable);
            }

            StringBuilder normalFormKeyWord = new StringBuilder();
            foreach (var el in keyWordsWithoutRepet.ToString().Split(default(char[]), StringSplitOptions.RemoveEmptyEntries))
            {
                normalFormKeyWord.AppendLine(GetForm.GetNormalForm(el));
            }
            var normalFormWithoutRepetList = new List<string>();
            var normalFormWithoutRepet = normalFormKeyWord.ToString().Split(default(char[]), StringSplitOptions.RemoveEmptyEntries).Distinct();
            foreach (var variable in normalFormWithoutRepet)
            {
                normalFormWithoutRepetList.Add(variable);
            }


            
            keyWordsWithoutRepet.Clear();
            normalFormKeyWord.Clear();
            sb.Clear();
            return normalFormWithoutRepetList;

        }
    }
}