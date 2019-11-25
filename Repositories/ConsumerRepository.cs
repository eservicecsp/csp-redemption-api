using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
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
        Task<List<Consumer>> GetConsumerByBrandIdAsync(int branId);
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
        public async Task<List<Consumer>> GetConsumerByBrandIdAsync(int branId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Consumer.Where(x=>x.BrandId == branId).ToListAsync();
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
