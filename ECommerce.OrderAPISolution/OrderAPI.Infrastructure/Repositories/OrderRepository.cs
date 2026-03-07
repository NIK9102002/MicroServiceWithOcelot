using eCommerce.SharedLibrary.Logging;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Application.Interfaces;
using OrderAPI.Domain.Entities;
using OrderAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderAPI.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Order placed successfully.") : new Response(false, "Failed to place the order.");                
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while placing the order.");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var order = await GetByIdAsync(id);
                if (order is null)                
                    return new Response(false, "Order not found.");
                
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while deleting the order.");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the orders.");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.FirstOrDefaultAsync(predicate);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the order.");
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                return order is not null ? order : null!; 
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the order.");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("An error occurred while retrieving the orders.");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var existingOrder = await GetByIdAsync(entity.Id);
                if (existingOrder is null)
                    return new Response(false, "Order not found.");

                context.Entry(existingOrder).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while updating the order.");
            }
        }
    }
}
