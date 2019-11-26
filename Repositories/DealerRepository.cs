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
    }
}
