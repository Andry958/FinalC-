using System.Runtime.InteropServices;
using ConsoleApp1;

internal class Program
{
    private static void Main(string[] args)
    {
        Dict dic = new Dict();
        string test = "--------------------------------------Load all words from file--------------------------------------";
        Console.WriteLine(test.Length);
        dic.Veiw();
        //dic.Change();
        dic.Sorted();
        //dic.Remove();
        dic.Veiw();
        //dic.Searche();
        /*        dic.DownloadInFile();
                dic.DownloadOneWord();*/
        //dic.LoadAllWordsFromFile();
        dic.Veiw();
        dic.Menu();
    }
}