namespace LibraryAPI.Models;

public class User
{
    private const int LoanLimit = 5;

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Email { get; private set; }

    public ICollection<Loan> Loans { get; private set; }
        = new List<Loan>();

    public int ActiveLoans =>
        Loans.Count(l => l.ReturnDate == null);

    public bool CanBorrow => ActiveLoans < LoanLimit;

    private User() { }

    public User(string name, string? email = null)
    {
        UpdateName(name);
        UpdateEmail(email);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome inválido.");

        Name = name;
    }

    public void UpdateEmail(string? email) => Email = email;

    public Loan BorrowBook(Book book)
    {
        if (!CanBorrow)
            throw new InvalidOperationException(
                "Limite de empréstimos atingido."
            );

        if (!book.IsAvailable)
            throw new InvalidOperationException(
                "O livro não está disponível."
            );

        book.Borrow();

        var loan = new Loan(this, book);

        Loans.Add(loan);

        return loan;
    }
}