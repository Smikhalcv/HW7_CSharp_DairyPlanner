using System;
using System.Collections.Generic;
using System.Text;

namespace DairyPlanner
{
    class Note
    {
        private string name;
        public string Name { get { return this.name; } set { this.name = value; } }
        private string title;
        public string Title { get { return this.title; } set { this.title = value; } }
        private string content;
        public string Content { get { return this.content; } set { this.content = value; } }
        public DateTime DateMakeNote { get; set; }
        public DateTime DateExecutionNote { get; set; }
        
        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine($"{"Имя записавшего", 15} {this.Name, 15}\n" +
                $"{"Тема", 15} {this.Title, 15}\n" +
                $"Содержание записи: \n" +
                $"{this.Content}\n" +
                $"Дата записи - {this.DateMakeNote}\n" +
                $"Дата исполнения - {this.DateExecutionNote}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.ResetColor();
        }

        public Note(string Name, string Title, string Content, DateTime DateMakeNote, DateTime DateExecutionNote)
        {
            this.Name = Name;
            this.title = Title;
            this.Content = Content;
            this.DateExecutionNote = DateExecutionNote;
            this.DateMakeNote = DateMakeNote;
        }

        public Note(string Name, string Title, string Content, DateTime DateExecutionNote) :
            this(Name, Title, Content, DateTime.Now, DateExecutionNote)
        { }

        public Note(string Name, string Title, string Content) :
            this(Name, Title, Content, DateTime.Now)
        { }

        public Note(string Name, string Content) :
            this(Name, "None", Content)
        { }

        public Note(string Content) :
            this("None", Content)
        { }
    }
}
