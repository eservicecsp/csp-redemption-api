using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetPromotionsAsync(int brandId);
        Task<Promotion> GetPromotionAsync(int brandId, int promotionId);
        Task<bool> CreateAsync(Promotion promotion);
        Task<bool> UpdateAsync(Promotion promotion);
        Task<List<Promotion>> GetPromotionsValidAsync(int brandId);
    }

    public class PromotionRepository : IPromotionRepository
    {
        private readonly CSP_RedemptionContext _context;
        public PromotionRepository(CSP_RedemptionContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetPromotionsAsync(int brandId)
        {
            return await _context.Promotion
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.PromotionType)
                    .Where(x => x.BrandId == brandId)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
        }

        public async Task<List<Promotion>> GetPromotionsValidAsync(int brandId)
        {
            DateTime currentDate = DateTime.Now;
            return await _context.Promotion
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.PromotionType)
                    .Where(x => x.BrandId == brandId)
                    //.Where(x=>x.StartDate.Value.Date >= currentDate.Date && currentDate.Date <= x.EndDate.Value.Date)
                    .Where(x => x.StartDate.Value.Date <= currentDate.Date && x.EndDate.Value.Date >= currentDate.Date)
                    .Where(x=>x.PromotionTypeId != 6)
                    .Where(x=>x.IsActived == true)
                    .OrderByDescending(x=>x.Id)
                    .ToListAsync();
        }
        public async Task<Promotion> GetPromotionAsync(int brandId, int promotionId)
        {
            return await _context.Promotion
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.PromotionType)
                    .FirstOrDefaultAsync(x => x.BrandId == brandId && x.Id == promotionId);
        }
        public async Task<bool> CreateAsync(Promotion promotion)
        {
            await _context.Promotion.AddAsync(promotion);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Promotion promotion)
        {
            Promotion dbPromotion = await _context.Promotion.Include(x => x.PromotionType).SingleAsync(x => x.Id == promotion.Id);
            _context.Entry(dbPromotion).CurrentValues.SetValues(promotion);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
