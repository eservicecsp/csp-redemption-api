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
        Task<bool> CreateAsync(PromotionType promotionType);
        Task<bool> UpdateAsync(PromotionType promotionType);
    }
    public class PromotionTypeRepository: IPromotionTypeRepository
    {
        private readonly CSP_RedemptionContext _context;
        public PromotionTypeRepository(CSP_RedemptionContext context)
        {
            _context = context;
        }

        public async Task<List<PromotionType>> GetPromotionTypesAsync()
        {
            return await _context.PromotionType.ToListAsync();
        }

        public async Task<PromotionType> GetPromotionTypeAsync(int id)
        {
            return await _context.PromotionType.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreateAsync(PromotionType promotionType)
        {
            await _context.PromotionType.AddAsync(promotionType);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(PromotionType promotionType)
        {
            PromotionType dbPromotionType = await _context.PromotionType.SingleAsync(x => x.Id == promotionType.Id);
            _context.Entry(dbPromotionType).CurrentValues.SetValues(promotionType);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
