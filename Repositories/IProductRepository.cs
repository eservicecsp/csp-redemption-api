using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByBrandIdAsync(int brandId);
    }

    public class ProductRepository: IProductRepository
    {
        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Product.Where(x => x.BrandId == brandId).ToListAsync();
            }
        }
    }
}
