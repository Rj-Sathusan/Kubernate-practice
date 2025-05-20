using DevPro.Application;
using DevPro.Application.Common;
using DevPro.Database;
using DevPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPro.Services
{
    public class ProductService : IProductService
    {
        private readonly DevProLocalDbContext _dbContext;

        public ProductService(DevProLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> CreateAsync(Product product)
        {
            try
            {
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return new ApiResponse
                {
                    Status = true,
                    Message = "Product created successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }
    }
}
