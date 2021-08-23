using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAPI.Database;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        // Readonly from parameter C# extensions
        private DatabaseContext DatabaseContext { get; }

        public ILogger<ProductController> Logger { get; }

        //* injection database  Context to clase need use database
        //* ILogger<ProductController = > Class> Logger
        public ProductController(ILogger<ProductController> logger, DatabaseContext databaseContext)
        {
            this.Logger = logger;

            this.DatabaseContext = databaseContext;
        }
        //*  /api/[controller]/
        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                //* DatabaseContext. name_table
                IEnumerable<Product> result = DatabaseContext.Products.ToList();
                Logger.LogInformation($"result count : {result.Count()}");
                //* Logger under LogInformation dont save in Logs file

                Logger.LogDebug($"result count : {result.Count()}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError($"GetProducts {ex.Message}");
                return BadRequest();
            }
        }
        //*  /api/[controller]/id
        [HttpGet("{id}", Name = "GetProductByID")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                //* querry product from id
                Product result = DatabaseContext.Products.SingleOrDefault(p => p.ProductID == id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                Logger.LogError($"GetProduct {ex.Message}");
                return BadRequest();
            }

        }
        //*  /api/[controller]/join
        [HttpGet("join")]
        public IActionResult GetProductsJoin()
        {
            try
            {
                IEnumerable<Product> result = DatabaseContext.Products.Include(c => c.ProductCategory)
                                                .Include(s => s.ProductSupplier).ToList();

                return Ok(result);


            }
            catch (Exception ex)
            {
                Logger.LogError($"GetProductsJoin {ex.Message}");
                return BadRequest();
            }

        }


        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product model)
        {
            try
            {
                DatabaseContext.Products.Add(model);
                DatabaseContext.SaveChanges();
                return CreatedAtRoute("GetProductByID", new { id = model.ProductID, model });


            }
            catch (Exception ex)
            {
                Logger.LogError($"CreateProduct : {ex.Message}");
                return BadRequest();
            }

        }

        //* update content  
        [HttpPut("{id}")]
        public IActionResult UpdateProduct([FromBody] Product model, int id)
        {
            try
            {
                Product result = DatabaseContext.Products.Find(id);

                if (result == null)
                {
                    return NotFound();
                }

                result.Name = model.Name;
                result.Price = model.Price;
                result.CategoryID = model.CategoryID;
                result.SupplierID = model.SupplierID;

                DatabaseContext.Products.Update(result);
                DatabaseContext.SaveChanges();

                return NoContent();


            }
            catch (Exception ex)
            {
                Logger.LogError($"UpdateProduct {ex.Message}");
                return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                Product result = DatabaseContext.Products.Find(id);

                if (result == null)
                {
                    return NotFound();
                }
                DatabaseContext.Products.Remove(result);
                DatabaseContext.SaveChanges();

                return NoContent();


            }
            catch (Exception ex)
            {
                Logger.LogError($"DeleteProduct : {ex.Message}");
                return BadRequest();
            }

        }






        // [HttpPost]
        // public IActionResult Post([FromBody] modelType model)
        // {
        //     try
        //     {
        //         return Created("", null);
        //     }
        //     catch (Exception)
        //     {
        //         Logger.LogError("Failed to execute POST");
        //         return BadRequest();
        //     }
        // }

        // [HttpPut]
        // public IActionResult Put([FromBody] modelType model)
        // {
        //     try
        //     {
        //         return Ok();
        //     }
        //     catch (Exception)
        //     {
        //         Logger.LogError("Failed to execute PUT");
        //         return BadRequest();
        //     }
        // }

        // [HttpDelete]
        // public IActionResult Delete(inputType id)
        // {
        //     try
        //     {
        //         return Ok();
        //     }
        //     catch (Exception)
        //     {
        //         Logger.LogError("Failed to execute DELETE");
        //         return BadRequest();
        //     }
        // }
    }
}