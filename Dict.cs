using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    [Serializable]
    public class Dict
    {
        //public List<Dictionary<string, string[]>> list = new List<Dictionary<string, string[]>>();
        public Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        public string fname = "../../../Dict.json";
        public string fname_ = "../../../OneWord.json";
        public bool type = false;//false Eng -> ukr, true Ukr -> Eng
        public Dict()
        {
            dic.Add("apple", new List<string> { "яблуко" });
            dic.Add("banana", new List<string> { "банан" });
            dic.Add("cherry", new List<string> { "вишня" });
            dic.Add("dog", new List<string> { "собака" });
            dic.Add("cat", new List<string> { "кіт" });
            dic.Add("tree", new List<string> { "дерево" });
            dic.Add("car", new List<string> { "автомобіль" });
            dic.Add("house", new List<string> { "будинок" });
            dic.Add("book", new List<string> { "книга" });
            dic.Add("computer", new List<string> { "комп'ютер" });
        }
        public void setType()
        {
            Console.WriteLine("Choose type dictionary(1: Eng -> ukr, 2: Ukr -> Eng) -> ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice) {
                case 1:
                    type = false;
                    break;
                case 2:
                    type = true;
                    break;
                default:
                    Console.WriteLine("you have selected a number not from the list!");
                    break;
            }
        }
        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------Menu--------------------------------------");
                Console.Write(@"    1 .Choose type dictionary
    2. Add words
    3. Change words
    4. Remove words
    5. Search word and its translation 
    6. Save in file
    7. Save one words
    8. Load from file
    9. Veiw
    0. Exit
        Choose num -> "); int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        setType();
                        break;
                    case 2:
                        Add();
                        break;
                    case 3:
                        Change();
                        break;
                    case 4:
                        Remove();
                        break;
                    case 5:
                        Searche();
                        break;
                    case 6:
                        WriteInFile();
                        break;
                    case 7:
                        WriteOneWord();
                        break;
                    case 8:
                        LoadAllWordsFromFile();
                        break;
                    case 9:
                        Veiw();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }

        }
        public void Veiw()
        {
            Console.WriteLine("--------------------------------------View all words--------------------------------------");
            foreach (var i in dic)
            {
                Console.WriteLine($"\t{i.Key,-10} -> {string.Join(", ", i.Value)}");
            }
        }
        public void Veiw(string key)
        {
            Console.WriteLine($"{key}: {string.Join(", ", dic[key])}");
        }
        public void Add()
        {
            Console.WriteLine("--------------------------------------Add word--------------------------------------");

            Console.Write("Enter word -> ");
            string key_ = Console.ReadLine();
            Console.WriteLine();
            //----
            Console.Write("Enter translate words -> ");
            List<string> value_ = Console.ReadLine().Split(", ").ToList();

            /*            List<string> value_ = new List<string>();
                        while (true) {
                            string tmp = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(tmp)) break;
                            value_.Add(tmp);
                        }*/
            dic[key_] = value_;
            Sorted();

        }
        public void Add(string key, List<string> value)
        {
            Console.WriteLine($"--------------------------------------word {key}--------------------------------------");
            dic[key] = value;
        }
        public void Change()
        {
            Console.WriteLine("--------------------------------------Change words--------------------------------------");

            Console.Write("\tchange word - 1; change translate words - 2; Enter -> ");
            int tmp = int.Parse(Console.ReadLine());
            switch (tmp)
            {
                case 1:
                    Console.Write("\tEnter old word -> "); string OldWord = Console.ReadLine();
                    if (!dic.ContainsKey(OldWord))
                    { Console.WriteLine("\tWord not found"); return; }

                    Console.Write("\tEnter new word -> "); string NewWord = Console.ReadLine();
                    dic[NewWord] = dic[OldWord];
                    dic.Remove(OldWord);
                    break;
                case 2:
                    Console.Write("\tEnter word -> "); string OldWord_ = Console.ReadLine();
                    if (!dic.ContainsKey(OldWord_))
                    { Console.Write("\tWord not found"); return; }

                    Console.Write("\tEnter new translate words -> "); List<string> NewWords = Console.ReadLine().Split(", ").ToList();
                    ;
                    dic[OldWord_] = NewWords;
                    break;
            }
            Sorted();
        }
        public void Remove()
        {
            Console.WriteLine("--------------------------------------Remove words and translation--------------------------------------");
            Console.Write("\tEnter word -> ");
            string Word = Console.ReadLine();
            Console.Write("\tremove word - 1; remove tranlation - 2\nYour choice ->"); int word = int.Parse(Console.ReadLine());
            switch (word)
            {
                case 1:
                    dic[Word].Clear();
                    dic.Remove(Word);
                    break;
                case 2:
                    if (dic[Word].Count <= 1)
                    {
                        Console.WriteLine("\tit is not possible to delete the translation of a word if it has only one translation");
                        return;
                    }

                    Console.Write("\tleave one translation - all; remove translation for number - one; Enter -> ");
                    string choiceRemove = Console.ReadLine();

                    switch (choiceRemove.ToLower())
                    {
                        case "all":
                            // Залишити лише перший переклад
                            string tmp = dic[Word][0];
                            dic[Word].Clear();
                            dic[Word].Add(tmp);
                            Console.WriteLine("\tall translations removed, only one translation left");
                            break;
                        case "one":
                            for (int i = 0; i < dic[Word].Count; i++)
                            {
                                Console.WriteLine($"\t{i + 1}. {dic[Word][i]}");
                            }

                            Console.Write("\tEnter the number -> ");
                            int index = int.Parse(Console.ReadLine()) - 1;

                            if (index >= 0 && index < dic[Word].Count)
                            {
                                dic[Word].RemoveAt(index);
                                Console.WriteLine($"\ttranslation '{dic[Word][index]}' removed");
                            }
                            else
                            {
                                Console.WriteLine("\tinvalid index");
                            }
                            break;

                        default:
                            Console.WriteLine("\tinvalid choice");
                            break;
                    }
                    break;
            }
            Sorted();
        }
        public void Sorted()
        {
            dic = dic.OrderBy(i => i.Key).ToDictionary(i => i.Key, i => i.Value);

        }
        public void Searche()
        {
            Console.WriteLine("--------------------------------------Searche--------------------------------------");
            Console.Write("\tEnter word -> "); string Word = Console.ReadLine();
            if (!dic.ContainsKey(Word))
            { Console.WriteLine("\tWord not found"); return; }
            Console.Write($"found -> "); Veiw(Word);
        }
        public void WriteInFile()
        {
            Console.WriteLine("--------------------------------------Write in file--------------------------------------");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };

            string json = JsonSerializer.Serialize(dic, options);
            File.WriteAllText(fname, json);
            Console.WriteLine("\tdata saved to file in JSON format");
        }
        public void WriteOneWord()
        {
            Console.WriteLine("--------------------------------------Write in file--------------------------------------");

            Console.Write("\tEnter word -> "); string Word = Console.ReadLine();
            if (!dic.ContainsKey(Word))
            { Console.WriteLine("\tWord not found"); return; }
            var wordData = new Dictionary<string, List<string>>()
    {
        { Word, dic[Word] }
    };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(wordData, options);
            File.AppendAllText("../../../word.json", json + Environment.NewLine);

            Console.WriteLine("\tdata saved to file in JSON format");
        }
        public void LoadAllWordsFromFile()
        {
            Console.WriteLine("--------------------------------------Load all words from file--------------------------------------");

            if (!File.Exists(fname))
            {
                Console.WriteLine("\tfile does not exist");
                return;
            }

            string json = File.ReadAllText(fname);

            dic = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            Console.WriteLine("\tdata loaded from file in JSON format");
        }

    }
}
/*git remote add origin https://github.com/Andry958/Final-Project-c-sharp.git
git branch -M main
git push -u origin main
*/