using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IAmphurRepository
    {
        Task<List<Amphur>> GetAmphursByProvinceCodeAsync(string provinceCode);
    }
    public class AmphurRepository: IAmphurRepository
    {
        public async Task<List<Amphur>> GetAmphursByProvinceCodeAsync(string provinceCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Amphur.Where(x => x.ProvinceCode == provinceCode).ToListAsync();
            }
        }
    }
}
