using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using FastMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ICampaignRepository
    {
        Task<Campaign> GetCampaignByIdAsync(int campaignId);
        Task<List<Campaign>> GetCampaignsByBrandIdAsync(int brandId);
        Task<ResponseModel> CreateAsync(CreateCampaignRequestModel requestModel, List<QrCode> qrCodes, Campaign campaign);
        Task<bool> UpdateAsync(Campaign campaign);
    }
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IConfiguration configuration;
        public CampaignRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Campaign
                                    .Include(x=>x.CampaignProduct)
                                    .Include(x=>x.CampaignDealer)
                                    .Include(x=>x.Collection)
                                    .Where(x => x.Id.Equals(campaignId))
                                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<Campaign>> GetCampaignsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Campaign.Where(x => x.BrandId == brandId).OrderByDescending(x=>x.Id).ToListAsync();
            }
        }

        public async Task<ResponseModel> CreateAsync(CreateCampaignRequestModel requestModel, List<QrCode> qrCodes, Campaign campaign)
        {
            var response = new ResponseModel();
            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string connectionString = this.configuration["ConnectionStrings:ApiDatabase"];

                        // Campaign
                        await Context.Campaign.AddAsync(campaign);
                        await Context.SaveChangesAsync();
                        int campaignId = campaign.Id;

                        if(requestModel.Campaign.Dealers != null)
                        {
                            List<CampaignDealer> campaignDealer = new List<CampaignDealer>();
                            foreach (var item in requestModel.Campaign.Dealers)
                            {
                                campaignDealer.Add(new CampaignDealer()
                                {
                                    CampaignId = campaignId,
                                    DealerId = item.Id
                                });
                            }
                            // CampaignDealer
                            await Context.CampaignDealer.AddRangeAsync(campaignDealer);
                            await Context.SaveChangesAsync();
                        }
                        

                        var copyParameters = new[]
                        {
                            nameof(QrCode.Token),
                            nameof(QrCode.CampaignId),
                            nameof(QrCode.Code),
                            nameof(QrCode.Peice),
                            nameof(QrCode.ConsumerId),
                            nameof(QrCode.TransactionId),
                            nameof(QrCode.Point),
                            nameof(QrCode.ScanDate),
                        };

                        switch (requestModel.Campaign.CampaignTypeId)
                        {
                            case 1: // Collecting
                                {
                                    List<Collection> collections = new List<Collection>();
                                    foreach(var item in requestModel.Campaign.CollectingData)
                                    {
                                        collections.Add(new Collection() { 
                                            CampaignId = campaignId,
                                            Quantity = item.Quantity,
                                            WasteQuantity = item.WasteQuantity,
                                            TotalQuantity = item.TotalQuantity,
                                            CollectionRow = item.row,
                                            CollectionColumn = item.column,
                                            CollectionName = item.name,
                                            CollectionPath = item.path,
                                            CollectionFile = item.file,
                                            Extension = item.extension
                                        });
                                    }
                                    // Collection
                                    await Context.Collection.AddRangeAsync(collections);
                                    await Context.SaveChangesAsync();

                                    int index = 1;
                                    int peiceIndex = 0;

                                    foreach (var item in qrCodes)
                                    {
                                        if (index <= requestModel.Peices[peiceIndex])
                                        {
                                            item.Peice = peiceIndex + 1;
                                            item.CampaignId = campaignId;
                                            index++;
                                        }
                                        else
                                        {
                                            index = 1;
                                            peiceIndex++;
                                            //tempTokens.Add(item);
                                        }
                                    }

                                    if (qrCodes.Any(x => x.Peice == null))
                                    {
                                        foreach (var item in qrCodes.Where(x => x.Peice == null))
                                        {
                                            item.Peice = peiceIndex + 1;
                                            item.CampaignId = campaignId;
                                        }
                                    }

                                    using (var bcp = new SqlBulkCopy(connectionString))
                                    {
                                        // map type with parameter that has property nullable;
                                        using (var reader = ObjectReader.Create(qrCodes, copyParameters))
                                        {
                                            SqlBulkCopyColumnMapping mapToken = new SqlBulkCopyColumnMapping(copyParameters[0], "Token");
                                            SqlBulkCopyColumnMapping mapCampaignId = new SqlBulkCopyColumnMapping(copyParameters[1], "CampaignId");
                                            SqlBulkCopyColumnMapping mapPeice = new SqlBulkCopyColumnMapping(copyParameters[3], "Peice");
                                            SqlBulkCopyColumnMapping mapConsumerId = new SqlBulkCopyColumnMapping(copyParameters[4], "ConsumerId");
                                            SqlBulkCopyColumnMapping mapTransactionId = new SqlBulkCopyColumnMapping(copyParameters[5], "TransactionId");
                                            SqlBulkCopyColumnMapping mapPoint = new SqlBulkCopyColumnMapping(copyParameters[6], "Point");
                                            SqlBulkCopyColumnMapping mapScanDate = new SqlBulkCopyColumnMapping(copyParameters[7], "ScanDate");

                                            bcp.ColumnMappings.Add(mapToken);
                                            bcp.ColumnMappings.Add(mapCampaignId);
                                            bcp.ColumnMappings.Add(mapPeice);
                                            bcp.ColumnMappings.Add(mapConsumerId);
                                            bcp.ColumnMappings.Add(mapTransactionId);
                                            bcp.ColumnMappings.Add(mapPoint);
                                            bcp.ColumnMappings.Add(mapScanDate);

                                            bcp.DestinationTableName = "[QrCode]";
                                            bcp.WriteToServer(reader);
                                        }
                                    }
                                    break;
                                }
                            case 2: // Point & Reward
                                {
                                    // QrCodes
                                    qrCodes.Select(q => { q.CampaignId = campaignId; q.Point = requestModel.Point; return q; }).ToList();
                                    //qrCodes.ToList().ForEach(c => c.CampaignId = campaign.Id);

                                    using (var bcp = new SqlBulkCopy(connectionString))
                                    {
                                        // map type with parameter that has property nullable;
                                        using (var reader = ObjectReader.Create(qrCodes, copyParameters))
                                        {
                                            SqlBulkCopyColumnMapping mapToken = new SqlBulkCopyColumnMapping(copyParameters[0], "Token");
                                            SqlBulkCopyColumnMapping mapCampaignId = new SqlBulkCopyColumnMapping(copyParameters[1], "CampaignId");
                                            SqlBulkCopyColumnMapping mapPeice = new SqlBulkCopyColumnMapping(copyParameters[3], "Peice");
                                            SqlBulkCopyColumnMapping mapConsumerId = new SqlBulkCopyColumnMapping(copyParameters[4], "ConsumerId");
                                            SqlBulkCopyColumnMapping mapTransactionId = new SqlBulkCopyColumnMapping(copyParameters[5], "TransactionId");
                                            SqlBulkCopyColumnMapping mapPoint = new SqlBulkCopyColumnMapping(copyParameters[6], "Point");
                                            SqlBulkCopyColumnMapping mapScanDate = new SqlBulkCopyColumnMapping(copyParameters[7], "ScanDate");

                                            bcp.ColumnMappings.Add(mapToken);
                                            bcp.ColumnMappings.Add(mapCampaignId);
                                            bcp.ColumnMappings.Add(mapPeice);
                                            bcp.ColumnMappings.Add(mapConsumerId);
                                            bcp.ColumnMappings.Add(mapTransactionId);
                                            bcp.ColumnMappings.Add(mapPoint);
                                            bcp.ColumnMappings.Add(mapScanDate);

                                            bcp.DestinationTableName = "[QrCode]";
                                            bcp.WriteToServer(reader);
                                        }
                                    }
                                    break;
                                }
                            case 3: // Enrollment & Member
                                {
                                    // QrCodes
                                    qrCodes.Select(q => { q.CampaignId = campaignId; return q; }).ToList();
                                    //qrCodes.ToList().ForEach(c => c.CampaignId = campaign.Id);

                                    using (var bcp = new SqlBulkCopy(connectionString))
                                    {
                                        // map type with parameter that has property nullable;
                                        using (var reader = ObjectReader.Create(qrCodes, copyParameters))
                                        {
                                            SqlBulkCopyColumnMapping mapToken = new SqlBulkCopyColumnMapping(copyParameters[0], "Token");
                                            SqlBulkCopyColumnMapping mapCampaignId = new SqlBulkCopyColumnMapping(copyParameters[1], "CampaignId"); 
                                            SqlBulkCopyColumnMapping mapCode = new SqlBulkCopyColumnMapping(copyParameters[2], "Code");
                                            SqlBulkCopyColumnMapping mapPeice = new SqlBulkCopyColumnMapping(copyParameters[3], "Peice");
                                            SqlBulkCopyColumnMapping mapConsumerId = new SqlBulkCopyColumnMapping(copyParameters[4], "ConsumerId");
                                            SqlBulkCopyColumnMapping mapTransactionId = new SqlBulkCopyColumnMapping(copyParameters[5], "TransactionId");
                                            SqlBulkCopyColumnMapping mapPoint = new SqlBulkCopyColumnMapping(copyParameters[6], "Point");
                                            SqlBulkCopyColumnMapping mapScanDate = new SqlBulkCopyColumnMapping(copyParameters[7], "ScanDate");

                                            bcp.ColumnMappings.Add(mapToken);
                                            bcp.ColumnMappings.Add(mapCampaignId);
                                            bcp.ColumnMappings.Add(mapCode);
                                            bcp.ColumnMappings.Add(mapPeice);
                                            bcp.ColumnMappings.Add(mapConsumerId);
                                            bcp.ColumnMappings.Add(mapTransactionId);
                                            bcp.ColumnMappings.Add(mapPoint);
                                            bcp.ColumnMappings.Add(mapScanDate);

                                            bcp.DestinationTableName = "[QrCode]";
                                            bcp.WriteToServer(reader);
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        var campProduct = new CampaignProduct()
                        {
                            CampaignId = campaignId,
                            ProductId = requestModel.Product
                        };
                        await Context.CampaignProduct.AddAsync(campProduct);
                        await Context.SaveChangesAsync();

                        //List<CampaignDealer> campaignDealers = new List<CampaignDealer>();
                        //foreach (var dealer in requestModel.Campaign.Dealers)
                        //{
                        //    campaignDealers.Add(new CampaignDealer()
                        //    {
                        //           CampaignId = campaignId,
                        //           DealerId = dealer.Id,
                        //    });
                        //}
                        //await Context.CampaignDealer.AddRangeAsync(campaignDealers);
                        //await Context.SaveChangesAsync();



                        transaction.Commit();
                        response.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Message = ex.Message;
                    }
                }
            }

            return response;
        }

        public async Task<bool> UpdateAsync(Campaign campaign)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Campaign thisRow = await Context.Campaign.SingleAsync(x => x.Id == campaign.Id);
                thisRow.Name = campaign.Name;
                thisRow.Description = campaign.Description;
                thisRow.StartDate = campaign.StartDate;
                thisRow.EndDate = campaign.EndDate;
                thisRow.AlertMessage = campaign.AlertMessage;
                thisRow.DuplicateMessage = campaign.DuplicateMessage;
                thisRow.QrCodeNotExistMessage = campaign.QrCodeNotExistMessage;
                thisRow.WinMessage = campaign.WinMessage;
                Context.Entry(thisRow).CurrentValues.SetValues(thisRow);
                return await Context.SaveChangesAsync() > 0;

            }
        }
    }
}
