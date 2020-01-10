﻿using CSP_Redemption_WebApi.Entities.Models;
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
        Task<ChartResponseModel> GetChartProvince(int campaignId);
    }
    public class ChartService: IChartService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IQrCodeRepository qrCodeRepository;
        private readonly IProvinceRepository  provinceRepository;
        private readonly IAmphurRepository amphurRepository;
        private readonly ITumbolRepository tumbolRepository;

        public ChartService
            (
            ITransactionRepository transactionRepository,
            IQrCodeRepository qrCodeRepository,
            IProvinceRepository provinceRepository,
            IAmphurRepository amphurRepository,
            ITumbolRepository tumbolRepository
            )
        {
            this.transactionRepository = transactionRepository;
            this.qrCodeRepository = qrCodeRepository;
            this.provinceRepository = provinceRepository;
            this.amphurRepository = amphurRepository;
            this.tumbolRepository = tumbolRepository;
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

        public async Task<ChartResponseModel> GetChartProvince(int campaignId)
        {
            var response = new ChartResponseModel();
            try
            {
                var charts = new List<ChartsModel>();

                var provinces = await this.provinceRepository.GetProvincesAsync();
                List<string> zipCodes = new List<string>();
                foreach (var province in provinces)
                {
                    List<string> zipCodesByProvince = new List<string>();

                    var amphurs = await this.amphurRepository.GetAmphursByProvinceCodeAsync(province.Code);
                    List<string> amphurCodes = amphurs.Select(x => x.Code).ToList();
                    var tumbols = await this.tumbolRepository.GetTumbolsByAmphurCodeArrayAsync(amphurCodes.ToArray());
                    //foreach(var amphur in amphurs)
                    //{
                    //    var tumbols = await this.tumbolRepository.GetTumbolsByAmphurCodeAsync(amphur.Code);
                    //    foreach (var tumbol in tumbols.Where(x=>x.ZipCode != null))
                    //    {
                    //        var inArray = zipCodes.Contains(tumbol.ZipCode);
                    //        if (inArray == false)
                    //        {
                    //            zipCodes.Add(tumbol.ZipCode);
                    //            zipCodesByProvince.Add(tumbol.ZipCode);  
                    //        }
                    //    }

                    //}
                    foreach (var tumbol in tumbols)
                    {
                        var inArray = zipCodes.Contains(tumbol.ZipCode);
                        if (inArray == false)
                        {
                            zipCodes.Add(tumbol.ZipCode);
                            zipCodesByProvince.Add(tumbol.ZipCode);
                        }

                    }
                    int countTran = await this.transactionRepository.GetCountTransactionByProvince(zipCodesByProvince.ToArray(), campaignId);
                    if (countTran > 0)
                    {
                        var chart = new ChartsModel();
                        chart.name = province.NameTh;
                        chart.value = countTran;
                        charts.Add(chart);
                    }

                }
                List<string> zipCodesNull = new List<string>();
                zipCodesNull.Add(null);
                int countTranNull = await this.transactionRepository.GetCountTransactionByProvince(zipCodesNull.ToArray(), campaignId);
                if (countTranNull > 0)
                {
                    var chartNull = new ChartsModel();
                    chartNull.name = "Other";
                    chartNull.value = countTranNull;
                    charts.Add(chartNull);
                }

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
