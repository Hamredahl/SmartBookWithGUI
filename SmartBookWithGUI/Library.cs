using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartBookWithGUI
{
    public class Library
    {
        //library set ska vara private, men blev problematiskt när tester skulle köras.
        public List<Book> library { get; set; }
        string fileLocation = "library.json";

        //Ladda Json fil för att fylla library vid start av programmet.
        public (bool, string) loadLibrary()
        {
            try
            {
                library = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(fileLocation));
                return (true, "Library loaded succesfully!");
            }
            //Vid fail, skapa en tom lista för att kunna köra programmet ändå.
            //TODO: Hantera risken av misslyckad load som leder till att ny Json sparar över äldre fil.
            catch (Exception e)
            {
                library = new List<Book>();
                return (false, "Could not load library from file!");
            }
        }
        //Spara nuvarande library till Json vid avslut av program.
        public string saveLibrary()
        {
            try
            {
                File.WriteAllText(fileLocation, JsonSerializer.Serialize(library));
                return "Library saved succesfully!";
            }
            catch (Exception e)
            {
                return "Could not save library to file!";
            }
        }
        //Metod för att skapa ny bok och lägga till i listan library. 
        public void addBook()
        {
            string title = "";
            string author = "";
            string category = "";
            string ISBN = "";

            //Varje while för att tvinga någorlunda korrekt input per parameter
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Fill in the title of the book.");
            bool validInput = false;
            while (!validInput)
            {
                Console.Write("Title: ");
                title = Console.ReadLine();
                Console.Clear();
                if (string.IsNullOrWhiteSpace(title)) Console.WriteLine("Title cannot be empty!");
                else validInput = true;
            }
            Console.Clear();
            Console.WriteLine("Fill in the author of the book.");
            validInput = false;
            while (!validInput)
            {
                Console.Write("Author: ");
                author = Console.ReadLine();
                Console.Clear();
                if (string.IsNullOrWhiteSpace(author)) Console.WriteLine("Author cannot be empty!");
                else validInput = true;
            }
            Console.Clear();
            Console.WriteLine("Fill in the category of the book.");
            validInput = false;
            while (!validInput)
            {
                Console.Write("Category: ");
                category = Console.ReadLine();
                Console.Clear();
                if (string.IsNullOrWhiteSpace(category)) Console.WriteLine("Category cannot be empty!");
                else validInput = true;
            }
            //För ISBN specifikt kollar vi att det är endast siffror, och 10 eller 13 stycken(ISBN10 eller ISBN13).
            Console.Clear();
            Console.WriteLine("Fill in the ISBN of the book.");
            validInput = false;
            while (!validInput)
            {
                Console.Write("ISBN: ");
                ISBN = Console.ReadLine();
                Console.Clear();
                if (!ISBNValidation(ISBN)) Console.WriteLine("Invalid ISBN, must be 10 or 13 digits only!");
                else if (library.Exists(book => book.ISBN == ISBN)) Console.WriteLine("ISBN already exists in the library!");
                else validInput = true;
            }

            library.Add(new Book(title, author, category, ISBN, true));
            Console.CursorVisible = false;
            Console.Clear();

            //Hjälpfunction för att validera ISBN
            bool ISBNValidation(string ISBN)
            {
                if (ISBN.Length == 10 || ISBN.Length == 13) return ISBN.All(Char.IsDigit);
                return false;
            }
        }
        public void removeBook(Book book)
        {
            library.Remove(book);
        }
    }
}
