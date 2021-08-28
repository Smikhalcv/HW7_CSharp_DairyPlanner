using System;
using System.Linq;

namespace DairyPlanner
{
    class Program
    {
        //DairyPlanner dairyPlanner = new DairyPlanner();
        static void Main(string[] args)
        {
            DairyPlanner dairyPlanner = new DairyPlanner();
            bool flag = true;
            int mode;
            while (flag)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("Просмотреть записи ежедневника - 1\n" +
                    "Просмотреть номера и краткие сведения о записи - 2\n" +
                    "Просмотреть выбранную записи - 3\n" +
                    "Внести новую запись из консоли - 4\n" +
                    "Внести новую запись из файла - 5\n" +
                    "Редактировать запись - 6\n" +
                    "Удалить запись - 7\n" +
                    "Загрузка записей из файла по диапазону дат создания - 8\n" +
                    "Загрузка записей из файла по диапазону дат исполнения - 9\n" +
                    "Записать все записи в файл - 10\n" +
                    "Упорядочивание записей по выбранному полю - 11\n" +
                    "Выход - 12");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();
                try
                {
                    mode = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write("Ошибка ввода!");
                    throw;
                }
                if (mode >= 1 && mode <= 11)
                {
                    switch (mode)
                    {
                        case 1:
                            dairyPlanner.ReadDairyPlannerDict();
                            dairyPlanner.CountNotes();
                            break;
                        case 2:
                            dairyPlanner.ReadKeysValuesFromDictPlanner();
                            break;
                        case 3:
                            dairyPlanner.ReadOneNote();
                            break;
                        case 4:
                            dairyPlanner.AddNoteFromConsole();
                            break;
                        case 5:
                            dairyPlanner.AddNoteFromFile();
                            break;
                        case 6:
                            dairyPlanner.EditNote();
                            break;
                        case 7:
                            Console.WriteLine("Укажите номер записи или название поля - ");
                            Console.WriteLine("Название полей - Имя, Заголовок, Дата создания, Дата исполнения");
                            string noteForRemove = Console.ReadLine();
                            try
                            {
                                int indexForRemove = Convert.ToInt32(noteForRemove);
                                dairyPlanner.RemoveNote(indexForRemove);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Укажите записи с каким значением нужно удалить - ");
                                string valueForRemove = Console.ReadLine();
                                dairyPlanner.RemoveNote(noteForRemove, valueForRemove);
                            }
                            break;
                        case 8:
                            dairyPlanner.AddNoteFromFileSelectionDateMake(true);
                            break;
                        case 9:
                            dairyPlanner.AddNoteFromFileSelectionDateMake(false);
                            break;
                        case 10:
                            dairyPlanner.WriteAllNotesToFile();
                            break;
                        case 11:
                            dairyPlanner.OrderingByField();
                            break;
                        case 12:
                            flag = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Не верно выбранное действие!");
                }
            }
        }
    }
}
