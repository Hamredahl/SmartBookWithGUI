using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBookWithGUI
{
    public class SmartBookRun
    {
        SmartBookGUI gui = new SmartBookGUI();
        Library lib = new Library();
        List<Book> queryLib;
        int vCoordinate = 0;
        int hCoordinate = 0;
        string currentQuery = "";

        //Vid start laddar vi tidigare library, eller ger felmeddelande om det inte går.
        //Eftersom currentQuery är en tom sträng, kommer nuvarande sortering att endast sortera listan i namnordning och visa alla objekt i listan.
        public SmartBookRun()
        {
            (bool loaded, string lMessage) = lib.loadLibrary();
            if (!loaded)
            {
                Console.WriteLine(lMessage);
                Console.WriteLine("Press any button to continue");                
                Console.ReadKey();
            }
            queryLib = sortLib(currentQuery);
        }

        public void Run()
        {           
            bool isRunning = true;
            Console.CursorVisible = false;
            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);

            //Switch för att läsa knapptryck och uppdatera både den grafiska representationen av aktivt menyval, och nuvarande koordinatinfo
            do
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (hCoordinate < gui.headerMenuOptions.Length - 1 && vCoordinate < 1)
                        {
                            hCoordinate++;
                            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (hCoordinate > 0 && vCoordinate < 1)
                        {
                            hCoordinate--;
                            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (vCoordinate < queryLib.Count)
                        {
                            vCoordinate++;
                            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (vCoordinate > 0)
                        {
                            vCoordinate--;
                            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                        }
                        break;
                    //Switch vid knapptryck enter. Först ser vi om vi är på huvudmenyn, och om så vilket horisontellt värde vi har.
                    case ConsoleKey.Enter:
                        if (vCoordinate == 0)
                        {
                            switch (hCoordinate)
                            {
                                //Search, Skrivs i samma fönster som huvud-GUIn.
                                //TODO: Se om det går att få bort hårdkodningen av plats att skriva på, nuvarande setcursorpos är ganska klumpig.
                                case 0:
                                    Console.Clear();
                                    currentQuery = "";
                                    gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                                    Console.SetCursorPosition(0, 3);
                                    Console.CursorVisible = true;                                    
                                    currentQuery = Console.ReadLine();
                                    Console.CursorVisible = false;
                                    queryLib = sortLib(currentQuery);
                                    Console.Clear();
                                    gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                                    break;
                                //Add book, öppnar ett nytt "fönster" för simpelhetens skull.
                                case 1:
                                    //TODO: Status message for GUI update? ergo, Book added below searchquery maybe.
                                    lib.addBook();
                                    queryLib = sortLib(currentQuery);
                                    gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                                    break;
                                //Quit, sparar library till Json och stänger sedan applikationen.
                                case 2:
                                    //TODO: Wish to save prompt?
                                    Console.Clear();                                    
                                    Console.WriteLine(lib.saveLibrary());
                                    isRunning = false;
                                    break;
                            }
                        }
                        //Om det vertikala värdet är mer än 0 är vi i boklistan, och behöver inte bry oss om det horisontella värdet. Eftersom listan presenteras i samma ordning som dess
                        //index i stigande ordning, och vKoordinaten gör samma med värde 1 från första boken, kan vi lätt hitta index med -1 av nuvarande koordinat. Enter öppnar "bok"-meny.
                        else
                        {
                            OpenBook(vCoordinate - 1);
                            queryLib = sortLib(currentQuery);
                            gui.Update(vCoordinate, hCoordinate, queryLib, currentQuery);
                        }
                        break;
                    //Debug-knapp för att stänga av programmet, bör tas bort eller återanvändas till annat.
                    case ConsoleKey.Escape:
                        isRunning = false;
                        break;
                }
            } while (isRunning);
        }
        //En egen "Run" för specifikt böcker, snarlik tidigare.
        private void OpenBook(int bookIndex)
        {
            Book book = queryLib[bookIndex];
            int tempHCoord = 0;
            bool bookOpen = true;
            gui.BookUpdate(tempHCoord, book);

            do
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (tempHCoord < 3)
                        {
                            tempHCoord++;
                            gui.BookUpdate(tempHCoord, book);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (tempHCoord > 0)
                        {
                            tempHCoord--;
                            gui.BookUpdate(tempHCoord, book);
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (tempHCoord) 
                        {
                            case 0:
                                book.isAvailable = book.isAvailable ? false : true;
                                gui.BookUpdate(tempHCoord, book);
                                break;
                            case 1:
                                lib.removeBook(book);
                                vCoordinate--;
                                bookOpen = false;
                                break;
                            case 2:
                                bookOpen = false;
                                break;
                        }
                        break;
                }
            } while (bookOpen);
        }
        //Sortering av library, eftersom den bör fortsätta existera efter updates sätts den på en avskiljd lista från huvudlibrary och är den som alltid visas,
        //library i sig agerar endast databas.
        public List<Book> sortLib(string query)
        {
            List<Book> tempLib = new List<Book>();

            var bookQuery = lib.library
                .Where(b => containsString(b, query))
                .OrderBy(b => b.title);

            foreach (Book book in bookQuery)
            {
                tempLib.Add(book);
            }
            return tempLib;

            static bool containsString(Book book, string toCheck)
            {
                if (book.title.ToLower().Contains(toCheck.ToLower()) || book.author.ToLower().Contains(toCheck.ToLower())) return true;
                return false;
            }
        }
    }
}
