using LibraryAPI.Models;

namespace LibraryAPI.Algorithms.Searching;

public static class BinarySearch
{
    public static Book? SearchByISBN(
        List<Book> books,
        string isbn)
    {
        int left = 0;
        int right = books.Count - 1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;

            int comparison = string.Compare(
                books[middle].ISBN,
                isbn,
                StringComparison.Ordinal
            );

            if (comparison == 0)
                return books[middle];

            if (comparison < 0)
                left = middle + 1;
            else
                right = middle - 1;
        }

        return null;
    }
}