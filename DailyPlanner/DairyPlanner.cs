using System;
using System.Collections.Generic;
using System.IO;

namespace DairyPlanner
{
    class DairyPlanner
    {
        private int index = 0;
        private int selectionIndex;
        public Dictionary<int, Note> dictPlanner = new Dictionary<int, Note>();
        //private List<Note> planner = new List<Note>();
        //private Note[] arrayNotes;
        private string patternForName = "Name: ";
        private string patternForTitle = "Title: ";
        private string patternForContant = "Contant: ";
        private string patternForDateMakeNote = "DateMakeNote: ";
        private string patternForDateExecutionNote = "DateExecutionNote: ";

        /// <summary>
        /// Добавляет запись в словарь dictPlanner используя структуру Note
        /// </summary>
        public void AddNoteFromConsole()
        {
            Note note;
            Console.WriteLine("Имя автора заметки -");
            string name = Console.ReadLine();
            Console.WriteLine("Тема заметки -");
            string title = Console.ReadLine();
            Console.WriteLine("Содержание заметки -");
            string content = Console.ReadLine();
            Console.WriteLine("Дата исполенения -");
            DateTime date;
            try
            {
                date = Convert.ToDateTime(Console.ReadLine());
                note = new Note(name, title, content, date);
            }
            catch (Exception)
            {
                note = new Note(name, title, content);
            }
            dictPlanner[index] = note;
            ++index;
        }

        /// <summary>
        /// Проверяет существует ли указанный файл и  возращет строку
        /// </summary>
        /// <returns>Путь если файл существует или null</returns>
        private string CheckExistsFile()
        {
            Console.WriteLine("Укажите путь к фйалу");
            string path = Console.ReadLine();
            if (!File.Exists(path))
            {
                path = null;
                Console.WriteLine("Неверный путь к файлу!");
            }
            return path;
        }

