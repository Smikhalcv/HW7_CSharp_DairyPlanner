using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DairyPlanner
{
    class DairyPlanner
    {
        private int indexNote = 0;
        private int selectionIndex;
        public Dictionary<int, Note> dictPlanner = new Dictionary<int, Note>();
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
            dictPlanner[indexNote] = note;
            ++indexNote;
        }

        /// <summary>
        /// Проверяет существует ли указанный файл и  возращет строку
        /// </summary>
        /// <returns>Путь если файл существует или null</returns>
        private string CheckExistsFile()
        {
            Console.WriteLine("Укажите путь к файлу");
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
            string path = CheckExistsFile();
            if (path != null)
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    while (!readFile.EndOfStream)
                    {
                        string name = readFile.ReadLine();
                        string title = readFile.ReadLine();
                        string content = readFile.ReadLine();
                        string dateMakeNote = readFile.ReadLine();
                        string dateExecutionNote = readFile.ReadLine();
                        try
                        {
                            Note note = new Note(name.Substring("Name: ".Length),
                                            title.Substring("Title: ".Length),
                                            content.Substring("Content: ".Length),
                                            Convert.ToDateTime(dateMakeNote.Substring("DateMakeNote: ".Length)),
                                            Convert.ToDateTime(dateExecutionNote.Substring("DateExecutionNote: ".Length))
                                            );
                            dictPlanner[indexNote] = note;
                            ++indexNote;
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Данные успешно добавленны из файла {path}");
            Console.ResetColor();
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

        /// <summary>
        /// Считывает из файла записи, даты создания или исполнения которых входят в указанный диапозон
        /// </summary>
        /// <param name="flagMode">если true выбирает из дат создания, если false из дат исполнения</param>
        public void AddNoteFromFileSelectionDateMake(bool flagMode)
        {
            bool flag = true;
            string path = CheckExistsFile();
            Console.WriteLine("Укажите диапозон дат для выборки");
            Console.WriteLine("Начала диапозона -");
            DateTime startRangeDate = GetDateFromConsole();
            Console.WriteLine("Конец диапозона -");
            DateTime endRangeDate = GetDateFromConsole();
            if (path != null)
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    while (!readFile.EndOfStream)
                    {
                        string name = readFile.ReadLine();
                        string title = readFile.ReadLine();
                        string content = readFile.ReadLine();
                        string dateMakeNote = readFile.ReadLine();
                        string dateExecutionNote = readFile.ReadLine();
                        try
                        {
                            Note note = new Note(name.Substring(patternForName.Length),
                                            title.Substring(patternForTitle.Length),
                                            content.Substring(patternForContant.Length),
                                            Convert.ToDateTime(dateMakeNote.Substring(patternForDateMakeNote.Length)),
                                            Convert.ToDateTime(dateExecutionNote.Substring(patternForDateExecutionNote.Length))
                                            );
                            if (flagMode)
                            {
                                if (note.DateMakeNote >= startRangeDate && note.DateMakeNote <= endRangeDate)
                                {
                                    dictPlanner[indexNote] = note;
                                    ++indexNote;
                                }
                            }
                            else
                            {
                                if (note.DateExecutionNote >= startRangeDate && note.DateExecutionNote <= endRangeDate)
                                {
                                    dictPlanner[indexNote] = note;
                                    ++indexNote;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Неверная структура данных в файле {path}");
                            flag = false;
                            break;
                            throw;
                        }
                    }
                }
            }
            else
            {
                flag = false;
            }
            if (flag)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Данные успешно добавленны из файла {path}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Редактирует поля темы, содержания и даты исполнения
        /// </summary>
        public void EditNote()
        {
            Console.WriteLine("Укажите номер записи, которую хотите изменить - ");
            int index = SelectIndexNote();
            if (index >= 0)
            {
                Console.WriteLine("Укажите поле которое желаете изменить");
                Console.WriteLine("(Имяб Тема, Содержание, Дата исполнения)");
                string mode = Console.ReadLine();
                switch (mode.ToLower())
                {
                    case "имя":
                        Console.WriteLine("Укажите новое значение -");
                        string newName = Console.ReadLine();
                        dictPlanner[index].Name = newName;
                        break;
                    case "тема":
                        Console.WriteLine("Укажите новое значение -");
                        string newTitle = Console.ReadLine();
                        dictPlanner[index].Title = newTitle;
                        break;
                    case "содержание":
                        Console.WriteLine("Укажите новое значение -");
                        string newContent = Console.ReadLine();
                        dictPlanner[index].Content = newContent;
                        break;
                    case "дата исполнения":
                        Console.WriteLine("Укажите новое значение -");
                        string newDateExecution = Console.ReadLine();
                        try
                        {
                            dictPlanner[index].DateMakeNote = Convert.ToDateTime(newDateExecution);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неверный формат даты! DD.MM.YYYY HH:MM");
                            throw;
                        }
                        break;

                    default:
                        Console.WriteLine("Такого поля нет!");
                        break;
                }
            }
        }

        /// <summary>
        /// Удаляет пару номер-запись из словаря ежедневника
        /// </summary>
        public void RemoveNote(int index)
        {
            dictPlanner.Remove(index);
            Console.WriteLine($"Запись под номером {index} была удалена");
        }

        /// <summary>
        /// Удаляет пару номер-запись из словаря ежедневника
        /// </summary>
        public void RemoveNote(string typeField, string valueForRemove)
        {
            DateTime dataForRemove;
            switch (typeField)
            {
                case "имя":
                    foreach (KeyValuePair<int, Note> note in dictPlanner)
                    {
                        if (note.Value.Name.ToLower() == valueForRemove.ToLower())
                        {
                            dictPlanner.Remove(note.Key);
                        }
                    }
                    break;
                case "заголовок":
                    foreach (KeyValuePair<int, Note> note in dictPlanner)
                    {
                        if (note.Value.Title.ToLower() == valueForRemove.ToLower())
                        {
                            dictPlanner.Remove(note.Key);
                        }
                    }
                    break;
                case "дата создания":
                    try
                    {
                        dataForRemove = Convert.ToDateTime(valueForRemove);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неверный формат даты");
                        break;
                        throw;
                    }
                    foreach (KeyValuePair<int, Note> note in dictPlanner)
                    {
                        if (note.Value.DateMakeNote == dataForRemove)
                        {
                            dictPlanner.Remove(note.Key);
                        }
                    }
                    break;
                case "дата исполнения":
                    try
                    {
                        dataForRemove = Convert.ToDateTime(valueForRemove);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неверный формат даты");
                        break;
                        throw;
                    }
                    foreach (KeyValuePair<int, Note> note in dictPlanner)
                    {
                        if (note.Value.DateExecutionNote == dataForRemove)
                        {
                            dictPlanner.Remove(note.Key);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Неверный тип поля!");
                    break;
            }
        }

        /// <summary>
        /// Записывает весь словарь ежедневника в файл
        /// </summary>
        public void WriteAllNotesToFile()
        {
            Console.WriteLine("Укажите путь к файлу -");
            string path = Console.ReadLine();
            if (path == "")
            {
                path = "DairyPlanner.txt";
            }
            if (!path.EndsWith(".txt"))
            {
                path += ".txt";
            }
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Данные успешно добавленны из файла {path}");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит отсортированный словарь по возрастанию указанного поля
        /// </summary>
        /// <param name="fieldNumber">Номер поля для сортировки</param>
        private void SortInAscendingOrder(int fieldNumber)
        {
            switch (fieldNumber)
            {
                case 1:
                    foreach (var item in dictPlanner.OrderBy(item => item.Value.Name))
                    {

                        item.Value.Print();
                    }
                    break;
                case 2:
                    foreach (var item in dictPlanner.OrderBy(item => item.Value.Title))
                    {
                        item.Value.Print();
                    }
                    break;
                case 3:
                    foreach (var item in dictPlanner.OrderBy(item => item.Value.Content))
                    {
                        item.Value.Print();
                    }
                    break;
                case 4:
                    foreach (var item in dictPlanner.OrderBy(item => item.Value.DateMakeNote))
                    {
                        item.Value.Print();
                    }
                    break;
                case 5:
                    foreach (var item in dictPlanner.OrderBy(item => item.Value.DateExecutionNote))
                    {
                        item.Value.Print();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Выводит отсортированный словарь по убыванию указанного поля
        /// </summary>
        /// <param name="fieldNumber">Номер поля для сортировки</param>
        private void SortInDescendingOrder(int fieldNumber)
        {
            switch (fieldNumber)
            {
                case 1:
                    foreach (var item in dictPlanner.OrderByDescending(item => item.Value.Name))
                    {
                        item.Value.Print();
                    }
                    break;
                case 2:
                    foreach (var item in dictPlanner.OrderByDescending(item => item.Value.Title))
                    {
                        item.Value.Print();
                    }
                    break;
                case 3:
                    foreach (var item in dictPlanner.OrderByDescending(item => item.Value.Content))
                    {
                        item.Value.Print();
                    }
                    break;
                case 4:
                    foreach (var item in dictPlanner.OrderByDescending(item => item.Value.DateMakeNote))
                    {
                        item.Value.Print();
                    }
                    break;
                case 5:
                    foreach (var item in dictPlanner.OrderByDescending(item => item.Value.DateExecutionNote))
                    {
                        item.Value.Print();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Запрашивает тип сортировки и номер поля по которому будет сортирвка и вызывает метод вывода отсортированного результата
        /// </summary>
        public void OrderingByField()
        {
            Console.WriteLine("Укажите тип сортирвки (возрастание/убывание)");
            string typeSort = Console.ReadLine();
            Console.WriteLine("Укажите поле для сортировки");
            Console.WriteLine("(Имя, Тема, Содержание, Дата создания, Дата исполнения)");
            string typeField = Console.ReadLine();
            switch (typeSort.ToLower())
            {
                case "возрастание":
                    switch (typeField.ToLower())
                    {
                        case "имя":
                            SortInAscendingOrder(1);
                            break;
                        case "тема":
                            SortInAscendingOrder(2);
                            break;
                        case "содержание":
                            SortInAscendingOrder(3);
                            break;
                        case "дата создания":
                            SortInAscendingOrder(4);
                            break;
                        case "дата исполнения":
                            SortInAscendingOrder(5);
                            break;
                        default:
                            Console.WriteLine("Неверный тип поля!");
                            break;
                    }
                    break;
                case "убывание":
                    switch (typeField.ToLower())
                    {
                        case "имя":
                            SortInDescendingOrder(1);
                            break;
                        case "тема":
                            SortInDescendingOrder(2);
                            break;
                        case "содержание":
                            SortInDescendingOrder(3);
                            break;
                        case "дата создания":
                            SortInDescendingOrder(4);
                            break;
                        case "дата исполнения":
                            SortInDescendingOrder(5);
                            break;
                        default:
                            Console.WriteLine("Неверный тип поля!");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Не верный режим сортировки!");
                    break;
            }
        }

        /// <summary>
        /// Проверяет существует ли запись с таким индексов в ежедневнике
        /// </summary>
        /// <returns>индекс записи - int</returns>
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
                    selectionIndex = -1;
                    ReadKeysValuesFromDictPlanner();
                }
            }
            catch (Exception)
            {
                selectionIndex = -1;
                Console.WriteLine("Вы указали неверный индекс");
            }
            return selectionIndex;
        }
        
        /// <summary>
        /// Выводит все записи в ежедневнике
        /// </summary>
        public void ReadDairyPlannerDict()
        {
            foreach (Note note in dictPlanner.Values)
            {
                note.Print();
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

        /// <summary>
        /// Выводит количество всех записей в ежедевнике
        /// </summary>
        public void CountNotes()
        {
            Console.WriteLine($"Количество записей - {dictPlanner.Count}");
        }

        /// <summary>
        /// Запрашивает индекс записи и выводит, если такова существует
        /// </summary>
        public void ReadOneNote()
        {
            Console.WriteLine("Введите номер записи");
            int indexNote = SelectIndexNote();
            if (indexNote > -1)
            {
                dictPlanner[indexNote].Print();
            }
        }
    }
}
