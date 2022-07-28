using Microsoft.EntityFrameworkCore;

namespace CashRegister.Models
{
  public class CashRegisterContext : DbContext
  {
    public string DbConnection { get; }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<ProductSale> ProductSales { get; set; } = null!;

    public DbSet<Sale> Sales { get; set; } = null!;

    public CashRegisterContext(DbContextOptions<CashRegisterContext> options, IConfiguration config) : base(options)
    {
      DbConnection = Environment.GetEnvironmentVariable("MYSQL_CONNECTION")!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseMySQL(DbConnection);
    }
  }
}