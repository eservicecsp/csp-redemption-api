using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IZoneRepository
    {
        Task<List<Zone>> GetZoneAsync();
        Task<List<Tumbol>> GetZipCodeAsync();
    }
    public class ZoneRepository: IZoneRepository
    {
        public async Task<List<Zone>> GetZoneAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Zone.ToListAsync();
            }
        }
        public async Task<List<Tumbol>> GetZipCodeAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Tumbol.ToListAsync();
            }
        }
    }
}
