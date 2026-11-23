using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;
using LibraryAPI.Enums;
using LibraryAPI.Algorithms.Searching;
using LibraryAPI.Algorithms.Sorting;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.Data;

namespace LibraryAPI.Services;

public class DefaultAlgorithmService(LibraryDbContext context) : AlgorithmService
{
    private readonly LibraryDbContext _context = context;

    public async Task<Book?> LinearSearchByISBNAsync(string isbn)
    {
        var books = await GetBooks();

        return LinearSearch.SearchByISBN(books, isbn);
    }

    public async Task<Book?> BinarySearchByISBNAsync(string isbn)
    {
        var books = await SortBooksAsync(BookSortCriteria.ISBN);

        return BinarySearch.SearchByISBN(books, isbn);
    }

    public async Task<List<Book>> SortBooksAsync(
        BookSortCriteria criteria)
    {
        var books = await GetBooks();

        return MergeSort.Sort(books, criteria);
    }

    private async Task<List<Book>> GetBooks()
        => await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
}