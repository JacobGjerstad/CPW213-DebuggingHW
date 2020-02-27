using AspnetCoreWithBugs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreWithBugs.Data
{
    public static class ProductDb
    {

        /// <summary>
        /// Creates a product object and adds it to the database.
        /// Returns the object with the Id populated
        /// </summary>
        public static async Task<Product> Create(Product p, ProductContext context)
        {
            await context.AddAsync(p);
            await context.SaveChangesAsync();
            return p;
        }

        /// <summary>
        /// Updates a product object from the DB and returns it.
        /// </summary>
        public static async Task<Product> Update(Product p, ProductContext context)
        {
            await context.AddAsync(p);
            context.Entry(p).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return p;
        }

        /// <summary>
        /// Deletes specified product
        /// </summary>
        public static async Task Delete(Product p, ProductContext context)
        {
            await context.AddAsync(p);
            context.Entry(p).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns single product item or null if there is no match
        /// </summary>
        /// <param name="id">Id of the clothing item</param>
        /// <param name="context">DB context</param>
        public async static Task<Product> GetProductById(int id, ProductContext context)
        {
            Product p = await (from product in context.Product
                               where product.ProductId == id
                               select product).SingleOrDefaultAsync();
            return p;
        }

        /// <summary>
        /// Returns the total number of Product items
        /// </summary>
        public static async Task<int> GetNumProduct(ProductContext context)
        {
            return await context.Product.CountAsync();
        }
    }
}
