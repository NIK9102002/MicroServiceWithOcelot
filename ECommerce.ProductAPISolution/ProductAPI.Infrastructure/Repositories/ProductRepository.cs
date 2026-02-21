using eCommerce.SharedLibrary.Logging;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.Repositories
{
    internal class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                await context.Products.AddAsync(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Product created successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while creating the product.");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var product = await GetByIdAsync(id);
                if (product is null)
                {
                    return new Response(false, "Product not found.");
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, "Product deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while deleting the product.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the products.");                
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {           
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the product.");
            }            
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the product.");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {           
            try
            {
                var product = await GetByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, "Product not found.");
                }

                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while updating the product.");
            }            
        }
    }
}
