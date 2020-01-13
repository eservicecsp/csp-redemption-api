using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IPromotionSubTypeService
    {
        Task<PromotionSubTypeResponseModel> GetPromotionSubTypeAsync();
    }
    public class PromotionSubTypeService : IPromotionSubTypeService
    {
        private readonly IPromotionSubTypeRepository promotionSubTypeRepository;

        public PromotionSubTypeService(IPromotionSubTypeRepository promotionSubTypeRepository)
        {
            this.promotionSubTypeRepository = promotionSubTypeRepository;
        }
        public async Task<PromotionSubTypeResponseModel> GetPromotionSubTypeAsync()
        {
            var response = new PromotionSubTypeResponseModel();
            try
            {
                var promotionSubTypes = await this.promotionSubTypeRepository.GetPromotionSubTypeAsync();
                List<PromotionSubTypeModel> promotionSubTypeModels = new List<PromotionSubTypeModel>();
                if (promotionSubTypes != null)
                {
                    foreach (var promotion in promotionSubTypes)
                    {
                        promotionSubTypeModels.Add(new PromotionSubTypeModel()
                        {
                            Id = promotion.Id,
                            Name = promotion.Name,
                            Description = promotion.Description
                        });
                    }
                }
                response.promotionSubTypes = promotionSubTypeModels;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
