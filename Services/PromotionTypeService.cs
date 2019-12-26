using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IPromotionTypeService
    {
        Task<PromotionTypesResponseModel> GetPromotionTypesAsync();
    }

    public class PromotionTypeService: IPromotionTypeService
    {
        private readonly IPromotionTypeRepository promotionTypeRepository;
        public PromotionTypeService(IPromotionTypeRepository promotionTypeRepository)
        {
            this.promotionTypeRepository = promotionTypeRepository;
        }

        public async Task<PromotionTypesResponseModel> GetPromotionTypesAsync()
        {
            var response = new PromotionTypesResponseModel();
            try
            {
                var promotionTypes = await this.promotionTypeRepository.GetPromotionTypesAsync();
                response.PromotionTypes = new List<PromotionTypeModel>();
                foreach (var promotionType in promotionTypes)
                {
                    response.PromotionTypes.Add(new PromotionTypeModel()
                    {
                        Description = promotionType.Description,
                        Id = promotionType.Id,
                        Name = promotionType.Name
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
    }
}
