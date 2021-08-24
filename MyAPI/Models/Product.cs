using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    //* EF Core  Enity Framework  => matching  with  Database
    //* From microsoft
    //* lock like  OOP
    public class Product
    {
        // *[Key] dataunotation primary key 
        // ! Set primary key  EF must  show  us ID  but we use product ID
        public int ProductID { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Sale { get; set; }

//* Reference from Category and Supplier
        public int CategoryID { get; set; } // Relation [Key] 

        public int SupplierID { get; set; } //* Relation [Key] SupplierID = class + ID 

// * Category => obj [Ef core] tell EF CategoryID from whcih class
        public Category ProductCategory { get; set; }

        public Supplier ProductSupplier { get; set; }
    }
}