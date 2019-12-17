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

        //Task<List<ProductAttachment>> GetProductAttachmentsAsync(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Product.Include(x => x.ProductAttachment).Include(x => x.CreatedByNavigation).Where(x => x.BrandId == brandId).ToListAsync();
            }
        }
        public async Task<Product> GetProductsByIdAsync(int id)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Product.Include(x => x.ProductAttachment).Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> CreateAsync(Product product)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await Context.Product.AddAsync(product);
                        await Context.SaveChangesAsync();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public async Task<bool> UpdateAsync(Product uProduct)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        Product dbProduct = await Context.Product.Include(x => x.ProductAttachment).SingleAsync(x => x.Id == uProduct.Id);
                        Context.Entry(dbProduct).CurrentValues.SetValues(uProduct);

                        foreach(var dbAttachmentProduct in dbProduct.ProductAttachment.ToList())
                        {
                            if (!uProduct.ProductAttachment.Any(x => x.Id == dbAttachmentProduct.Id))
                                Context.ProductAttachment.Remove(dbAttachmentProduct);
                        }

                        foreach(var uAttachmentProduct in uProduct.ProductAttachment)
                        {
                            Context.ProductAttachment.Attach(uAttachmentProduct);
                        }

                        await Context.SaveChangesAsync();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //public async Task<List<ProductAttachment>> GetProductAttachmentsAsync(Product product)
        //{
        //    using (var Context = new CSP_RedemptionContext())
        //    {
        //        return await Context.ProductAttachment.Where(x => x.ProductId == product.Id).ToListAsync();
        //    }
        //}
    }

}
