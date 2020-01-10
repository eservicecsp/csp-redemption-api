using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IQrCodeService
    {
        Task<QrCodeResponseModel> GetQrCodeByCampaignIdAsync(PaginationModel data);
        Task<FileResponseDataBinding> ExportTextQrCodeByCampaignId(int campaignId, int campaignTypeId);
    }
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeRepository qrCodeRepository;
        private readonly ICampaignRepository campaignRepository;
        private readonly IEnrollmentRepository enrollmentRepository;

        public QrCodeService
            (
                IQrCodeRepository qrCodeRepository,
                ICampaignRepository campaignRepository,
                IEnrollmentRepository enrollmentRepository
            )
        {
            this.qrCodeRepository = qrCodeRepository;
            this.campaignRepository = campaignRepository;
            this.enrollmentRepository = enrollmentRepository;
        }

        public async Task<QrCodeResponseModel> GetQrCodeByCampaignIdAsync(PaginationModel data)
        {
            var response = new QrCodeResponseModel();
            try
            {
                var dbQrCodes = await this.qrCodeRepository.GetQrCodeByCompanyIdAsync(data);
                var camp = await this.campaignRepository.GetCampaignByIdAsync(dbQrCodes[0].CampaignId);

                if (dbQrCodes != null)
                {
                    var qrCodes = new List<QrCodeModel>();
                    foreach (var item in dbQrCodes)
                    {
                        string campaignUrl = null;
                        string FirstName = string.Empty;
                        string LastName = string.Empty;
                        string Email = string.Empty;
                        string Phone = string.Empty;
                        if (item.ConsumerId == null && item.EnrollmentId != null)
                        {
                            var enrollment = await this.enrollmentRepository.GetEnrollmentByIdAsync(item.EnrollmentId.Value);
                            FirstName = enrollment.FirstName;
                            LastName = enrollment.LastName;
                            Email = enrollment.Email;
                            Phone = enrollment.Tel;
                        }
                        else
                        {
                            FirstName = item.Consumer == null ? null : item.Consumer.FirstName;
                            LastName = item.Consumer == null ? null : item.Consumer.LastName;
                            Email = item.Consumer == null ? null : item.Consumer.Email;
                            Phone = item.Consumer == null ? null : item.Consumer.Phone;
                        }

                        campaignUrl = camp.Url.Replace("[#token#]", item.Token);
                        campaignUrl = campaignUrl.Replace("[#campaignId#]", Convert.ToString(item.CampaignId));
                        qrCodes.Add(new QrCodeModel()
                        {
                            Token = item.Token,
                            Peice = item.Peice,
                            Code = item.Code,
                            ConsumerId = item.ConsumerId,
                            TransactionId = item.TransactionId,
                            Point = item.Point,
                            ScanDate = item.ScanDate,
                            FirstName = FirstName ,
                            LastName = LastName,
                            Email = Email,
                            Phone = Phone,
                            FullUrl = campaignUrl
                        }); ;
                    }
                    response.length = await this.qrCodeRepository.GetQrCodeTotalByCompanyIdAsync(data);
                    response.data = qrCodes;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<FileResponseDataBinding> ExportTextQrCodeByCampaignId(int campaignId, int campaignTypeId)
        {
            FileResponseDataBinding result = new FileResponseDataBinding();
            try
            {
                var campaign = await this.campaignRepository.GetCampaignByIdAsync(campaignId);
                var qrCodesDb = await this.qrCodeRepository.ExportQrCodeByCompanyIdAsync(campaignId);
                if (qrCodesDb.Count() > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + campaignId + "_" + "qrCode.txt";
                    string filePath = Path.Combine(@"Upload\" + fileName);
                    if (!File.Exists(filePath))
                    {
                        string url = campaign.Url.Replace("[#campaignId#]", Convert.ToString(campaignId));

                        using (StreamWriter writer = File.CreateText(filePath))
                        //using (StreamWriter writer = new StreamWriter(pathFile, true))
                        {

                            if(campaignTypeId == 3)
                            {
                                writer.WriteLine("Url|Code");
                                foreach (var item in qrCodesDb)
                                {
                                    
                                    string newUrl = url.Replace("[#token#]", item.Token);
                                    writer.WriteLine($"{newUrl}|{item.Code}");
                                }
                            }
                            else
                            {
                                writer.WriteLine("Url");
                                foreach (var item in qrCodesDb)
                                {
                                    string newUrl = url.Replace("[#token#]", item.Token);
                                    writer.WriteLine($"{newUrl}");
                                }
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
    }
}
