using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IPromotionService
    {
        Task<PromotionsResponseModel> GetPromotionsAsync(int brandId);
        Task<PromotionResponseModel> GetPromotionAsync(int brandId, int promotionId);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository promotionRepository;
        public PromotionService(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        public async Task<PromotionsResponseModel> GetPromotionsAsync(int brandId)
        {
            var response = new PromotionsResponseModel();
            try
            {
                var promotions = await this.promotionRepository.GetPromotionsAsync(brandId);
                response.Promotions = new List<PromotionModel>();
                foreach (var promotion in promotions)
                {
                    response.Promotions.Add(new PromotionModel()
                    {
                        Id = promotion.Id,
                        Description = promotion.Description,
                        CreatedDate = promotion.CreatedDate,
                        ModifiedDate = promotion.ModifiedDate,
                        BrandId = promotion.BrandId,
                        IsActived = promotion.IsActived,
                        CreatedBy = promotion.CreatedBy,
                        CreatedByName = $"{promotion.CreatedByNavigation.FirstName} {promotion.CreatedByNavigation.LastName}",
                        ModifiedBy = promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId,
                    });
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<PromotionResponseModel> GetPromotionAsync(int brandId, int promotionId)
        {
            var response = new PromotionResponseModel();
            try
            {
                var promotion = await this.promotionRepository.GetPromotionAsync(brandId, promotionId);
                if (promotion != null)
                {
                    response.Promotion = new PromotionModel()
                    {
                        Id = promotion.Id,
                        Description = promotion.Description,
                        CreatedDate = promotion.CreatedDate,
                        ModifiedDate = promotion.ModifiedDate,
                        BrandId = promotion.BrandId,
                        IsActived = promotion.IsActived,
                        CreatedBy = promotion.CreatedBy,
                        CreatedByName = $"{promotion.CreatedByNavigation.FirstName} {promotion.CreatedByNavigation.LastName}",
                        ModifiedBy= promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId
                    };
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
