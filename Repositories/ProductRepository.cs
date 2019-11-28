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
        Task<bool> CreateAsync(Product product);
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

        public async Task<bool> CreateAsync(Product product)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                await Context.Product.AddAsync(product);
                return await Context.SaveChangesAsync() > 0;
            }
        }
    }

}
