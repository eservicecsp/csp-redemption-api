using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface ICampaignTypeService
    {
        Task<CampaignTypesResponseModel> GetCampaignTypesAsync();
    }

    public class CampaignTypeService : ICampaignTypeService
    {
        private readonly ICampaignTypeRepository campaignTypeRepository;
        public CampaignTypeService(ICampaignTypeRepository campaignTypeRepository)
        {
            this.campaignTypeRepository = campaignTypeRepository;
        }

        public async Task<CampaignTypesResponseModel> GetCampaignTypesAsync()
        {
            var response = new CampaignTypesResponseModel();
            try
            {
                var campaignTypes = await this.campaignTypeRepository.GetCampaignTypesAsync();
                response.CampaignTypes = new List<CampaignTypeModel>();

                foreach (var campaignType in campaignTypes)
                {
                    response.CampaignTypes.Add(new CampaignTypeModel()
                    {
                        Id = campaignType.Id,
                        Description = campaignType.Description,
                        Name = campaignType.Name,
                        SubTitle = campaignType.SubTitle,
                        Title = campaignType.Title,
                        ImagePath = campaignType.ImagePath,
                    });
                }

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
