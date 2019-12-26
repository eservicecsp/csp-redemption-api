using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IProductTypeRepository
    {
        Task<List<ProductType>> GetProductTypesByBrandIdAsync(int brandId);
        Task<bool> CreateAsync(ProductType productType);
        Task<bool> UpdateAsync(ProductType productType);
    }
    public class ProductTypeRepository : IProductTypeRepository
    {
        public async Task<List<ProductType>> GetProductTypesByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.ProductType.Include(x => x.CreatedByNavigation).Where(x => x.BrandId == brandId).ToListAsync();
            }
        }

        public async Task<bool> CreateAsync(ProductType productType)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                await Context.ProductType.AddAsync(productType);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateAsync(ProductType productType)
        {
            using (var Context = new CSP_RedemptionContext())
            {

                ProductType thisRow = await Context.ProductType.SingleAsync(x => x.Id == productType.Id);
                thisRow.Name = productType.Name;
                thisRow.Description = productType.Description;
                thisRow.IsActived = productType.IsActived;
                thisRow.ModifiedBy = productType.ModifiedBy;
                thisRow.ModifiedDate = productType.ModifiedDate;
                Context.Entry(thisRow).CurrentValues.SetValues(thisRow);
                return await Context.SaveChangesAsync() > 0;
            }
        }
    }
}
