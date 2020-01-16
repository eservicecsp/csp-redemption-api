using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IContactUsRepository
    {
        Task<ContactUs> GetContactUsByBrandIdAsync(int brandId);
        Task<bool> CreateAsync(ContactUs contactUs);
        Task<bool> UpdateAsync(ContactUs contactUs);
    }
    public class ContactUsRepository : IContactUsRepository
    {
        public async Task<ContactUs> GetContactUsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.ContactUs.Where(x => x.BrandId == brandId).FirstOrDefaultAsync();
            }
        }
        public async Task<bool> CreateAsync(ContactUs contactUs)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                await Context.ContactUs.AddAsync(contactUs);
                return await Context.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> UpdateAsync(ContactUs contactUs)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                ContactUs dbContactUs = await Context.ContactUs.FirstOrDefaultAsync(x => x.Id == contactUs.Id);
                Context.Entry(dbContactUs).CurrentValues.SetValues(contactUs);
                return await Context.SaveChangesAsync() > 0;
            }
        }
    }
}
