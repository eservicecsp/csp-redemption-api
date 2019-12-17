using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IQrCodeService
    {
        Task<QrCodeResponseModel> GetQrCodeByCampaignIdAsync(PaginationModel data);
    }
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeRepository qrCodeRepository;
        private readonly ICampaignRepository campaignRepository;

        public QrCodeService
            (
                IQrCodeRepository qrCodeRepository,
                ICampaignRepository campaignRepository
            )
        {
            this.qrCodeRepository = qrCodeRepository;
            this.campaignRepository = campaignRepository;
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
                            FirstName = item.Consumer == null ? null : item.Consumer.FirstName,
                            LastName = item.Consumer == null ? null : item.Consumer.LastName,
                            Email = item.Consumer == null ? null : item.Consumer.Email,
                            Phone = item.Consumer == null ? null : item.Consumer.Phone,
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
    }
}
