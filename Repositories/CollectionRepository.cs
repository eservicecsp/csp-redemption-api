using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ICollectionRepository
    {
        Task<List<Collection>> GetCollecttionsByCampaignIdAsync(int campaignId);
    }
    public class CollectionRepository: ICollectionRepository
    {
        public async Task<List<Collection>> GetCollecttionsByCampaignIdAsync(int campaignId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Collection.Where(x => x.CampaignId == campaignId).ToListAsync();
            }
        }
    }
}
