using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IConsumerService
    {
        Task<ConsumersByPaginationResponseModel> GetConsumersByBrandIdAsync(PaginationModel data);
        Task<IsExistResponseModel> IsExist(CheckExistConsumerRequestModel checkExistConsumerRequestModel);
        Task<RedemptionResponseModel> Register(ConsumerRequestModel consumerRequest);
        Task<RedemptionResponseModel> Redemption(CheckExistConsumerRequestModel consumerRequest);
        Task<ResponseModel> ImportJob(ImportDataBinding data);
        Task<FileResponseDataBinding> ExportTextFileConsumerByBrandId(FiltersModel data, int brandId);
        Task<ResponseModel> SendSelected(List<Consumer> enrollments, string channel, int brandId);
        Task<ResponseModel> SendAll(PaginationModel data, string channel, int brandId);
    }
    public class ConsumerService : IConsumerService
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly IConsumerRepository consumerRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IQrCodeRepository qrCodeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        public ConsumerService
            (
              ICampaignRepository campaignRepository,
              IConsumerRepository consumerRepository,
              ITransactionRepository transactionRepository,
              IQrCodeRepository qrCodeRepository,
              IHostingEnvironment hostingEnvironment,
              IConfiguration configuration
            )
        {
            this.campaignRepository = campaignRepository;
            this.consumerRepository = consumerRepository;
            this.transactionRepository = transactionRepository;
            this.qrCodeRepository = qrCodeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        public async Task<ConsumersByPaginationResponseModel> GetConsumersByBrandIdAsync(PaginationModel data)
        {
            var response = new ConsumersByPaginationResponseModel();
            try
            {
                var consumers = await this.consumerRepository.GetConsumersByBrandIdAsync(data, "WEB");
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
                    if(campaign.CampaignTypeId == 3)  //Enrollment & Member
                    {
                        if ((isExist != null && isExist.BirthDate == null) || isExist == null)
                        {
                            ConsumerRequestModel dbConsumer = new ConsumerRequestModel();
                            if(isExist != null)
                            {

                                dbConsumer.Id = isExist.Id;
                                dbConsumer.FirstName = isExist.FirstName;
                                dbConsumer.LastName = isExist.LastName;
                                dbConsumer.Email = isExist.Email;
                                dbConsumer.Phone = isExist.Phone;
                            }

                            response.IsSuccess = true;
                            response.IsExist = false;
                            response.consumer = dbConsumer;
                        }
                        else
                        {
                            var tran = new CheckExistConsumerRequestModel();
                            tran.CampaignId = dataConsumer.CampaignId;
                            tran.ConsumerId = isExist.Id;
                            tran.Token = dataConsumer.Token;
                            tran.Code = dataConsumer.Code;
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

                            response.IsSuccess = true;
                            response.IsExist = true;
                        }

                            
                    }
                    else
                    {
                        if (isExist != null)
                        {
                            var tran = new CheckExistConsumerRequestModel();
                            tran.CampaignId = dataConsumer.CampaignId;
                            tran.ConsumerId = isExist.Id;
                            tran.Token = dataConsumer.Token;
                            tran.Code = dataConsumer.Code;
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
                            response.TotalPieces = redemption.TotalPieces;

                            response.IsSuccess = true;
                            response.IsExist = true;
                        }
                        else
                        {
                            response.IsSuccess = true;
                            response.IsExist = false;
                        }
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
                    CreatedDate = DateTime.Now,
                };
                try
                {   if(consumerRequest.Id > 0)
                    {
                        consumer.Id = consumerRequest.Id;
                        var isCreated = await this.consumerRepository.UpdateAsync(consumer);
                        if (isCreated)
                        {
                            Consumer isExist = await this.consumerRepository.IsExist(consumer);

                            var tran = new CheckExistConsumerRequestModel();
                            tran.CampaignId = consumerRequest.CampaignId;
                            tran.ConsumerId = isExist.Id;
                            tran.Token = consumerRequest.Token;
                            tran.Code = consumerRequest.Code;
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
                            response.TotalPieces = redemption.TotalPieces;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Internal server error.";
                        }
                        
                    }
                    else
                    {
                        consumer.IsSkincare = consumerRequest.IsSkincare;
                        consumer.IsMakeup = consumerRequest.IsMakeup;
                        consumer.IsBodycare = consumerRequest.IsBodycare;
                        consumer.IsSupplements = consumerRequest.IsSupplements;
                        consumer.CreatedBy = 0;
                        var isCreated = await this.consumerRepository.CreateAsync(consumer);
                        if (isCreated)
                        {
                            Consumer isExist = await this.consumerRepository.IsExist(consumer);

                            var tran = new CheckExistConsumerRequestModel();
                            tran.CampaignId = consumerRequest.CampaignId;
                            tran.ConsumerId = isExist.Id;
                            tran.Token = consumerRequest.Token;
                            tran.Code = consumerRequest.Code;
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
                            response.TotalPieces = redemption.TotalPieces;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Internal server error.";
                        }
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
                tran.Code = consumerRequest.Code;
                tran.Point = consumerRequest.Point;
                tran.Latitude = consumerRequest.Latitude;
                tran.Longitude = consumerRequest.Longitude;
                tran.Location = consumerRequest.Location;
                tran.CreatedDate = DateTime.Now;

                response.ConsumerId = consumerRequest.ConsumerId;
                response.CampaignType = campaign.CampaignTypeId;
                response.TotalPieces = campaign.TotalPeice;
                if ((campaign.StartDate.Value.Date <= DateTime.Now.Date) && (campaign.EndDate.Value.Date >= DateTime.Now.Date))
                {
                    qrCode.Token = consumerRequest.Token;
                    qrCode.CampaignId = consumerRequest.CampaignId;
                    qrCode.ConsumerId = consumerRequest.ConsumerId;
                    qrCode.ScanDate = DateTime.Now;
                    qrCode.Code = null;

                    if (campaign.CampaignTypeId == 3) //Enrollment & Member
                    {
                        
                        qrCode.Code = consumerRequest.Code;
                        var dbQrCode = await this.qrCodeRepository.GetQCodeByCode(qrCode);
                        if(dbQrCode != null)
                        {
                            qrCode.Id = dbQrCode.Id;
                            qrCode.Peice = dbQrCode.Peice;
                            if (dbQrCode.ConsumerId == null)
                            {
                                response.Message = campaign.WinMessage;
                                response.StatusTypeCode = "SUCCESS";

                                tran.TransactionTypeId = 4;
                                tran.ResponseMessage = campaign.WinMessage;

                                var statusTran = await this.transactionRepository.CreateTransactionEnrollmentAsync(tran, qrCode);
                                if (!statusTran)
                                {
                                    IsError = true;
                                    response.Message = "System error.";
                                    response.StatusTypeCode = "FAIL";

                                    tran.TransactionTypeId = 1;
                                    tran.ResponseMessage = "System error.";
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
                        }
                        else
                        {
                            IsError = true;
                            response.Message = campaign.QrCodeNotExistMessage;
                            response.StatusTypeCode = "EMPTY";

                            tran.TransactionTypeId = 2;
                            tran.ResponseMessage = campaign.QrCodeNotExistMessage;
                        }
                        
                    }
                    else
                    {
                        var dbQrCode = await this.qrCodeRepository.GetQrCode(qrCode);

                        if (dbQrCode != null)
                        {
                            if (dbQrCode.ConsumerId == null)
                            {
                                qrCode.Id = dbQrCode.Id;
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
                                }
                                else if (campaign.CampaignTypeId == 2) //Point
                                {


                                    var dbConsumer = await this.consumerRepository.GetConsumerByIdAsync(consumerRequest.ConsumerId);
                                    tran.Point = dbQrCode.Point != null ? Convert.ToInt32(dbQrCode.Point) : 0;
                                    totalPoint = (dbConsumer.Point != null ? Convert.ToInt32(dbConsumer.Point) : 0) + (dbQrCode.Point != null ? Convert.ToInt32(dbQrCode.Point) : 0);
                                    // qrCode.Point = dbQrCode.Point;

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


                            if (campaign.CampaignTypeId == 1) //JigSaw
                            {
                                response.Pieces = await this.qrCodeRepository.GetPiece(qrCode);
                            }
                        }
                        else
                        {
                            IsError = true;
                            response.Message = campaign.QrCodeNotExistMessage;
                            response.StatusTypeCode = "EMPTY";

                            tran.TransactionTypeId = 2;
                            tran.ResponseMessage = campaign.QrCodeNotExistMessage;
                        }
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

        public async Task<ResponseModel> ImportJob(ImportDataBinding data)
        {
            var response = new ResponseModel();
            DateTime now = DateTime.Now;
            string contentRoot = hostingEnvironment.ContentRootPath;
            string webRoot = hostingEnvironment.ContentRootPath;
            string AttachfilePath = string.Empty;
            string subDomain = this.configuration["SubDomain"];

            // temp directory in root directory
            var filePath = Path.Combine(AttachfilePath, data.fileName);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                if (!string.IsNullOrEmpty(subDomain))
                {
                    AttachfilePath = Path.Combine(webRoot, subDomain, "Temp");
                }
                else
                {
                    AttachfilePath = Path.Combine(webRoot, "Temp");
                }
                if (!Directory.Exists(AttachfilePath)) Directory.CreateDirectory(AttachfilePath);

                File.WriteAllBytes(filePath, Convert.FromBase64String(data.file.Split(',').Last()));

                string[] lines = System.IO.File.ReadAllLines(filePath, Encoding.GetEncoding(874));
                int countLine = 0;
                var consumers = new List<Consumer>();
                if (lines.Count() <= 1)
                {
                    response.IsSuccess = false;
                    response.Message = "Text file invalid format or format not supported.";
                    return response;
                }

                try
                {
                    foreach (string line in lines)
                    {
                        if ((countLine != 0) && (line.Split('|')[0] != ""))
                        {
                            consumers.Add(new Consumer()
                            {
                                FirstName = line.Split('|')[0],
                                LastName = line.Split('|')[1],
                                Phone = line.Split('|')[2],
                                Email = line.Split('|')[3],
                                BrandId = data.brandId,
                                ConsumerSourceId = 2,
                                CreatedDate = now
                            });
                        }
                        countLine++;
                    }
                }
                catch(Exception ex)
                {
                    if (File.Exists(filePath)) File.Delete(filePath);

                    response.IsSuccess = false;
                    response.Message = "Text file invalid format or format not supported.";
                    return response;
                }

                if (File.Exists(filePath)) File.Delete(filePath);

                if (consumers.Count > 0)
                {
                    response.IsSuccess = await this.consumerRepository.ImportFileAsync(consumers);
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            return response;
        }

        public async Task<FileResponseDataBinding> ExportTextFileConsumerByBrandId(FiltersModel data , int brandId)
        {
            FileResponseDataBinding result = new FileResponseDataBinding();
            try
            {
                var consumersDb = await this.consumerRepository.ExportTextFileConsumerByBrandIdAsync(data, brandId);
                if (consumersDb.Count() > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd")+"_"+ brandId+"_"+"Consumers.txt";
                    string filePath = Path.Combine(@"Upload\" + fileName);
                    if (!File.Exists(filePath))
                    {
                        using (StreamWriter writer = File.CreateText(filePath))
                        //using (StreamWriter writer = new StreamWriter(pathFile, true))
                        {
                            writer.WriteLine("FirstName|LastName|Email|Phone|BirthDate|Address|Tumbol|Amphur|Province|ZipCode");

                            foreach (var item in consumersDb)
                            {
                                string Tumbol = item.TumbolCodeNavigation == null ? "" : item.TumbolCodeNavigation.NameTh;
                                string Amphur = item.AmphurCodeNavigation == null ? "" : item.AmphurCodeNavigation.NameTh;
                                string Province = item.ProvinceCodeNavigation == null ? "" : item.ProvinceCodeNavigation.NameTh;
                                writer.WriteLine(item.FirstName + "|" + 
                                    item.LastName + "|" + 
                                    item.Email + "|" + 
                                    item.Phone + "|" + 
                                    item.BirthDate.Value.ToString("yyyy-MM-dd")+ "|" + 
                                    item.Address1+ "|" +
                                    Tumbol + "|" +
                                    Amphur + "|" +
                                    Province + "|" +
                                    item.ZipCode);
                            }
                        }

                        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                        String base64File = Convert.ToBase64String(bytes);

                        // delete file txt
                        if (File.Exists(filePath)) File.Delete(filePath);

                        result.IsSuccess = true;
                        result.Message = fileName + "," + base64File;
                        //result.File = bytes;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ResponseModel> SendSelected(List<Consumer> enrollments, string channel, int brandId)
        {
            var response = new ResponseModel();
            try
            {
                var intArray = enrollments.Select(x => x.Id).ToArray();
                var mainUri = this.configuration["GMCServices:Uri"];
                var apiPath = this.configuration["GMCServices:AutomationApiPath"];
                //var stringPayload = JsonConvert.SerializeObject(intArray);
                var stringPayload = JsonConvert.SerializeObject(new
                {
                    channel = channel,
                    type = "consumer",
                    id = intArray,

                });
                var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var responseFromApi = await client.PostAsync(mainUri + apiPath, content);
                    var result = await responseFromApi.Content.ReadAsStringAsync();
                }

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> SendAll(PaginationModel data, string channel, int brandId)
        {
            var response = new ResponseModel();
            try
            {
                data.BrandId = brandId;
                var consumers = await this.consumerRepository.GetConsumersByBrandIdAsync(data, "API");
                if (consumers != null)
                {
                    var intArray = consumers.Select(x => x.Id).ToArray();
                    var mainUri = this.configuration["GMCServices:Uri"];
                    var apiPath = this.configuration["GMCServices:AutomationApiPath"];
                    var stringPayload = JsonConvert.SerializeObject(new
                    {
                        channel = channel,
                        type = "consumer",
                        id = intArray,

                    });
                    var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        var responseFromApi = await client.PostAsync(mainUri + apiPath, content);
                        var result = await responseFromApi.Content.ReadAsStringAsync();
                    }

                    response.IsSuccess = true;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found consumer.";
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
