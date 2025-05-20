using DevPro.Application.Common;
using DevPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPro.Application
{
    public interface IProductService
    {
        Task<ApiResponse> CreateAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
