using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IPromotionSubTypeRepository
    {
        Task<List<PromotionSubType>> GetPromotionSubTypeAsync();
    }
    public class PromotionSubTypeRepository : IPromotionSubTypeRepository
    {
        public async Task<List<PromotionSubType>> GetPromotionSubTypeAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.PromotionSubType.ToListAsync();
            }
        }
    }
}
