﻿using CSP_Redemption_WebApi.Entities.DBContext;
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
    }

    public class PromotionRepository : IPromotionRepository
    {
        public async Task<List<Promotion>> GetPromotionsAsync(int brandId)
        {
            using (var context = new CSP_RedemptionContext())
            {
                return await context.Promotion
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.PromotionType)
                    .Where(x => x.BrandId == brandId).ToListAsync();
            }
        }
        public async Task<Promotion> GetPromotionAsync(int brandId, int promotionId)
        {
            using (var context = new CSP_RedemptionContext())
            {
                return await context.Promotion
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.PromotionType)
                    .FirstOrDefaultAsync(x => x.BrandId == brandId && x.Id == promotionId);
            }
        }
    }
}
