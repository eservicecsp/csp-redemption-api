using CSP_Redemption_WebApi.Entities.Models;
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
        Task<PromotionTypeResponseModel> GetPromotionTypeAsync(int id);
        Task<ResponseModel> CreateAsync(PromotionTypeModel cPromotionType);
        Task<ResponseModel> UpdateAsync(PromotionTypeModel uPromotionType);
    }

    public class PromotionTypeService: IPromotionTypeService
    {
        private readonly IPromotionTypeRepository _promotionTypeRepository;
        public PromotionTypeService(IPromotionTypeRepository promotionTypeRepository)
        {
            _promotionTypeRepository = promotionTypeRepository;
        }

        public async Task<PromotionTypesResponseModel> GetPromotionTypesAsync()
        {
            var response = new PromotionTypesResponseModel();
            try
            {
                var promotionTypes = await _promotionTypeRepository.GetPromotionTypesAsync();
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

        public async Task<PromotionTypeResponseModel> GetPromotionTypeAsync(int id)
        {
            var response = new PromotionTypeResponseModel();
            try
            {
                var promotionType = await _promotionTypeRepository.GetPromotionTypeAsync(id);
                response.PromotionType = new PromotionTypeModel()
                {
                    Id = promotionType.Id,
                    Description = promotionType.Description,
                    Name = promotionType.Name
                };
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> CreateAsync(PromotionTypeModel cPromotionType)
        {
            var response = new ResponseModel();
            try
            {

                var promotionType = new PromotionType()
                {
                    Id = 0,
                    Name = cPromotionType.Name,
                    Description = cPromotionType.Description,
                };

                bool isSuccess = await _promotionTypeRepository.CreateAsync(promotionType);
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

        public async Task<ResponseModel> UpdateAsync(PromotionTypeModel uPromotionType)
        {
            var response = new ResponseModel();
            try
            {
                var dbPromotionType = await _promotionTypeRepository.GetPromotionTypeAsync(uPromotionType.Id);

                if (dbPromotionType != null)
                {
                    dbPromotionType.Id = uPromotionType.Id;
                    dbPromotionType.Name = uPromotionType.Name;
                    dbPromotionType.Description = uPromotionType.Description;

                    bool isSuccess = await _promotionTypeRepository.UpdateAsync(dbPromotionType);
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
