using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IFunctionRepository
    {
        Task<List<Function>> GetFunctionsAsync();
        Task<List<Function>> GetFunctionsByRoleIdAsync(int roleId);
        Task<Function> GetFunctionsByIdAsync(int functionId);
    }
    public class FunctionRepository : IFunctionRepository
    {
        public async Task<List<Function>> GetFunctionsAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Function.ToListAsync();
            }
        }

        public async Task<List<Function>> GetFunctionsByRoleIdAsync(int roleId)
        {
            var menus = new List<Function>();
            using (var Context = new CSP_RedemptionContext())
            {
                var menuRole = await Context.RoleFunction.Where(x => x.RoleId == roleId).ToListAsync();
                foreach (var item in menuRole)
                {
                    var menu = await Context.Function.Where(x => x.Id == item.FunctionId && x.IsActived == true).FirstOrDefaultAsync();
                    if (menu != null)
                        menus.Add(menu);
                }
                return menus;
            }
        }

        public async Task<Function> GetFunctionsByIdAsync(int functionId)
        {
            using (var Context = new CSP_RedemptionContext())
            {

                return await Context.Function.Where(x => x.Id == functionId).FirstOrDefaultAsync();
            }
        }
    }
}
