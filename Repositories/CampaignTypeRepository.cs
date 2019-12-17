using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ICampaignTypeRepository
    {
        Task<List<CampaignType>> GetCampaignTypesAsync();
    }

    public class CampaignTypeRepository: ICampaignTypeRepository
    {
        public async Task<List<CampaignType>> GetCampaignTypesAsync()
        {
            using(var Context = new CSP_RedemptionContext())
            {
                return await Context.CampaignType.Where(x=>x.IsActive == true).ToListAsync();
            }
        }

    }
}
