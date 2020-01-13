using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private readonly IConfiguration _configuration;
        private readonly IDealerRepository dealerRepository;
        private readonly ICollectionRepository collectionRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly IQrCodeRepository qrCodeRepository;
        public CampaignService
            (
            ICampaignRepository campaignRepository,
            ITransactionRepository transactionRepository,
            IConfiguration configuration ,
            IDealerRepository dealerRepository,
            ICollectionRepository collectionRepository,
            IEnrollmentRepository enrollmentRepository,
            IQrCodeRepository qrCodeRepository
            )
        {
            this.campaignRepository = campaignRepository;
            this.transactionRepository = transactionRepository;
            _configuration = configuration;
            this.dealerRepository = dealerRepository;
            this.collectionRepository = collectionRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.qrCodeRepository = qrCodeRepository;
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
                        GrandTotal = campaign.GrandTotal == null ? 0 : campaign.GrandTotal.Value
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
                        string FirstName = item.Consumer == null ? null : item.Consumer.FirstName;
                        string LastName = item.Consumer == null ? null : item.Consumer.LastName;
                        string Email = item.Consumer == null ? null : item.Consumer.Email;
                        string Phone = item.Consumer == null ? null : item.Consumer.Phone;
                        if (item.ConsumerId == null)
                        {
                            var qrCode = new QrCode()
                            {
                                CampaignId = item.CampaignId,
                                Token = item.Token,
                                Code = item.Code
                            };
                            var qrCodeDb = await this.qrCodeRepository.GetQCodeByCode(qrCode);
                            if (qrCodeDb != null && qrCodeDb.EnrollmentId != null)
                            {
                                var enrollment = await this.enrollmentRepository.GetEnrollmentByIdAsync(qrCodeDb.EnrollmentId.Value);
                                if (enrollment != null)
                                {
                                    FirstName = enrollment.FirstName;
                                    LastName = enrollment.LastName;
                                    Email = enrollment.Email;
                                    Phone = enrollment.Tel;
                                }

                            }

                        }
                        if(item.ConsumerId == null && item.EnrollmentId != null)
                        {
                            var enrollment = await this.enrollmentRepository.GetEnrollmentByIdAsync(item.EnrollmentId.Value);
                            if (enrollment != null)
                            {
                                FirstName = enrollment.FirstName;
                                LastName = enrollment.LastName;
                                Email = enrollment.Email;
                                Phone = enrollment.Tel;
                            }
                        }


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
                            FirstName = FirstName,
                            LastName = LastName,
                            Email = Email,
                            Phone = Phone,
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
                Campaign campaign = new Campaign()
                {
                    Name = requestModel.Campaign.Name,
                    Description = requestModel.Campaign.Description,
                    CampaignTypeId = requestModel.Campaign.CampaignTypeId,
                    BrandId = requestModel.Campaign.BrandId,
                    Url = requestModel.Campaign.Url,
                    Quantity = requestModel.Campaign.Quantity,
                    TotalPeice = requestModel.Campaign.TotalPeice,
                    WastePercentage = requestModel.Campaign.Waste,
                    StartDate = requestModel.Campaign.StartDate,
                    EndDate = requestModel.Campaign.EndDate,
                    AlertMessage = requestModel.Campaign.AlertMessage,
                    DuplicateMessage = requestModel.Campaign.DuplicateMessage,
                    QrCodeNotExistMessage = requestModel.Campaign.QrCodeNotExistMessage,
                    WinMessage = requestModel.Campaign.WinMessage,
                    CreatedBy = requestModel.Campaign.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                if (requestModel.Campaign.CampaignTypeId == 3)  //Enrollment & Member
                {
                   
                    var qrToken = this.GenerateTokens(1);

                    int percentWaste = requestModel.Campaign.Waste == null ? 0 : requestModel.Campaign.Waste.Value;
                    double waste = ((double)requestModel.Campaign.Quantity * (double)percentWaste) / 100;
                    int Ceiling = (int)Math.Ceiling(waste);
                    int grandTotal = requestModel.Campaign.Quantity + Ceiling;

                    requestModel.Campaign.GrandTotal = grandTotal;
                    campaign.GrandTotal = grandTotal;
                    var qrCodes = this.GenerateCode(qrToken[0].Token, grandTotal);


                    var hasSaved = await this.campaignRepository.CreateAsync(requestModel, qrCodes, campaign);
                    if (hasSaved.IsSuccess)
                    {
                        reponse.IsSuccess = true;
                    }
                    else
                    {
                        reponse.Message = hasSaved.Message;
                    }

                }
                else if (requestModel.Campaign.CampaignTypeId == 1)//Collecting
                {
                    int[] n = new int[requestModel.Campaign.CollectingData.Count];
                    int grandTotal = 0;

                    string campaginAttachmentPath = _configuration["Attachments:Campaigns"];
                    campaginAttachmentPath = campaginAttachmentPath.Replace("[#campaignId#]", $"{requestModel.Campaign.BrandId.ToString()}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}");
  
                    int i = 0;
                    List<CollectionModel> collections = new List<CollectionModel>();
                    foreach (var item in requestModel.Campaign.CollectingData)
                    {
                        int percentWaste = requestModel.Campaign.Waste == null ? 0 : requestModel.Campaign.Waste.Value;
                        double waste = ((double)item.Quantity * (double)percentWaste) / 100;
                        int Ceiling = (int)Math.Ceiling(waste);
                        n[i] = (Ceiling + item.Quantity);
                        grandTotal += (Ceiling + item.Quantity);
                        string[] Extensions = item.file.Split(',');
                        var filePath = Path.Combine(campaginAttachmentPath, item.name);
                        if(!Directory.Exists(campaginAttachmentPath))
                        {
                            Directory.CreateDirectory(campaginAttachmentPath);
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                        //File.WriteAllBytes(filePath, Convert.FromBase64String(item.file));

                        collections.Add(new CollectionModel()
                        {
                            Quantity = item.Quantity,
                            WasteQuantity = Ceiling,
                            TotalQuantity = (Ceiling + item.Quantity),
                            row = item.row,
                            column = item.column,
                            name = item.name,
                            path = filePath,
                            extension = Extensions[0]

                        });
                        i++;
                    }
                    campaign.CollectingType = Convert.ToInt32(requestModel.Campaign.CollectingType);
                    campaign.Rows = requestModel.Campaign.Rows;
                    campaign.Columns = requestModel.Campaign.Columns;


                    campaign.Quantity  = requestModel.Campaign.CollectingData.Sum(x=>x.Quantity);
                    requestModel.Campaign.CollectingData = collections;
                    //int Ceiling = (int)Math.Ceiling((requestModel.Campaign.Quantity * requestModel.Campaign.Waste) / 100);
                    requestModel.Campaign.GrandTotal = grandTotal;
                    campaign.GrandTotal = grandTotal;
                    campaign.TotalPeice = requestModel.Campaign.CollectingData.Count();
                    var qrCodes = this.GenerateTokens(grandTotal);
                    requestModel.Peices = n.ToList();



                    var hasSaved = await this.campaignRepository.CreateAsync(requestModel, qrCodes, campaign);
                    if (hasSaved.IsSuccess)
                    {
                        reponse.IsSuccess = true;
                    }
                    else
                    {
                        Directory.Delete(campaginAttachmentPath);
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
                    campaign.GrandTotal = grandTotal;
                    var qrCodes = this.GenerateTokensPoint(grandTotal, requestModel.Point);
                    var hasSaved = await this.campaignRepository.CreateAsync(requestModel, qrCodes, campaign);
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
                        Token = $"{token}{Guid.NewGuid().ToString("N")}",
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
        public List<QrCode> GenerateTokensPoint(int quantity, int point)
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
        public List<QrCode> GenerateCode(string token, int quantity)
        {
            List<QrCode> qrCodes = new List<QrCode>();
            try
            {
                int length = Convert.ToString(quantity).Length;
                int i = 1;
                while (i <= quantity)
                {

                    string value = Helpers.ShortenerHelper.GenerateToken(10);

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
                if (campaignDb != null)
                {
                    int ProductId = 0;
                    if (campaignDb.CampaignProduct.Count() > 0)
                    {
                        ProductId = campaignDb.CampaignProduct.FirstOrDefault(x => x.CampaignId == campaignId).ProductId;
                    }
                    List<Dealer> dealers = new List<Dealer>(); 
                    if (campaignDb.CampaignDealer.Count() > 0)
                    {
                        int[] DealerId = campaignDb.CampaignDealer.Select(x => x.DealerId).ToArray();
                        dealers = await this.dealerRepository.GetDealerByDealersIdAsync(DealerId);
                        //int dealdersId = new int { campaignDb.CampaignDealer.Select(x=>x.DealerId).ToArray() };
                    }
                    List<CollectionModel> collections = new List<CollectionModel>();
                    var collection = await this.collectionRepository.GetCollecttionsByCampaignIdAsync(campaignId);
                    if(collection!= null)
                    {
                       foreach(var item in collection)
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(item.CollectionPath);
                            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                            collections.Add(new CollectionModel()
                            {
                                Id = item.Id,
                                Quantity = item.Quantity,
                                TotalQuantity = item.TotalQuantity.Value,
                                row = item.CollectionRow,
                                column = item.CollectionColumn,
                                name = item.CollectionName,
                                //path = item.CollectionPath,
                                file = base64ImageRepresentation,
                                extension = item.Extension

                            });
                        }
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
                        Waste = campaignDb.WastePercentage == null ? 0 : campaignDb.WastePercentage.Value,
                        Dealers = dealers,
                        Rows = campaignDb.Rows,
                        Columns = campaignDb.Columns,
                        CollectingType = campaignDb.CollectingType.ToString(),
                        CollectingData = collections,
                        CampaignTypeId = campaignDb.CampaignTypeId

                    };
                    response.Campaign = campaign;
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
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
