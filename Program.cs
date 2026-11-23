using Microsoft.EntityFrameworkCore;

using LibraryAPI.Data;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<BookService, DefaultBookService>();
builder.Services.AddScoped<AuthorService, DefaultAuthorService>();
builder.Services.AddScoped<CategoryService, DefaultCategoryService>();
builder.Services.AddScoped<UserService, DefaultUserService>();
builder.Services.AddScoped<LoanService, DefaultLoanService>();
builder.Services.AddScoped<AlgorithmService, DefaultAlgorithmService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