        /// <summary>
        /// Считывает файл и добавляет данные в словарь ежедневника
        /// </summary>
        public void AddNoteFromFile()
        {
            //string path = CheckExistsFile();
            string path = "DairyPlanner.txt";
            if (path != null)
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    while (!readFile.EndOfStream)
                    {
                        string name = readFile.ReadLine();
                        Console.WriteLine(name.Substring(patternForName.Length));
                        string title = readFile.ReadLine();
                        Console.WriteLine(title.Substring(patternForTitle.Length));
                        string content = readFile.ReadLine();
                        Console.WriteLine(content.Substring(patternForContant.Length));
                        string dateMakeNote = readFile.ReadLine();
                        Console.WriteLine(dateMakeNote.Substring(patternForDateMakeNote.Length));
                        string dateExecutionNote = readFile.ReadLine();
                        Console.WriteLine(dateExecutionNote.Substring(patternForDateExecutionNote.Length));
                        try
                        {
                            Note note = new Note(name.Substring("Name: ".Length),
                                            title.Substring("Title: ".Length),
                                            content.Substring("Content: ".Length),
                                            Convert.ToDateTime(dateMakeNote.Substring("DateMakeNote: ".Length)),
                                            Convert.ToDateTime(dateExecutionNote.Substring("DateExecutionNote: ".Length))
                                            );
                            dictPlanner[index] = note;
                            ++index;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Неверная структура данных в файле {path}");
                            break;
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получает начение из консоли и преобразует его в тип DateTime, 
        /// если ввод не корректен или пуст присваивает текущее значение даты и времени 
        /// </summary>
        /// <returns>Переменную типа DateTime</returns>
        private DateTime GetDateFromConsole()
        {
            DateTime specifiedDate;
            Console.WriteLine("Укажите дату в формате DD.MM.YYYY HH:MM");
            try
            {
                specifiedDate = Convert.ToDateTime(Console.ReadLine());
            }
            catch (Exception)
            {
                specifiedDate = DateTime.Now;
                throw;
            }
            return specifiedDate;
        }

        public void AddNoteFromFileSelectionDate()
        {
            bool flag = true;
            string path = CheckExistsFile();
            Console.WriteLine("Укажите диапозон дат для выборки");
            DateTime startRangeDate = GetDateFromConsole();
            DateTime endRangeDate = GetDateFromConsole();
            if (path != null)
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    while (!readFile.EndOfStream)
                    {
                        string name = readFile.ReadLine();
                        Console.WriteLine(name.Substring(patternForName.Length));
                        string title = readFile.ReadLine();
                        Console.WriteLine(title.Substring(patternForTitle.Length));
                        string content = readFile.ReadLine();
                        Console.WriteLine(content.Substring(patternForContant.Length));
                        string dateMakeNote = readFile.ReadLine();
                        Console.WriteLine(dateMakeNote.Substring(patternForDateMakeNote.Length));
                        string dateExecutionNote = readFile.ReadLine();
                        Console.WriteLine(dateExecutionNote.Substring(patternForDateExecutionNote.Length));
                        try
                        {
                            Note note = new Note(name.Substring("Name: ".Length),
                                            title.Substring("Title: ".Length),
                                            content.Substring("Content: ".Length),
                                            Convert.ToDateTime(dateMakeNote.Substring("DateMakeNote: ".Length)),
                                            Convert.ToDateTime(dateExecutionNote.Substring("DateExecutionNote: ".Length))
                                            );
                            if (note.DateMakeNote >= startRangeDate && note.DateMakeNote <= endRangeDate)
                            {
                                dictPlanner[index] = note;
                                ++index;
                            }
                            
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Неверная структура данных в файле {path}");
                            break;
                            throw;
                        }
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("Записей в данном диапозоне не найдено.");
            }
        }

        public void EditNote()
        {
            Console.WriteLine("Укажите номер записи, которую хотите изменить - ");
            int index = SelectIndexNote();
            if (index >= 0)
            {
                Console.WriteLine("Укажите поле которое желаете изменить");
                Console.WriteLine("(Тема, Содержание, Дата исполнения)");
                string mode = Console.ReadLine();
                switch (mode.ToLower())
                {
                    case "тема":
                        Console.WriteLine("Укажите новое значение -");
                        string newValue = Console.ReadLine();
                        dictPlanner[index].EditTitle(newValue);
                        break;
                    default:
                        Console.WriteLine("Такого поля нет!");
                        break;
                }
            }
        }

        public void RemoveNote()
        {
            Console.WriteLine("Укажите номер записи, которую хотите удалить - ");
            int index = SelectIndexNote();
            if (index >= 0)
            {
                dictPlanner.Remove(index);
                Console.WriteLine($"Запись под номером {index} была удалена");
            }
            else
            {
                Console.WriteLine("Удаление отмененно!");
            }
        }

        /// <summary>
        /// Записывает весь словарь ежедневника в файл
        /// </summary>
        public void WriteAllNotesToFile()
        {
            string path = "DairyPlanner2.txt";
            using (StreamWriter writeToFile = new StreamWriter(path, true))
            {
                foreach (Note note in dictPlanner.Values)
                {
                    writeToFile.WriteLine(patternForName + note.Name);
                    writeToFile.WriteLine(patternForTitle + note.Title);
                    writeToFile.WriteLine(patternForContant + note.Content);
                    writeToFile.WriteLine(patternForDateMakeNote + note.DateMakeNote);
                    writeToFile.WriteLine(patternForDateExecutionNote + note.DateExecutionNote);
                }
            }
        }

        public void OrderingByField()
        {

        }

        private int SelectIndexNote()
        {
            try
            {
                selectionIndex = Convert.ToInt32(Console.ReadLine());

                if (dictPlanner.ContainsKey(selectionIndex) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Отсутствует запись под таким номером!");
                    Console.WriteLine("Проерьте записи.");
                    Console.ResetColor();
                    ReadKeysValuesFromDictPlanner();
                }
            }
            catch (Exception)
            {
                selectionIndex = -1;
                Console.WriteLine("Вы указали неверный индекс");
                throw;
            }
            return selectionIndex;
        }
        
        public void ReadDairyPlannerDict()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------------------");
            Console.ResetColor();
            foreach (Note note in dictPlanner.Values)
            {
                note.Print();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------------------------------------------------");
                Console.ResetColor();
            }

        }

        /// <summary>
        /// Выводит номер записи (ключ) и сокращённое значения словаря включающее имя создателя записи, заголовок и дату создания
        /// </summary>
        public void ReadKeysValuesFromDictPlanner()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------------------");
            Console.ResetColor();
            foreach (KeyValuePair<int, Note> notes in dictPlanner)
            {
                Console.WriteLine($"{notes.Key} : {notes.Value.Name} - {notes.Value.Title} {notes.Value.DateMakeNote}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------------------------------------------------");
                Console.ResetColor();
            }
        }

        private Note SelectDictNote()
        {
            Console.WriteLine("Укажите номер записи, выбранной записи:");
            return dictPlanner[selectionIndex];
        }

        //private Note SelectArrayNote()
        //{
        //    return arrayNotes[selectionIndex];
        //}

        public void CountNotes()
        {
            Console.WriteLine($"Количество записей - {dictPlanner.Count}");
        }

        public void IndexNotesInDairy()
        {
            Console.WriteLine("Номера записей в Ежедневнике:");
            foreach  (int index in dictPlanner.Keys)
            {
                Console.WriteLine(index);
            }
        }
    }
}
