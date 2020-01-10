using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ITumbolRepository
    {
        Task<List<Tumbol>> GetTumbolsByAmphurCodeAsync(string amphurCode);
        Task<List<Tumbol>> GetTumbolsByAmphurCodeArrayAsync(string[] amphurCode);
    }
    public class TumbolRepository: ITumbolRepository
    {
        public async Task<List<Tumbol>> GetTumbolsByAmphurCodeAsync(string amphurCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Tumbol.Where(x => x.AmphurCode == amphurCode).ToListAsync();
            };
        }
        public async Task<List<Tumbol>> GetTumbolsByAmphurCodeArrayAsync(string[] amphurCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Tumbol.Where(x => amphurCode.Contains(x.AmphurCode) && x.ZipCode != null).ToListAsync();
            };
        }
    }
}
