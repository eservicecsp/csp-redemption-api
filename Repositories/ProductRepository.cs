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
        Task<Product> GetProductsByIdAsync(int id);
        Task<bool> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
    }

    public class ProductRepository: IProductRepository
    {
        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Product.Include(x => x.CreatedByNavigation).Where(x => x.BrandId == brandId).ToListAsync();
            }
        }
        public async Task<Product> GetProductsByIdAsync(int id)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Product.Where(x => x.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> UpdateAsync(Product product)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Product thisRow = await Context.Product.SingleAsync(x => x.Id == product.Id);
                thisRow.Name = product.Name;
                thisRow.Description = product.Description;
                Context.Entry(thisRow).CurrentValues.SetValues(thisRow);
                return await Context.SaveChangesAsync() > 0;
            }
        }
    }

}
