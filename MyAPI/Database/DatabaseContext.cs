using Microsoft.EntityFrameworkCore;
using MyAPI.Models;

namespace MyAPI.Database
{
    public class DatabaseContext : DbContext
    {
        //Coductor for connect database DbContextOptions <Class>
        //* Injection Database
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #region tablename in database
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //* Primary Key API c is variable
            modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryID);

            // //*Name convention snake case
            // modelBuilder.Entity<Product>().ToTable("cm_product");
            // //* Mathing Name with  ColummName product_name
            // modelBuilder.Entity<Product>(p =>
            // {
            //   p.Property(u => u.Name).HasColumnName("product_name");
            // });
        }

    }
}