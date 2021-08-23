using System;

namespace DairyPlanner
{
    class Program
    {
        //DairyPlanner dairyPlanner = new DairyPlanner();
        static void Main(string[] args)
        {
            DairyPlanner dairyPlanner = new DairyPlanner();
            //dairyPlanner.AddNoteFromConsole();
            //dairyPlanner.AddNoteFromConsole();
            //Console.WriteLine("---------------------------");
            //dairyPlanner.ReadDairyPlannerDict();
            //dairyPlanner.CountNotes();
            dairyPlanner.AddNoteFromFile();
            dairyPlanner.ReadDairyPlannerDict();
            dairyPlanner.ReadKeysValuesFromDictPlanner();
            dairyPlanner.WriteAllNotesToFile();
            Console.WriteLine(dairyPlanner.dictPlanner[1].Title);
            string newValue = Console.ReadLine();
            dairyPlanner.dictPlanner[1].EditTitle(newValue);
            Console.WriteLine(dairyPlanner.dictPlanner[1].Title);
        }
    }
}
