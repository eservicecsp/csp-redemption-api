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

        public async Task<bool> CreateAsync(Dealer dealer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                await Context.Dealer.AddAsync(dealer);
                return await Context.SaveChangesAsync() > 0;
            }
        }
    }
}
