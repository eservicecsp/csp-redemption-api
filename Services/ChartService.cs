using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IChartService
    {
        Task<ChartResponseModel> GetChartTransaction(int canpaignId);
        Task<ChartResponseModel> GetChartQrCode(int canpaignId);
    }
    public class ChartService: IChartService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IQrCodeRepository qrCodeRepository;

        public ChartService
            (
            ITransactionRepository transactionRepository,
            IQrCodeRepository qrCodeRepository
            )
        {
            this.transactionRepository = transactionRepository;
            this.qrCodeRepository = qrCodeRepository;
        }

        public async Task<ChartResponseModel> GetChartTransaction(int canpaignId)
        {
            var response = new ChartResponseModel();
            try
            {
                var charts = new List<ChartsModel>();

                var all = new ChartsModel();
                all.name = "ALL";
                all.value = await this.transactionRepository.GetCountAllTransaction(canpaignId);
                charts.Add(all);

                var fail = new ChartsModel();
                fail.name = "Fail";
                fail.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 1);
                charts.Add(fail);

                var empty = new ChartsModel();
                empty.name = "Empty";
                empty.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 2);
                charts.Add(empty);

                var dup = new ChartsModel();
                dup.name = "Duplicate";
                dup.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 3);
                charts.Add(dup);

                var success = new ChartsModel();
                success.name = "Success";
                success.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 4);
                charts.Add(success);

                response.charts = charts;
                response.IsSuccess = true;

            } catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ChartResponseModel> GetChartQrCode(int canpaignId)
        {
            var response = new ChartResponseModel();
            try
            {
                var charts = new List<ChartsModel>();

                var all = new ChartsModel();
                all.name = "ALL";
                all.value = await this.qrCodeRepository.GetCountQrCode(canpaignId);
                charts.Add(all);

                var redeem = new ChartsModel();
                redeem.name = "Redeem";
                redeem.value = await this.qrCodeRepository.GetCountQrCodeUsed(canpaignId);
                charts.Add(redeem);

                response.charts = charts;
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
