using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ICampaignRepository
    {
        Task<Campaign> GetCampaignByIdAsync(int campaignId);
    }
    public class CampaignRepository: ICampaignRepository
    {
        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Campaign.Where(x => x.Id.Equals(campaignId)).FirstOrDefaultAsync();
            }
        }
    }
}
