using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IDealerRepository
    {
        Task<List<Dealer>> GetDealersByBrandIdAsync(int brandId);
        Task<Dealer> GetDealersByIdAsync(int id);
        Task<bool> UpdateAsync(Dealer dealer);
        Task<bool> CreateAsync(Dealer dealer);
    }
    public class DealerRepository : IDealerRepository
    {
        public async Task<List<Dealer>> GetDealersByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Dealer.Where(x => x.BrandId == brandId).ToListAsync();
            }
        }
        public async Task<Dealer> GetDealersByIdAsync(int id)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Dealer.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }
        public async Task<bool> UpdateAsync(Dealer dealer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Dealer thisRow = await Context.Dealer.SingleAsync(x => x.Id == dealer.Id);
                thisRow.BranchNo = dealer.BranchNo;
                thisRow.Name = dealer.Name;
                thisRow.Email = dealer.Email;
                thisRow.TaxNo = dealer.TaxNo;
                //thisRow.BrandId = thisRow.BrandId;
                thisRow.Phone = dealer.Phone;
                thisRow.Tel = dealer.Tel;
                thisRow.Address1 = dealer.Address1;
                thisRow.ProvinceCode = dealer.ProvinceCode;
                thisRow.AmphurCode = dealer.AmphurCode;
                thisRow.TumbolCode = dealer.TumbolCode;
                thisRow.ZipCode = dealer.ZipCode;
                Context.Entry(thisRow).CurrentValues.SetValues(thisRow);
                return await Context.SaveChangesAsync() > 0;

            }
        }

        public async Task<bool> CreateAsync(Dealer dealer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                await Context.Dealer.AddAsync(dealer);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        //public async Task<List<Dealer>> GetDealerByCampaignIdAsync(int campaignId)
        //{
        //    //List<Dealer> dealers = new
        //    using (var Context = new CSP_RedemptionContext())
        //    {
        //        return await Context.Dealer.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    }
        //}
    }
}
