using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IProvinceRepository
    {
        Task<List<Province>> GetProvincesAsync();
        Task<List<Province>> GetProvincesByZoneAsync(int zoneId);
    }
    public class ProvinceRepository : IProvinceRepository
    {
        public async Task<List<Province>> GetProvincesAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Province.ToListAsync();
            }
        }

        public async Task<List<Province>> GetProvincesByZoneAsync(int zoneId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Province.Where(x => x.ZoneId == zoneId).ToListAsync();
            }
        }
    }
}
