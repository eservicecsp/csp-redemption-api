using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IPromotionTypeRepository
    {
        Task<List<PromotionType>> GetPromotionTypesAsync();
        Task<PromotionType> GetPromotionTypeAsync(int id);
    }
    public class PromotionTypeRepository: IPromotionTypeRepository
    {
        public async Task<List<PromotionType>> GetPromotionTypesAsync()
        {
            using(var context = new CSP_RedemptionContext())
            {
                return await context.PromotionType.ToListAsync();
            }
        }

        public async Task<PromotionType> GetPromotionTypeAsync(int id)
        {
            using (var context = new CSP_RedemptionContext())
            {
                return await context.PromotionType.FirstOrDefaultAsync(x=>x.Id == id);
            }
        }
    }
}
