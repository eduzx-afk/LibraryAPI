using LibraryAPI.Models;
using LibraryAPI.Enums;

namespace LibraryAPI.Algorithms.Selection;

public static class QuickSelect
{
    public static Book Select(
        List<Book> books,
        int k,
        BookSortCriteria criteria)
    {
        if (books == null || books.Count == 0)
            throw new ArgumentException("A lista de livros está vazia.");

        if (k < 0 || k >= books.Count)
            throw new ArgumentOutOfRangeException(
                nameof(k),
                "A posição solicitada é inválida."
            );

        int left = 0;
        int right = books.Count - 1;

        while (left <= right)
        {
            // Escolhe um pivô aleatório.
            int pivotIndex = Random.Shared.Next(left, right + 1);

            // Particiona a lista em três regiões.
            var (lt, gt) = Partition(
                books,
                left,
                right,
                pivotIndex,
                criteria
            );

            if (k < lt)
            {
                right = lt - 1;
            }
            else if (k > gt)
            {
                left = gt + 1;
            }
            else
            {
                return books[k];
            }
        }

        throw new InvalidOperationException(
            "Não foi possível encontrar o elemento."
        );
    }

    private static (int lt, int gt) Partition(
        List<Book> books,
        int left,
        int right,
        int pivotIndex,
        BookSortCriteria criteria)
    {
        Book pivot = books[pivotIndex];

        int lt = left;
        int i = left;
        int gt = right;

        while (i <= gt)
        {
            int comparison = Compare(books[i], pivot, criteria);

            if (comparison < 0)
            {
                Swap(books, lt, i);
                lt++;
                i++;
            }
            else if (comparison > 0)
            {
                Swap(books, i, gt);
                gt--;
            }
            else
            {
                i++;
            }
        }

        return (lt, gt);
    }

    private static int Compare(
        Book a,
        Book b,
        BookSortCriteria criteria)
    {
        return criteria switch
        {
            BookSortCriteria.Title =>
                string.Compare(a.Title, b.Title),

            BookSortCriteria.PublicationYear =>
                a.PublicationYear.CompareTo(b.PublicationYear),

            BookSortCriteria.ISBN =>
                string.Compare(a.ISBN, b.ISBN),

            _ => throw new ArgumentException(
                "Critério de ordenação inválido."
            )
        };
    }

    private static void Swap(List<Book> books, int i, int j)
        => (books[i], books[j]) = (books[j], books[i]);
}
