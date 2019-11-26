using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface ICampaignService
    {
        Task<CampaignsResponseModel> GetCampaignsByBrandIdAsync(int brandId);
        Task<ResponseModel> CreateCampaignAsync(CreateCampaignRequestModel requestModel);
    }

    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository campaignRepository;
        public CampaignService(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        public async Task<CampaignsResponseModel> GetCampaignsByBrandIdAsync(int brandId)
        {
            var response = new CampaignsResponseModel();
            try
            {
                var campaigns = await this.campaignRepository.GetCampaignsByBrandIdAsync(brandId);
                response.Campaigns = new List<CampaignModel>();
                foreach (var campaign in campaigns)
                {
                    response.Campaigns.Add(new CampaignModel()
                    {
                        Description = campaign.Description,
                        DuplicateMessage = campaign.DuplicateMessage,
                        CreatedDate = campaign.CreatedDate,
                        EndDate = campaign.EndDate,
                        StartDate = campaign.StartDate,
                        AlertMessage = campaign.AlertMessage,
                        BrandId = campaign.BrandId,
                        CampaignTypeId = campaign.CampaignTypeId,
                        CreatedBy = campaign.CreatedBy,
                        Id = campaign.Id,
                        Name = campaign.Name,
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

        public async Task<ResponseModel> CreateCampaignAsync(CreateCampaignRequestModel requestModel)
        {
            var reponse = new ResponseModel();

            try
            {
                var qrCodes = this.GenerateTokens(requestModel.Campaign.Quantity);
                var hasSaved = await this.campaignRepository.CreateAsync(requestModel, qrCodes);
                if (hasSaved.IsSuccess)
                {
                    reponse.IsSuccess = true;
                }
                else
                {
                    reponse.Message = hasSaved.Message;
                }
            }
            catch (Exception ex)
            {
                reponse.Message = ex.Message;
            }

            return reponse;
        }

        public List<QrCode> GenerateTokens(int quantity)
        {
            List<QrCode> qrCodes = new List<QrCode>();
            try
            {
                int i = 0;
                while (i < quantity)
                {
                    string token = Helpers.ShortenerHelper.GenerateToken(10);
                    qrCodes.Add(new QrCode()
                    {
                        Token = $"{token}{Guid.NewGuid().ToString("N")}"
                    });
                    i++;
                }

                // Check dupplicate token
                var dupplicateTokens = qrCodes.GroupBy(x => x.Token)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key);
                if (dupplicateTokens.Count() > 0)
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return qrCodes;
        }
    }
}
