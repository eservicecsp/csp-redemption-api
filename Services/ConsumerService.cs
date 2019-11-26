using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IConsumerService
    {
        Task<ConsumersByPaginationResponseModel> GetConsumersByBrandIdAsync(PaginationModel data);
        Task<IsExistResponseModel> IsExist(CheckExistConsumerRequestModel checkExistConsumerRequestModel);
        Task<RedemptionResponseModel> Register(ConsumerRequestModel consumerRequest);
        Task<RedemptionResponseModel> Redemption(CheckExistConsumerRequestModel consumerRequest);
    }
    public class ConsumerService : IConsumerService
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly IConsumerRepository consumerRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IQrCodeRepository qrCodeRepository;

        public ConsumerService
            (
              ICampaignRepository campaignRepository,
              IConsumerRepository consumerRepository,
              ITransactionRepository transactionRepository,
              IQrCodeRepository qrCodeRepository
            )
        {
            this.campaignRepository = campaignRepository;
            this.consumerRepository = consumerRepository;
            this.transactionRepository = transactionRepository;
            this.qrCodeRepository = qrCodeRepository;
        }
        public async Task<ConsumersByPaginationResponseModel> GetConsumersByBrandIdAsync(PaginationModel data)
        {
            var response = new ConsumersByPaginationResponseModel();
            try
            {
                var consumers = await this.consumerRepository.GetConsumersByBrandIdAsync(data);
                if (consumers != null)
                {
                    response.length = await this.consumerRepository.GetConsumersTotalByBrandIdAsync(data);
                    response.data = consumers;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<IsExistResponseModel> IsExist(CheckExistConsumerRequestModel dataConsumer)
        {
            var response = new IsExistResponseModel();
            try
            {
                var campaign = await this.campaignRepository.GetCampaignByIdAsync(dataConsumer.CampaignId);
                if (campaign != null)
                {
                    var checkConsumer = new Consumer()
                    {
                        BrandId = campaign.BrandId,
                        Phone = dataConsumer.Phone
                    };
                    Consumer isExist = await this.consumerRepository.IsExist(checkConsumer);
                    if (isExist != null)
                    {
                        var tran = new CheckExistConsumerRequestModel();
                        tran.CampaignId = dataConsumer.CampaignId;
                        tran.ConsumerId = isExist.Id;
                        tran.Token = dataConsumer.Token;
                        tran.Point = dataConsumer.Point;
                        tran.Latitude = dataConsumer.Latitude;
                        tran.Longitude = dataConsumer.Longitude;
                        tran.Location = dataConsumer.Location;
                        //await this.Redemption(tran);
                        var redemption = await this.Redemption(tran);
                        response.IsSuccess = true;
                        response.StatusTypeCode = redemption.StatusTypeCode;
                        response.CampaignType = redemption.CampaignType;
                        response.Message = redemption.Message;
                        response.Pieces = redemption.Pieces;
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.IsExist = false;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.IsExist = false;
                    response.Message = "Campaign not found.";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<RedemptionResponseModel> Register(ConsumerRequestModel consumerRequest)
        {
            var response = new RedemptionResponseModel();
            var campaign = await this.campaignRepository.GetCampaignByIdAsync(consumerRequest.CampaignId);
            if (campaign == null)
            {
                response.Message = "Campaign not found.";
                response.StatusTypeCode = "FAIL";

            }
            else
            {
                var consumer = new Consumer()
                {
                    FirstName = consumerRequest.FirstName.Trim(),
                    LastName = consumerRequest.LastName.Trim(),
                    Email = consumerRequest.Email,
                    Phone = consumerRequest.Phone.Trim(),
                    BirthDate = consumerRequest.BirthDate,
                    Address1 = consumerRequest.Address1,
                    Address2 = consumerRequest.Address2,
                    TumbolCode = consumerRequest.TumbolCode,
                    AmphurCode = consumerRequest.AmphurCode,
                    ProvinceCode = consumerRequest.ProvinceCode,
                    ZipCode = consumerRequest.ZipCode,
                    ConsumerSourceId = 1,
                    BrandId = campaign.BrandId,
                    CampaignId = consumerRequest.CampaignId,
                    Point = 0,
                    CreatedDate = DateTime.Now
                };
                try
                {
                    var isCreated = await this.consumerRepository.CreateAsync(consumer);
                    if (isCreated)
                    {
                        Consumer isExist = await this.consumerRepository.IsExist(consumer);

                        var tran = new CheckExistConsumerRequestModel();
                        tran.CampaignId = consumerRequest.CampaignId;
                        tran.ConsumerId = isExist.Id;
                        tran.Token = consumerRequest.Token;
                        tran.Point = consumerRequest.Point == null ? 0 : Convert.ToInt32(consumerRequest.Point);
                        tran.Latitude = consumerRequest.Latitude;
                        tran.Longitude = consumerRequest.Longitude;
                        tran.Location = consumerRequest.Location;
                        var redemption = await this.Redemption(tran);
                        response.IsSuccess = true;
                        response.StatusTypeCode = redemption.StatusTypeCode;
                        response.CampaignType = redemption.CampaignType;
                        response.Message = redemption.Message; 
                        response.Pieces = redemption.Pieces;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Internal server error.";
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                }
            }
           
            
            return response;
        }

        public async Task<RedemptionResponseModel> Redemption(CheckExistConsumerRequestModel consumerRequest)
        {
            var response = new RedemptionResponseModel();
            var tran = new Transaction();
            var qrCode = new QrCode();
            bool IsError = false;
            int totalPoint = 0;
            try
            {
                var campaign = await this.campaignRepository.GetCampaignByIdAsync(consumerRequest.CampaignId);
                tran.CampaignId = campaign.Id;
                tran.ConsumerId = consumerRequest.ConsumerId;
                tran.Token = consumerRequest.Token;
                tran.Point = consumerRequest.Point;
                tran.Latitude = consumerRequest.Latitude;
                tran.Longitude = consumerRequest.Longitude;
                tran.Location = consumerRequest.Location;
                tran.CreatedDate = DateTime.Now;

                response.ConsumerId = consumerRequest.ConsumerId;
                response.CampaignType = campaign.CampaignTypeId;
                if ((campaign.StartDate <= DateTime.Now) && (campaign.EndDate >= DateTime.Now))
                {
                    qrCode.Token = consumerRequest.Token;
                    qrCode.CampaignId = consumerRequest.CampaignId;
                    qrCode.ConsumerId = consumerRequest.ConsumerId;
                    qrCode.ScanDate = DateTime.Now;

                    var dbQrCode = await this.qrCodeRepository.GetQrCode(qrCode);

                    if(dbQrCode != null)
                    {
                        if(dbQrCode.ConsumerId == null)
                        {
                            qrCode.Peice = dbQrCode.Peice;
                            qrCode.Point = dbQrCode.Point + consumerRequest.Point;

                            if (campaign.CampaignTypeId == 1) //JigSaw
                            {
                                response.Message = campaign.WinMessage;
                                response.StatusTypeCode = "SUCCESS";

                                tran.TransactionTypeId = 4;
                                tran.ResponseMessage = campaign.WinMessage;

                                var statusTran = await this.transactionRepository.CreateTransactionJigSawAsync(tran, qrCode);
                                if (!statusTran)
                                {
                                    IsError = true;
                                    response.Message = "System error.";
                                    response.StatusTypeCode = "FAIL";

                                    tran.TransactionTypeId = 1;
                                    tran.ResponseMessage = "System error.";
                                }
                                //else
                                //{
                                //    IsError = true;
                                //    response.Message = "System error.";
                                //    response.StatusTypeCode = "FAIL";

                                //    tran.TransactionTypeId = 1;
                                //    tran.ResponseMessage = "System error.";
                                //}
                               
                            } 
                            else if (campaign.CampaignTypeId == 2) //Point
                            {

  
                                var dbConsumer = await this.consumerRepository.GetConsumerByIdAsync(consumerRequest.ConsumerId);
                                tran.Point = dbQrCode.Point != null ? Convert.ToInt32(dbQrCode.Point) : 0;
                                totalPoint = (dbConsumer.Point != null ? Convert.ToInt32(dbConsumer.Point) : 0) + (dbQrCode.Point != null ? Convert.ToInt32(dbQrCode.Point) : 0);
                                qrCode.Point = totalPoint;

                                response.Message = campaign.WinMessage + " และมีคะแนนรวม ( " + totalPoint + " ) คะแนน";
                                response.StatusTypeCode = "SUCCESS";

                                tran.TransactionTypeId = 4;
                                tran.ResponseMessage = campaign.WinMessage + " และมีคะแนนรวม ( " + totalPoint + " ) คะแนน";

                                var statusTran = await this.transactionRepository.CreateTransactionPointAsync(tran, qrCode);
                                if (!statusTran)
                                {
                                    IsError = true;
                                    response.Message = "System error.";
                                    response.StatusTypeCode = "FAIL";

                                    tran.TransactionTypeId = 1;
                                    tran.ResponseMessage = "System error.";

                                }


                            }

                        }
                        else
                        {
                            IsError = true;
                            response.Message = campaign.DuplicateMessage;
                            response.StatusTypeCode = "DUPLICATE";

                            tran.TransactionTypeId = 3;
                            tran.ResponseMessage = campaign.DuplicateMessage;
                        }


                        if (campaign.CampaignTypeId == 1 && !IsError) //JigSaw
                        {
                            response.Pieces = await this.qrCodeRepository.GetPiece(qrCode);
                        }
                    }
                    else{
                        IsError = true;
                        response.Message = campaign.QrCodeNotExistMessage;
                        response.StatusTypeCode = "EMPTY";

                        tran.TransactionTypeId = 2;
                        tran.ResponseMessage = campaign.QrCodeNotExistMessage;
                    }
                }
                else
                {
                    IsError = true;
                    response.Message = campaign.AlertMessage;
                    response.StatusTypeCode = "FAIL";

                    tran.TransactionTypeId = 1;
                    tran.ResponseMessage = campaign.AlertMessage;
                }

                response.CampaignType = campaign.CampaignTypeId;
                if (IsError)
                {
                    await this.transactionRepository.CreateTransactionErrorAsync(tran);
                }
   
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
