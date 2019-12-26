using CSP_Redemption_WebApi.Entities.Models;
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
        Task<ResponseModel> CreateAsync(PromotionModel cPromotion);
        Task<ResponseModel> UpdateAsync(PromotionModel uPromotion);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPromotionTypeRepository _promotionTypeRepository;
        private readonly IBrandRepository _brandRepository;
        public PromotionService(IPromotionRepository promotionRepository, IPromotionTypeRepository promotionTypeRepository, IBrandRepository brandRepository)
        {
            _promotionRepository = promotionRepository;
            _promotionTypeRepository = promotionTypeRepository;
            _brandRepository = brandRepository;
        }

        public async Task<PromotionsResponseModel> GetPromotionsAsync(int brandId)
        {
            var response = new PromotionsResponseModel();
            try
            {
                var promotions = await _promotionRepository.GetPromotionsAsync(brandId);
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
                        PromotionType = new PromotionTypeModel()
                        {
                            Id = promotion.PromotionType.Id,
                            Name = promotion.PromotionType.Name,
                            Description = promotion.PromotionType.Description
                        }
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
                var promotion = await _promotionRepository.GetPromotionAsync(brandId, promotionId);
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
                        ModifiedBy = promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId
                    };
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> CreateAsync(PromotionModel cPromotion)
        {
            var response = new ResponseModel();
            try
            {
                var brand = await _brandRepository.GetBrandAsync(cPromotion.BrandId);
                if (brand == null)
                {
                    response.Message = "Brand not found.";
                }

                var promotionType = await _promotionTypeRepository.GetPromotionTypeAsync(cPromotion.PromotionTypeId);
                if (promotionType == null)
                {
                    response.Message = "PromotionType not found";
                }

                var promotion = new Promotion()
                {
                    Id = 0,
                    Name = cPromotion.Name,
                    Description = cPromotion.Description,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = cPromotion.CreatedBy,
                    IsActived = cPromotion.IsActived,
                    Brand = brand,
                    PromotionType = promotionType,
                };

                bool isSuccess = await _promotionRepository.CreateAsync(promotion);
                if (isSuccess)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAsync(PromotionModel uPromotion)
        {
            var response = new ResponseModel();
            try
            {
                var dbPromotion = await _promotionRepository.GetPromotionAsync(uPromotion.BrandId, uPromotion.Id);
                var dbPromotionType = await _promotionTypeRepository.GetPromotionTypeAsync(uPromotion.PromotionTypeId);

                if (dbPromotion != null)
                {
                    dbPromotion.Id = uPromotion.Id;
                    dbPromotion.IsActived = uPromotion.IsActived;
                    dbPromotion.ModifiedDate = DateTime.UtcNow;
                    dbPromotion.ModifiedBy = uPromotion.ModifiedBy;
                    dbPromotion.Name = uPromotion.Name;
                    dbPromotion.Description = uPromotion.Description;
                    dbPromotion.PromotionType = dbPromotionType;

                    bool isSuccess = await _promotionRepository.UpdateAsync(dbPromotion);
                    if (isSuccess)
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found.";
                }
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
