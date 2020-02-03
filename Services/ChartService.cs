using CSP_Redemption_WebApi.Entities.Models;
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
        Task<ChartResponseModel> GetGraphCampaignByBrandId(SearchGraphModel searchGraph);
    }
    public class ChartService: IChartService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IQrCodeRepository qrCodeRepository;
        private readonly IProvinceRepository  provinceRepository;
        private readonly IAmphurRepository amphurRepository;
        private readonly ITumbolRepository tumbolRepository;
        private readonly ICampaignRepository campaignRepository;

        public ChartService
            (
            ITransactionRepository transactionRepository,
            IQrCodeRepository qrCodeRepository,
            IProvinceRepository provinceRepository,
            IAmphurRepository amphurRepository,
            ITumbolRepository tumbolRepository,
            ICampaignRepository campaignRepository
            )
        {
            this.transactionRepository = transactionRepository;
            this.qrCodeRepository = qrCodeRepository;
            this.provinceRepository = provinceRepository;
            this.amphurRepository = amphurRepository;
            this.tumbolRepository = tumbolRepository;
            this.campaignRepository = campaignRepository;
        }

        public async Task<ChartResponseModel> GetChartTransaction(int canpaignId)
        {
            var response = new ChartResponseModel();
            try
            {
                var charts = new List<ChartsModel>();

                var all = new ChartsModel();
                all.name = "Enroll";
                all.value = await this.transactionRepository.GetCountAllTransaction(canpaignId);
                charts.Add(all);

                var success = new ChartsModel();
                success.name = "Success";
                success.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 4);
                charts.Add(success);

                var fail = new ChartsModel();
                fail.name = "Fail";
                fail.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 1);
                charts.Add(fail);

                var empty = new ChartsModel();
                empty.name = "Invalid";
                empty.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 2);
                charts.Add(empty);

                var dup = new ChartsModel();
                dup.name = "Duplicate";
                dup.value = await this.transactionRepository.GetCountTransactionByTypeId(canpaignId, 3);
                charts.Add(dup);

                

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
                int Qrall = await this.qrCodeRepository.GetCountQrCode(canpaignId);
                var all = new ChartsModel();
                all.name = "Total";
                all.value = Qrall;
                charts.Add(all);

                int used = await this.qrCodeRepository.GetCountQrCodeUsed(canpaignId);
                var redeem = new ChartsModel();
                redeem.name = "Used";
                redeem.value = used;
                charts.Add(redeem);

                var unused = new ChartsModel();
                unused.name = "Unused";
                unused.value = Qrall - used;
                charts.Add(unused);

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
                var MarkerProvinces = new List<MarkerProvincesModel>();

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

                        var MarkerProvince = new MarkerProvincesModel();
                        MarkerProvince.Location = $"{province.NameTh} : {countTran}";
                        MarkerProvince.Latitude = province.Latitude;
                        MarkerProvince.Longitude = province.Longitude;
                        MarkerProvince.Total = countTran;

                        MarkerProvinces.Add(MarkerProvince);
                    }

                }
                //MarkerProvinces = MarkerProvinces.OrderByDescending(x => x.Total);

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
                response.MarkerProvinces = MarkerProvinces;
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ChartResponseModel> GetGraphCampaignByBrandId(SearchGraphModel searchGraph)
        {
            var response = new ChartResponseModel();
            try
            {
                var campaigns = await this.campaignRepository.GetCampaignsActiveByBrandIdAsync(searchGraph.brandId);
                List<GraphModel> graphModels = new List<GraphModel>();
                if (campaigns != null)
                {
                    if(searchGraph.startDate != null)
                    {
                        campaigns = campaigns.Where(x => x.StartDate.Value.Date >= searchGraph.startDate.Value.Date).ToList();
                    }
                    if (searchGraph.endDate != null)
                    {
                        campaigns = campaigns.Where(x => x.EndDate.Value.Date <= searchGraph.endDate.Value.Date).ToList();
                    }

                    
                    // Line 1
                    //var TranAll = new GraphModel();
                    //TranAll.name = "Total";
                    //List<ChartsModel> seriesAll = new List<ChartsModel>();
                    //foreach (var campaign in campaigns)
                    //{
                    //    seriesAll.Add(new ChartsModel()
                    //    {
                    //        name = campaign.Name,
                    //        // value = await this.transactionRepository.GetCountAllTransaction(campaign.Id)
                    //        value = await this.qrCodeRepository.GetCountQrCode(campaign.Id)
                    //    });

                    //}
                    //TranAll.series = seriesAll;
                    //graphModels.Add(TranAll);

                    //var TranSuccess = new GraphModel();
                    //TranSuccess.name = "Success";
                    //List<ChartsModel> seriesSuccess = new List<ChartsModel>();
                    //foreach (var campaign in campaigns)
                    //{
                    //    seriesSuccess.Add(new ChartsModel()
                    //    {
                    //        name = campaign.Name,
                    //        value = await this.transactionRepository.GetCountTransactionByTypeId(campaign.Id, 4)
                    //    });

                    //}
                    //TranSuccess.series = seriesSuccess;
                    //graphModels.Add(TranSuccess);

                    //var TranOther = new GraphModel();
                    //TranOther.name = "Other";
                    //List<ChartsModel> seriesOther = new List<ChartsModel>();
                    //foreach (var campaign in campaigns)
                    //{
                    //    int countAll = await this.transactionRepository.GetCountAllTransaction(campaign.Id);
                    //    int countSuccess = await this.transactionRepository.GetCountTransactionByTypeId(campaign.Id, 4);
                    //    seriesOther.Add(new ChartsModel()
                    //    {
                    //        name = campaign.Name,
                    //        value = countAll - countSuccess
                    //    });

                    //}
                    //TranOther.series = seriesOther;
                    //graphModels.Add(TranOther);

                    // Line 2

                    foreach (var campaign in campaigns.OrderByDescending(x=>x.Id))
                    {
                        var campaignGroup = new GraphModel();
                        campaignGroup.name = campaign.Name;
                        List<ChartsModel> series = new List<ChartsModel>();
                        int countAll = await this.transactionRepository.GetCountAllTransaction(campaign.Id);
                        int countQr = await this.qrCodeRepository.GetCountQrCode(campaign.Id);
                        int countSuccess = await this.transactionRepository.GetCountTransactionByTypeId(campaign.Id, 4);
                        series.Add(new ChartsModel()
                        {
                            name = "Total",
                            value = countQr
                        });

                        series.Add(new ChartsModel()
                        {
                            name = "Unused",
                            value = countQr - countSuccess
                        });

                        series.Add(new ChartsModel()
                        {
                            name = "Success",
                            value = countSuccess
                        });
                        series.Add(new ChartsModel()
                        {
                            name = "Not success",
                            value = countAll - countSuccess
                        });
                        campaignGroup.series = series;
                        graphModels.Add(campaignGroup);

                    }

                    // Bar 1
                    //series.Add(success);

                    //foreach (var campaign in campaigns)
                    //{
                    //    List<ChartsModel> series = new List<ChartsModel>();

                    //    int countAll = await this.transactionRepository.GetCountAllTransaction(campaign.Id);
                    //    int countSuccess = await this.transactionRepository.GetCountTransactionByTypeId(campaign.Id, 4);
                    //    int countOther = countAll - countSuccess;


                    //    var success = new ChartsModel();
                    //    success.name = "Success";
                    //    success.value = countSuccess;
                    //    series.Add(success);

                    //    var other = new ChartsModel();
                    //    other.name = "Other";
                    //    other.value = countOther;
                    //    series.Add(other);

                    //    graphModels.Add(new GraphModel()
                    //    {
                    //        name = campaign.Name,
                    //        series = series
                    //    });



                    //}

                }
                response.IsSuccess = true;
                response.graphs = graphModels;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
           
            return response;
        }
    }
}
