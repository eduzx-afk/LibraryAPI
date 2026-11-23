using LibraryAPI.Models;

namespace LibraryAPI.Algorithms.Searching;

public static class LinearSearch
{
    public static Book? SearchByISBN(
        List<Book> books,
        string isbn)
    {
        foreach (var book in books)
        {
            if (book.ISBN == isbn)
                return book;
        }

        return null;
    }
}