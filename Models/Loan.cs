namespace LibraryAPI.Models;

public class Loan
{
    public int Id { get; private set; }

    public int BookId { get; private set; }
    public Book Book { get; private set; } = null!;

    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    public DateTime LoanDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsActive => ReturnDate == null;

    private Loan() { }

    internal Loan(User user, Book book)
    {
        User = user;
        Book = book;

        LoanDate = DateTime.UtcNow;
        DueDate = LoanDate.AddDays(14);
    }

    public void Return()
    {
        if (!IsActive)
            throw new InvalidOperationException(
                "Esse empréstimo já foi devolvido."
            );

        ReturnDate = DateTime.UtcNow;

        Book.Return();
    }
}