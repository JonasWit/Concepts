using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFScaffolder;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("EF Core Scaffolder and SQL Query Viewer");
    Console.WriteLine("========================================\n");

    Console.WriteLine("SCAFFOLD COMMAND FOR MSSQL SERVER 16 (Windows Authentication):");
    Console.WriteLine("---------------------------------------------------------------");
    Console.WriteLine("dotnet ef dbcontext scaffold \"Server=localhost;Database=YourDatabase;Integrated Security=True;TrustServerCertificate=True;\" Microsoft.EntityFrameworkCore.SqlServer -o Models --force\n");

    var connectionString = "Server=localhost;Database=YourDatabase;Integrated Security=True;TrustServerCertificate=True;";

    using var context = new AppDbContext(connectionString);

    Console.WriteLine("\n=== QUERY SQL (using ToQueryString) ===");
    var sql = context.Products
        .Where(p => p.Price > 10)
        .OrderBy(p => p.Name)
        .Take(5)
        .ToQueryString();
    Console.WriteLine(sql);

    Console.WriteLine("\n=== SAVECHANGES SQL (logged but rolled back) ===");

    using var transaction = context.Database.BeginTransaction();
    try
    {


    }
    finally
    {
      transaction.Rollback();
    }


    // Wrap in transaction and rollback to see SQL without committing
    using var transaction = context.Database.BeginTransaction();

    // INSERT
    var newProduct = new Product { Name = "New Product", Price = 25.99m };
    context.Products.Add(newProduct);
    context.SaveChanges(); // SQL will be printed

    // UPDATE
    var product = context.Products.First();
    product.Price = 99.99m;
    context.SaveChanges(); // SQL will be printed

    // DELETE
    var productToDelete = context.Products.Skip(1).First();
    context.Products.Remove(productToDelete);
    context.SaveChanges(); // SQL will be printed

    transaction.Rollback(); // Nothing is committed to database
    Console.WriteLine("\n--- Transaction rolled back, no changes committed ---");
  }
}

public class AppDbContext(string connectionString) : DbContext
{
  private readonly string _connectionString = connectionString;

  public DbSet<Product> Products { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
        .UseSqlServer(_connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging();
  }
}

public class Product
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }
}
