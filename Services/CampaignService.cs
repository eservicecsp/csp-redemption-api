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
        Task<TransactionResponseModel> GetTransactionByCampaignsIdAsync(PaginationModel data);
        Task<ResponseModel> CreateCampaignAsync(CreateCampaignRequestModel requestModel);
        Task<CampaignsResponseModel> GetCampaignsByCampaignIdAsync(int campaignId);
        Task<ResponseModel> UpdateAsync(Campaign campaign);

    }

    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly ITransactionRepository transactionRepository;
        public CampaignService
            (
            ICampaignRepository campaignRepository,
            ITransactionRepository transactionRepository
            )
        {
            this.campaignRepository = campaignRepository;
            this.transactionRepository = transactionRepository;
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
                        Url = campaign.Url,
                        CampaignTypeId = campaign.CampaignTypeId,
                        CreatedBy = campaign.CreatedBy,
                        Id = campaign.Id,
                        Name = campaign.Name,
                        QrCodeNotExistMessage = campaign.QrCodeNotExistMessage,
                        Quantity = campaign.Quantity,
                        WinMessage = campaign.WinMessage,
                        GrandTotal = campaign.GrandTotal == null? 0: campaign.GrandTotal.Value
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

        public async Task<TransactionResponseModel> GetTransactionByCampaignsIdAsync(PaginationModel data)
        {
            var response = new TransactionResponseModel();
            try
            {
                var dbTran = new List<TransactionModel>();
                var transactions = await this.transactionRepository.GetTransactionByCampaignsIdAsync(data);
                if (transactions != null)
                {
                    foreach (var item in transactions)
                    {
                        dbTran.Add(new TransactionModel()
                        {
                            Id = item.Id,
                            ConsumerId = item.ConsumerId,
                            Token = item.Token,
                            Code = item.Code,
                            Point = item.Point,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Location = item.Location,
                            TransactionType = item.TransactionType.Name,
                            ResponseMessage = item.ResponseMessage,
                            CreatedDate = item.CreatedDate,
                            FirstName = item.Consumer == null ? null : item.Consumer.FirstName,
                            LastName = item.Consumer == null ? null : item.Consumer.LastName,
                            Email = item.Consumer == null ? null : item.Consumer.Email,
                            Phone = item.Consumer == null ? null : item.Consumer.Phone,
                            BirthDate = item.Consumer == null ? null : item.Consumer.BirthDate,
                            TotalPoint = item.Consumer == null ? 0 : (item.Consumer.Point != null) ? Convert.ToInt32(item.Consumer.Point) : 0
                        });
                    }
                    response.length = await this.transactionRepository.GetTransactionTotalByCampaignsIdAsync(data);
                    response.data = dbTran;
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
                 if(requestModel.Campaign.CampaignTypeId == 3)  //Enrollment & Member
                {
                    var qrToken = this.GenerateTokens(1);

                    int percentWaste = requestModel.Campaign.Waste == null ? 0 : requestModel.Campaign.Waste.Value;
                    double waste = ((double)requestModel.Campaign.Quantity * (double)percentWaste) / 100;
                    int Ceiling = (int)Math.Ceiling(waste);
                    int grandTotal = requestModel.Campaign.Quantity + Ceiling;

                    requestModel.Campaign.GrandTotal = grandTotal;
                    var qrCodes = this.GenerateCode(qrToken[0].Token, grandTotal);
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
                else if(requestModel.Campaign.CampaignTypeId == 1)//Collecting
                {
                    int[] n = new int[requestModel.Campaign.CollectingData.Count];
                    int grandTotal = 0;
                    for (int i = 0; i < requestModel.Campaign.CollectingData.Count; i++)
                    {
                        int percentWaste = requestModel.Campaign.Waste == null ? 0 : requestModel.Campaign.Waste.Value;
                        double waste = ((double)requestModel.Campaign.CollectingData[i].Quantity * (double)percentWaste) / 100;
                        int Ceiling = (int)Math.Ceiling(waste);
                        n[i] = (Ceiling + requestModel.Campaign.CollectingData[i].Quantity);
                        grandTotal += (Ceiling + requestModel.Campaign.CollectingData[i].Quantity);
                    }
                    //int Ceiling = (int)Math.Ceiling((requestModel.Campaign.Quantity * requestModel.Campaign.Waste) / 100);
                    requestModel.Campaign.GrandTotal = grandTotal;
                    var qrCodes = this.GenerateTokens(grandTotal);
                    requestModel.Peices = n.ToList();
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
                else //Point & Reward
                {
                    int percentWaste = requestModel.Campaign.Waste == null ? 0 : requestModel.Campaign.Waste.Value;
                    double waste = ((double)requestModel.Campaign.Quantity * (double)percentWaste) / 100;
                    int Ceiling = (int)Math.Ceiling(waste);
                    int grandTotal = requestModel.Campaign.Quantity + Ceiling;

                    requestModel.Campaign.GrandTotal = grandTotal;
                    var qrCodes = this.GenerateTokensPoint(grandTotal, requestModel.Point);
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
  
            }
            catch(Exception ex)
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
                        Token = $"{token}{Guid.NewGuid().ToString("N")}" ,
                        Code = null
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
            }
            return qrCodes;
        }
        public List<QrCode> GenerateTokensPoint(int quantity,int point)
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
                        Token = $"{token}{Guid.NewGuid().ToString("N")}",
                        Point = point
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
            }
            return qrCodes;
        }
        public List<QrCode> GenerateCode(string token ,int quantity)
        {
            List<QrCode> qrCodes = new List<QrCode>();
            try
            {
                int length = Convert.ToString(quantity).Length;
                int i = 1;
                while (i <= quantity)
                {

                    string value = String.Format("{0:D"+ length + "}", i);

                    qrCodes.Add(new QrCode()
                    {
                        Token = token,
                        Code = value
                    });
                    i++;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return qrCodes;
        }

        public async Task<CampaignsResponseModel> GetCampaignsByCampaignIdAsync(int campaignId)
        {
            var response = new CampaignsResponseModel();
            try
            {
                var campaignDb = await this.campaignRepository.GetCampaignByIdAsync(campaignId);
                if(campaignDb != null)
                {
                    int ProductId = 0;
                    if (campaignDb.CampaignProduct.Count() > 0)
                    {
                        ProductId = campaignDb.CampaignProduct.FirstOrDefault(x=>x.CampaignId == campaignId).ProductId;
                    }
                    var campaign = new CampaignModel()
                    {
                       Id = campaignDb.Id,
                       Name = campaignDb.Name,
                       Description = campaignDb.Description,
                       Product = ProductId,
                       StartDate = campaignDb.StartDate,
                       EndDate = campaignDb.EndDate,
                       AlertMessage = campaignDb.AlertMessage,
                       DuplicateMessage = campaignDb.DuplicateMessage,
                       QrCodeNotExistMessage = campaignDb.QrCodeNotExistMessage,
                       WinMessage = campaignDb.WinMessage,
                       BrandId = campaignDb.BrandId,
                       GrandTotal = campaignDb.GrandTotal == null ? 0 : campaignDb.GrandTotal.Value,
                       Waste = campaignDb.Waste == null ? 0 : campaignDb.Waste.Value

                    };
                    response.Campaign = campaign;
                    response.IsSuccess = true;
                }
            }catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseModel> UpdateAsync(Campaign campaign)
        {
            var response = new ResponseModel();
            try
            {
                response.IsSuccess = await this.campaignRepository.UpdateAsync(campaign);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
