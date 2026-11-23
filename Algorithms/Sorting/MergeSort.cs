using LibraryAPI.Models;
using LibraryAPI.Enums;

namespace LibraryAPI.Algorithms.Sorting;

public static class MergeSort
{
    public static List<Book> Sort(
        List<Book> books,
        BookSortCriteria criteria)
    {
        if (books.Count <= 1)
            return books;

        int middle = books.Count / 2;

        var left = books.Take(middle).ToList();
        var right = books.Skip(middle).ToList();

        left = Sort(left, criteria);
        right = Sort(right, criteria);

        return Merge(left, right, criteria);
    }

    private static List<Book> Merge(
        List<Book> left,
        List<Book> right,
        BookSortCriteria criteria)
    {
        var result = new List<Book>();

        int i = 0;
        int j = 0;

        while (i < left.Count && j < right.Count)
        {
            if (Compare(left[i], right[j], criteria) <= 0)
            {
                result.Add(left[i]);
                i++;
            }
            else
            {
                result.Add(right[j]);
                j++;
            }
        }

        while (i < left.Count)
        {
            result.Add(left[i]);
            i++;
        }

        while (j < right.Count)
        {
            result.Add(right[j]);
            j++;
        }

        return result;
    }

    private static int Compare(
        Book a,
        Book b,
        BookSortCriteria criteria)
    {
        return criteria switch
        {
            BookSortCriteria.Title =>
                string.Compare(a.Title, b.Title,
                    StringComparison.OrdinalIgnoreCase),

            BookSortCriteria.PublicationYear =>
                a.PublicationYear.CompareTo(b.PublicationYear),

            BookSortCriteria.ISBN =>
                string.Compare(a.ISBN, b.ISBN,
                    StringComparison.Ordinal),

            _ => throw new ArgumentOutOfRangeException(
                nameof(criteria),
                criteria,
                "Critério de ordenação inválido.")
        };
    }
}