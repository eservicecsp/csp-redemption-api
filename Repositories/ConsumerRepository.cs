using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
    public class ConsumerRepository : IConsumerRepository
    {
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
    }
}
