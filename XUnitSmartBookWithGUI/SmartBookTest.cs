using SmartBookWithGUI;

namespace XUnitSmartBookWithGUI

{
    public class SmartBookTest
    {
        [Fact]
        public void RemoveBookTest()
        {
            Library lib = new Library();
            Book book1 = new Book("test1", "test1", "test1", "1234567890", true);
            lib.library.Add(book1);
            Book book2 = new Book("test2", "test2", "test2", "1234567891", true);
            lib.library.Add(book2);
            Book book3 = new Book("test3", "test3", "test3", "1234567892", true);
            lib.library.Add(book3);

            Assert.Contains(book1, lib.library);
            Assert.Contains(book2, lib.library);
            Assert.Contains(book3, lib.library);

            lib.removeBook(book1);

            Assert.DoesNotContain(book1, lib.library);
            Assert.Contains(book2, lib.library);
            Assert.Contains(book3, lib.library);
        }
        [Fact]
        public void SortBookTest()
        {
            var smartBook = new SmartBookRun();
            var lib = new Library();
            var book1 = new Book("ctest1", "123", "test1", "1234567890", true);
            lib.library.Add(book1);
            var book2 = new Book("btest2", "123", "test2", "1234567891", true);
            lib.library.Add(book2);
            var book3 = new Book("atest3", "456", "test3", "1234567892", true);
            lib.library.Add(book3);

            List<Book> sortedList = smartBook.sortLib("123");

            Assert.Contains(book1, sortedList);
            Assert.Contains(book2, sortedList);
            Assert.DoesNotContain(book3, sortedList);

            List<Book> sortedList2 = smartBook.sortLib("test");

            Assert.Contains(book1, sortedList);
            Assert.Contains(book2, sortedList);
            Assert.Contains(book3, sortedList);
        }
    }
}
