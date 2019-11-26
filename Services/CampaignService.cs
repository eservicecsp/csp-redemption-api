using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface ICampaignService
    {
        Task<CampaignsResponseModel> GetCampaignsByBrandIdAsync(int brandId);
    }

    public class CampaignService: ICampaignService
    {
        private readonly ICampaignRepository campaignRepository;
        public CampaignService(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        public async Task<CampaignsResponseModel>  GetCampaignsByBrandIdAsync(int brandId)
        {
            var response = new CampaignsResponseModel();
            try
            {
                var campaigns = await this.campaignRepository.GetCampaignsByBrandIdAsync(brandId);
                response.Campaigns = new List<CampaignModel>();
                foreach(var campaign in campaigns)
                {
                    response.Campaigns.Add(new CampaignModel()
                    {
                        Description = campaign.Description,
                        DuplicateMessage = campaign.DuplicateMessage,
                        CreatedDate = campaign.CreatedDate  ,
                        EndDate = campaign.EndDate,
                        StartDate = campaign.StartDate,
                        AlertMessage = campaign.AlertMessage,
                        BrandId = campaign.BrandId,
                        CampaignTypeId = campaign.CampaignTypeId,
                        CreatedBy = campaign.CreatedBy,
                        Id = campaign.Id,
                        Name= campaign.Name,
                        QrCodeNotExistMessage = campaign.QrCodeNotExistMessage,
                        Quantity = campaign.Quantity,
                        WinMessage = campaign.WinMessage,
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
