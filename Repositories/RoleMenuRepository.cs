using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IRoleFunctionRepository
    {
        Task<List<RoleFunction>> GetRoleFunctionsByRoleIdAsync(int roleId);
    }

    public class RoleFunctionRepository : IRoleFunctionRepository
    {
        public async Task<List<RoleFunction>> GetRoleFunctionsByRoleIdAsync(int roleId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.RoleFunction.Include(x => x.Function).Where(x => x.RoleId == roleId).ToListAsync();
            }
        }
    }
}
