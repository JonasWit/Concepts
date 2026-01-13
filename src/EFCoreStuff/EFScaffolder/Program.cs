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

    var sql = context.Products
        .Where(p => p.Price > 10)
        .OrderBy(p => p.Name)
        .Take(5)
        .ToQueryString();

    Console.WriteLine("Generated SQL:");
    Console.WriteLine(sql);
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
