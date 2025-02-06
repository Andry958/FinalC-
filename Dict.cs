using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        public string fnameUa = "../../../DictUa.json";
        public string fname_Ua = "../../../OneWordUa.json";
        public bool type;//false Eng -> ukr, true Ukr -> Eng
/*        public Dict(bool type)
        {
            type = type;
            if (type)
            {
                dic.Add("яблуко", new List<string> { "apple" });
                dic.Add("банан", new List<string> { "banana" });
                dic.Add("вишня", new List<string> { "cherry" });
                dic.Add("собака", new List<string> { "dog" });
                dic.Add("кіт", new List<string> { "cat" });
                dic.Add("дерево", new List<string> { "tree" });
                dic.Add("автомобіль", new List<string> { "car" });
                dic.Add("будинок", new List<string> { "house" });
                dic.Add("книга", new List<string> { "book" });
                dic.Add("комп'ютер", new List<string> { "computer" });

            }
            else
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

        }*/
        public Dict() { }
        private bool IsUkrainian(string text)
        {
            return text.Any(c => c >= 'А' && c <= 'Я' || c >= 'а' && c <= 'я' || c == 'Є' || c == 'є' || c == 'І' || c == 'і' || c == 'Ї' || c == 'ї' || c == 'Ґ' || c == 'ґ');
        }
        public void setType()
        {
            //Console.Write(type);
            Console.WriteLine("When you change type, all words remove");
            Console.WriteLine("Choose type dictionary(1: Eng -> ukr, 2: Ukr -> Eng) -> ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice) {
                case 1:
                    type = false;
                    dic.Clear();
                    break;
                case 2:
                    type = true;
                    dic.Clear();
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
                Console.Write(@"    1. Choose type dictionary
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
            Console.WriteLine("---------------------------------View all words---------------------------------");
            if(dic.Count == 0) { Console.WriteLine("\tDictionary is empty;("); return; }
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
            Console.Write("Enter word -> ");
            string key_ = Console.ReadLine();
            if (!UkrEng(key_)) return;

            Console.Write("Enter translate words (a, b) -> ");
            List<string> value_ = Console.ReadLine().Split(", ").ToList();

            dic[key_] = value_;
            Sorted();

        }
        public void Add(string key, List<string> value)
        {
            Console.WriteLine($"------------------------------------word {key}------------------------------------");
            dic[key] = value;
        }
        private bool UkrEng(string text)
        {
            bool isUkr = IsUkrainian(text);
            if (type && !isUkr)
            {
                Console.WriteLine("Your dictionary: Ukr -> Eng");
                return false;
            }
            if (!type && isUkr)
            {
                Console.WriteLine("Your dictionary: Eng -> Ukr");
                return false;
            }
            return true;
        }
        public void Change()
        {
            Console.WriteLine("----------------------------------Change words----------------------------------");
            Console.Write("\tchange word - 1; change translate words - 2; Enter -> ");
            int tmp = int.Parse(Console.ReadLine());
            switch (tmp)
            {
                case 1:
                    Console.Write("\tEnter old word -> "); string OldWord = Console.ReadLine();
                    if (!UkrEng(OldWord)) { Change(); return; }
                    if (!dic.ContainsKey(OldWord)) { Console.WriteLine("\tWord not found"); return; }

                    Console.Write("\tEnter new word -> "); string NewWord = Console.ReadLine();
                    if (!UkrEng(NewWord)) { Change(); return; }

                    dic[NewWord] = dic[OldWord];
                    dic.Remove(OldWord);
                    break;
                case 2:
                    Console.Write("\tEnter word -> "); string OldWord_ = Console.ReadLine();
                    if (!UkrEng(OldWord_)) { Change(); return; }
                    if (!dic.ContainsKey(OldWord_)) { Console.WriteLine("\tWord not found"); return; }

                    Console.Write("\tEnter new translate words -> "); List<string> NewWords = Console.ReadLine().Split(", ").ToList();
                    if (!UkrEng(NewWords[0])) { Change(); return; }

                    dic[OldWord_] = NewWords;
                    break;
            }
            Sorted();
        }
        public void Remove()
        {
            Console.WriteLine("--------------------------Remove words and translation--------------------------");
            Console.Write("\tEnter word -> "); string Word = Console.ReadLine();
            if (!UkrEng(Word)) { Change(); return; }

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
                            // залишити лише перший переклад
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
            Console.WriteLine("------------------------------------Searche-------------------------------------");
            Console.Write("\tEnter word -> "); string Word = Console.ReadLine();
            if (!UkrEng(Word)) { return; }

            if (!dic.ContainsKey(Word))
            { Console.WriteLine("\tWord not found"); return; }
            Console.Write("found -> "); Veiw(Word);
        }
        public void WriteInFile()
        {
            Console.WriteLine("----------------------------------Write in file---------------------------------");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };

            string json = JsonSerializer.Serialize(dic, options);
            if (type) { File.WriteAllText(fnameUa, json); }
            else { File.WriteAllText(fname, json); }
            
            Console.WriteLine("\tdata saved to file in JSON format");
        }
        public void WriteOneWord()
        {
            Console.WriteLine("----------------------------------Write in file----------------------------------");

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
            /*            if (!type) { File.AppendAllText("../../../word.json", json + Environment.NewLine); }
                        else { File.AppendAllText("../../../wordUa.json", json + Environment.NewLine); }*/
            if (!type) { File.AppendAllText(fname_, json + Environment.NewLine); }
            else { File.AppendAllText(fname_Ua, json + Environment.NewLine); }

            Console.WriteLine("\tdata saved to file in JSON format");
        }
        public void LoadAllWordsFromFile()
        {
            Console.WriteLine("----------------------------Load all words from file----------------------------");

            if (!File.Exists(fname))
            {
                Console.WriteLine("\tfile does not exist");
                return;
            }
            string path;
            if (type){path = File.ReadAllText(fnameUa); }
            else{ path = File.ReadAllText(fname);}
            string json = path;
            dic = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            Console.WriteLine("\tdata loaded from file in JSON format");
        }

    }
}
/*git remote add origin https://github.com/Andry958/Final-Project-c-sharp.git
git branch -M main
git push -u origin main
*/