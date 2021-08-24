using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Models;

namespace MyAPI.Database
{
    public class DatabaseInit
    {
        //* Inject database
        public static void INIT(IServiceProvider ServiceProvider)
        {
            var context = new DatabaseContext(ServiceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>());

            // If database does not exist then the database and all its schema are created
            //* If no database this  line will created database from database context
            context.Database.EnsureCreated();

            InsertData(context); 
        }
        //*For code frist
        private static void InsertData(DatabaseContext context)
        {
            // Auto Insert datbase
            // If category table has data, it will return.
            if (context.Categories.Any())
            {
                return;
            }

            context.Categories.Add(new Category
            {
                Name = "IT",
                Description = "Mac Products"
            });
            context.SaveChanges();
            //* Add function  of EntityFrameworkCore for Add database
            context.Suppliers.Add(new Supplier
            {
                Name = "Arnoldo",
                Phone = "994-919-2393"
            });
            // Save database
            context.SaveChanges();

            context.Products.Add(new Product
            {
                Name = "MacBook",
                Price = 50000,
                CategoryID = 1,
                SupplierID = 1
            });
            // Save database
            context.SaveChanges();
        }
    }
}