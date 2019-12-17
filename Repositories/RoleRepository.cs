using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoleIdByBrandIsAsync(int brandId);
    }
    public class RoleRepository : IRoleRepository
    {
        public async Task<List<Role>> GetRoleIdByBrandIsAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Role.Where(x=>x.BrandId == brandId).ToListAsync();
            }
        }
    }
}
