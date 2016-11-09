using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2Vigenere
{
    class Program
    {
        private static string tablePath = "Tablica.txt";
        private static string explicitTextPath = "Plik.txt";

        private static Dictionary<char, int> alphabet = new Dictionary<char, int>
        {
            {'a', 0 },
            {'b', 1 },
            {'c', 2 },
            {'d', 3 },
            {'e', 4 },
            {'f', 5 },
            {'g', 6 },
            {'h', 7 },
            {'i', 8 },
            {'j', 9 },
            {'k', 10 },
            {'l', 11 },
            {'m', 12 },
            {'n', 13 },
            {'o', 14 },
            {'p', 15 },
            {'q', 16 },
            {'r', 17 },
            {'s', 18 },
            {'t', 19 },
            {'u', 20 },
            {'v', 21 },
            {'w', 22 },
            {'x', 23 },
            {'y', 24 },
            {'z', 25 }
        };

        private static string keyword = "tajne";
        private static char[,] table;
        private static string explicitText;

        static void Main(string[] args)
        {
            table = GetTable();
            explicitText = GetExplicitTextFromFile();
            var encodedText = EncodeText(explicitText, keyword);
            var decodedText = EncodeText(encodedText, RevertKeyword(keyword));

            Console.WriteLine("Encoded text: ");
            Console.WriteLine(encodedText);
            Console.WriteLine();
            Console.WriteLine(decodedText);

            Console.ReadLine();
        }

        private static char[,] GetTable()
        {
            using (StreamReader reader = new StreamReader(tablePath))
            {
                char[,] table = new char[alphabet.Count, alphabet.Count];
                string line = String.Empty;
                int x = 0;
                int y = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    foreach (var item in line)
                    {
                        table[x,y] = item;
                        x++;
                    }
                    x = 0;
                    y++;
                }

                return table;
            }
        }

        private static string GetExplicitTextFromFile()
        {
            if(File.Exists(explicitTextPath))
            {
                return File.ReadAllText(explicitTextPath);
            }

            return String.Empty;
        }

        private static List<KeyValuePair<char, char>> CreatePairs(string text, string keyword)
        {
            List<KeyValuePair<char, char>> pairs = new List<KeyValuePair<char, char>>();
            int i = 0;

            foreach (var item in text.ToLower())
            {
                if (!Char.IsWhiteSpace(item))
                {
                    if (i >= keyword.Length)
                    {
                        i = 0;
                    }
                    pairs.Add(new KeyValuePair<char, char>(item, keyword[i]));
                    i++;
                }
                else
                {
                    pairs.Add(new KeyValuePair<char, char>(item, item));
                }
            }

            return pairs;
        }

        private static string EncodeText(string text, string keyword)
        {
            StringBuilder encodedText = new StringBuilder();
            var pairs = CreatePairs(text, keyword);

            foreach (var item in pairs)
            {
                if (!Char.IsWhiteSpace(item.Key))
                {
                    int x = alphabet[item.Key];
                    int y = alphabet[item.Value];

                    encodedText.Append(table[x, y]);
                }
                else
                {
                    encodedText.Append(item.Key);
                }
            }

            return encodedText.ToString();
        }

        private static string RevertKeyword(string keyword)
        {
            StringBuilder revertedKeyword = new StringBuilder();

            foreach (var item in keyword)
            {
                var i = (alphabet.Count - alphabet[item]) % alphabet.Count;
                revertedKeyword.Append(alphabet.FirstOrDefault(x => x.Value == i).Key);
            }

            return revertedKeyword.ToString();
        }

        private static void PrintTable()
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    Console.Write(table[i, y] + " ");
                }
                Console.WriteLine();
            }
        }

        private static void PrintPairs(List<KeyValuePair<char, char>> pairs)
        {
            foreach (var item in pairs)
            {
                Console.Write(item.Key);
            }

            Console.WriteLine();

            foreach (var item in pairs)
            {
                Console.Write(item.Value);
            }
        }
    }
}
