using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Hjälpklass för att rendera det mesta av GUIn.  
namespace SmartBookWithGUI
{
    class SmartBookGUI
    {
        int vCoordinate = 0;
        int hCoordinate = 0;
        int padding = 20;
        public string[] headerMenuOptions = { "Search", "Add book", "Quit" };
        //Olika Updates bör vara det enda som kallas på utifrån, och de använder sedan privata klasser för att rita olika element
        public void Update(int vUpdate, int hUpdate, List<Book> libQ, string currentQuery)
        {
            vCoordinate = vUpdate;
            hCoordinate = hUpdate;
            Console.Clear();
            PrintHeaderMenu(currentQuery);
            PrintLibraryList(libQ);
        }
        //Specifikt headermenyn då den har horisontella menyval som behöver hanteras. Current Query är för att kunna behålla nuvarande sökord utskrivet efter varje update.
        private void PrintHeaderMenu(string currentQuery)
        {
            Console.WriteLine("Welcome to SmartBook!");
            Console.WriteLine();

            for (int i = 0; i < headerMenuOptions.Length; i++)
            {
                //Aktivt menyval, alltså det menyvalet som för närvarande är markerat, eftersom det är header ska den bara vara aktiv när den vertikala koordinaten är 0.
                //Behåller sin horisontella koordinat även om man går vertikalt i listan, för att återvända rätt när man kommer till v = 0 igen.
                if (vCoordinate == 0 && hCoordinate == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.Write(headerMenuOptions[i]);
                Console.ResetColor();

                if (i < headerMenuOptions.Length - 1)
                {
                    Console.CursorLeft += 2;
                    Console.Write("|");
                    Console.CursorLeft += 2;
                } else Console.WriteLine();
            }
            Console.WriteLine(currentQuery);
        }
        //Listan av böcker som nuvarande searchquery av library innehåller.
        private void PrintLibraryList(List<Book> libToPrint)
        {
            Console.WriteLine();
            Console.WriteLine("Title".PadRight(padding) + "Author".PadRight(padding) + "Category".PadRight(padding) + "ISBN".PadRight(padding) + "Availability");
            Console.WriteLine();

            if (libToPrint.Count == 0) Console.WriteLine("Could not find any books.");
            else
            {
                for (int i = 0; i < libToPrint.Count; i++)
                {
                    //markering av aktiv bok. Eftersom inget horisontellt val finns i boklistan behöver inte hcoordinat tas i hänsyn.
                    if (vCoordinate - 1 == i)
                    {
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                    }
                    //Hantering av för långa strängar i titel, författare, och kategori. Eftersom ISBN och Availability är ganska statiska storlekar hanteras inte det. 
                    //Just nu hanteras strängar som är ängre än 15 tecken, vore bättre att inte hårdkoda utan arbeta med padding-värdet.
                    Console.Write(libToPrint[i].title.Length < padding - 5 ? libToPrint[i].title : (libToPrint[i].title.Substring(0, 15) + "..."));
                    Console.ResetColor();
                    (int _, int top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(padding, top);
                    Console.Write((libToPrint[i].author.Length < padding - 5 ? libToPrint[i].author.PadRight(padding) : (libToPrint[i].author.Substring(0, 15) + "...").PadRight(padding))
                        + (libToPrint[i].category.Length < padding - 5 ? libToPrint[i].category.PadRight(padding) : (libToPrint[i].category.Substring(0, 15) + "...").PadRight(padding))
                        + libToPrint[i].ISBN.PadRight(padding));
                    Console.ForegroundColor = libToPrint[i].isAvailable ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine(libToPrint[i].isAvailable ? "Available" : "Loaned out");
                    Console.ResetColor();
                }
            }
        }
        //GUI update för specifika menyn när man "öppnar" en bok. 
        public void BookUpdate(int tempVCoord, Book book)
        {
            Console.Clear();
            Console.WriteLine(book.title);
            Console.WriteLine("by " + book.author);
            Console.WriteLine("Category: " + book.category);
            Console.WriteLine("ISBN" + (book.ISBN.Length == 10 ? "10" : "13") + ": " + book.ISBN);
            Console.ForegroundColor = book.isAvailable ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(book.isAvailable ? "Available" : "Loaned out");
            Console.ResetColor();
            Console.WriteLine();


            string[] bookMenuOptions = { (book.isAvailable ? "Loan out" : "Return"), "Remove", "Exit" };
            for (int i = 0; i < bookMenuOptions.Length; i++)
            {
                if (tempVCoord == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.Write(bookMenuOptions[i]);
                if (i < bookMenuOptions.Length - 1) Console.CursorLeft += 4;
                Console.ResetColor();
            }
        }
    }
}
