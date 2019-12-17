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
    public interface IConsumerRepository
    {
        Task<Consumer> IsExist(Consumer consumer);
        //Task<List<Consumer>> GetConsumersByBrandIdAsync(int branId);
        Task<List<Consumer>> GetConsumersByBrandIdAsync(PaginationModel data);
        Task<int> GetConsumersTotalByBrandIdAsync(PaginationModel data);
        Task<bool> CreateAsync(Consumer consumer);
        Task<Consumer> GetConsumerByIdAsync(int consumerId);
        Task<bool> UpdateAsync(Consumer consumer);
        Task<bool> ImportFileAsync(List<Consumer>  consumers);
    }
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly IConfiguration configuration;

        public ConsumerRepository
            (
                IConfiguration configuration
            )
        {
            this.configuration = configuration;
        }
        public async Task<Consumer> IsExist(Consumer consumer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Consumer.FirstOrDefaultAsync(x => x.BrandId == consumer.BrandId && x.Phone == consumer.Phone);
            }
        }
        //public async Task<List<Consumer>> GetConsumersByBrandIdAsync(int branId)
        //{
        //    using (var Context = new CSP_RedemptionContext())
        //    {
        //        return await Context.Consumer.Where(x=>x.BrandId == branId).ToListAsync();
        //    }
        //}

        public async Task<List<Consumer>> GetConsumersByBrandIdAsync(PaginationModel data)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var consumers = Context.Consumer.Where(x => x.BrandId == data.BrandId);
                if (data.filter != null)
                {
                    consumers = consumers.Where(x => x.FirstName.Contains(data.filter) ||
                                                 x.LastName.Contains(data.filter) ||
                                                 x.Phone.Contains(data.filter) ||
                                                 x.Email.Contains(data.filter)
                                         );
                }
                
                if (data.sortActive != null)
                {  
                    //FirstName
                    if (data.sortActive == "FirstName" && data.sortDirection == "desc")
                    {
                        consumers = consumers.OrderByDescending(x => x.FirstName);
                    }
                    else if (data.sortActive == "FirstName")
                    {
                        consumers = consumers.OrderBy(x => x.FirstName);
                    }

                    //LastName
                    if (data.sortActive == "LastName" && data.sortDirection == "desc")
                    {
                        consumers = consumers.OrderByDescending(x => x.LastName);
                    }
                    else if (data.sortActive == "LastName")
                    {
                        consumers = consumers.OrderBy(x => x.LastName);
                    }

                    //Phone
                    if (data.sortActive == "Phone" && data.sortDirection == "desc")
                    {
                        consumers = consumers.OrderByDescending(x => x.Phone);
                    }
                    else if (data.sortActive == "Phone")
                    {
                        consumers = consumers.OrderBy(x => x.Phone);
                    }

                    //Email
                    if (data.sortActive == "Email" && data.sortDirection == "desc")
                    {
                        consumers = consumers.OrderByDescending(x => x.Email);
                    }
                    else if (data.sortActive == "Email")
                    {
                        consumers = consumers.OrderBy(x => x.Email);
                    }

                }

                int length = await this.GetConsumersTotalByBrandIdAsync(data);
                int index = 0;
                if (data.pageIndex > 0)
                {
                    index = (data.pageIndex * data.pageSize);
                }


                if (index >= length)
                {
                    consumers = consumers.Skip(0).Take(data.pageSize);
                }
                else
                {
                    consumers = consumers.Skip(index).Take(data.pageSize);
                }
                return await consumers.ToListAsync();
            }
        }

        public async Task<int> GetConsumersTotalByBrandIdAsync(PaginationModel data)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var consumers = Context.Consumer.Where(x => x.BrandId == data.BrandId);
                if (data.filter != null)
                {
                    consumers = consumers.Where(x => x.FirstName.Contains(data.filter) ||
                                                 x.LastName.Contains(data.filter) ||
                                                 x.Phone.Contains(data.filter) ||
                                                 x.Email.Contains(data.filter)
                                         );
                }

                return await consumers.CountAsync();
            }
        }
        public async Task<bool> CreateAsync(Consumer consumer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Context.Consumer.Add(consumer);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        public async Task<Consumer> GetConsumerByIdAsync(int consumerId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Consumer.FirstOrDefaultAsync(x => x.Id == consumerId);
            }
        }

        public async Task<bool> UpdateAsync(Consumer consumer)
        {
            using (var Context = new CSP_RedemptionContext())
            {
              
                var dbConsumer = await Context.Consumer.SingleAsync(x => x.Id == consumer.Id);
                var data = new Consumer
                {
                    Id = consumer.Id,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Email = consumer.Email,
                    Phone = consumer.Phone,
                    BirthDate = consumer.BirthDate,
                    Address1 = consumer.Address1,
                    Address2 = consumer.Address2,
                    TumbolCode = consumer.TumbolCode,
                    AmphurCode = consumer.AmphurCode,
                    ProvinceCode = consumer.ProvinceCode,
                    ZipCode = consumer.ZipCode,
                    ConsumerSourceId = dbConsumer.ConsumerSourceId,
                    BrandId = dbConsumer.BrandId,
                    CampaignId = consumer.CampaignId,
                    Point = consumer.Point,
                    CreatedBy = dbConsumer.CreatedBy,
                    CreatedDate = dbConsumer.CreatedDate
                };
                Context.Entry(dbConsumer).CurrentValues.SetValues(data);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> ImportFileAsync(List<Consumer> consumers)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string connectionstring = this.configuration["ConnectionStrings:ApiDatabase"];
                        var copyParameters = new[]
                        {
                            nameof(Consumer.FirstName),
                            nameof(Consumer.LastName),
                            nameof(Consumer.Phone),
                            nameof(Consumer.Email),
                            nameof(Consumer.BrandId),
                            nameof(Consumer.ConsumerSourceId),
                            nameof(Consumer.CreatedDate)
                        };
                        using (var bcp = new SqlBulkCopy(connectionstring))
                        {
                            using (var reader = ObjectReader.Create(consumers, copyParameters))
                            {
                                SqlBulkCopyColumnMapping mapFirstName = new SqlBulkCopyColumnMapping(copyParameters[0], "FirstName");
                                SqlBulkCopyColumnMapping mapLastName = new SqlBulkCopyColumnMapping(copyParameters[1], "LastName");
                                SqlBulkCopyColumnMapping mapPhone = new SqlBulkCopyColumnMapping(copyParameters[2], "Phone");
                                SqlBulkCopyColumnMapping mapEmail = new SqlBulkCopyColumnMapping(copyParameters[3], "Email");
                                SqlBulkCopyColumnMapping mapBrandId = new SqlBulkCopyColumnMapping(copyParameters[4], "BrandId");
                                SqlBulkCopyColumnMapping mapConsumerSourceId = new SqlBulkCopyColumnMapping(copyParameters[5], "ConsumerSourceId");
                                SqlBulkCopyColumnMapping mapCreatedDate = new SqlBulkCopyColumnMapping(copyParameters[6], "CreatedDate");

                                bcp.ColumnMappings.Add(mapFirstName);
                                bcp.ColumnMappings.Add(mapLastName);
                                bcp.ColumnMappings.Add(mapPhone);
                                bcp.ColumnMappings.Add(mapEmail);
                                bcp.ColumnMappings.Add(mapBrandId);
                                bcp.ColumnMappings.Add(mapConsumerSourceId);
                                bcp.ColumnMappings.Add(mapCreatedDate);
                                bcp.DestinationTableName = "[Consumer]";
                                bcp.WriteToServer(reader);
                            }
                        }
                        isSuccess = true;
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    
                }
            }
                return isSuccess;
        }
    }
}
