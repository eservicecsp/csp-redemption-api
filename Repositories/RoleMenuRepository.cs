using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IRoleMenuRepository
    {
        Task<List<RoleMenu>> GetRoleMenusByRoleIdAsync(int roleId);
    }

    public class RoleMenuRepository : IRoleMenuRepository
    {
        public async Task<List<RoleMenu>> GetRoleMenusByRoleIdAsync(int roleId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.RoleMenu.Include(x => x.Menu).Where(x => x.RoleId == roleId).ToListAsync();
            }
        }
    }
}
