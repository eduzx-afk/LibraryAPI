using LibraryAPI.Models;
using LibraryAPI.Enums;

namespace LibraryAPI.Services.Interfaces;

public interface AlgorithmService
{
    Task<Book?> LinearSearchByISBNAsync(string isbn);

    Task<Book?> BinarySearchByISBNAsync(string isbn);

    Task<Book?> QuickSelectAsync(int k, BookSortCriteria criteria);

    Task<List<Book>> SortBooksAsync(BookSortCriteria criteria);
}