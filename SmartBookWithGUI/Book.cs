using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBookWithGUI
{
    public class Book
    {
        public string title { get; }
        public string author { get; }
        public string category { get; }
        public string ISBN { get; }
        public bool isAvailable { get; set; }

        public Book(string title, string author, string category, string ISBN, bool isAvailable)
        {
            this.title = title;
            this.author = author;
            this.category = category;
            this.ISBN = ISBN;
            this.isAvailable = isAvailable;
        }
    }
}
