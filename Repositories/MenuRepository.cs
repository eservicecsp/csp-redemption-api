using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetMenusAsync();
        Task<List<Menu>> GetMenusByParentIdAsync(int parentId);
        Task<List<Menu>> GetMenusByRoleIdAsync(int roleId);
        Task<Menu> GetMenusBtIdAsync(int menuId);
    }
    public class MenuRepository : IMenuRepository
    {
        public async Task<List<Menu>> GetMenusAsync()
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Menu.ToListAsync();
            }
        }

        public async Task<List<Menu>> GetMenusByParentIdAsync(int parentId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Menu.Where(x => x.ParentId == parentId).ToListAsync();
            }
        }
        public async Task<List<Menu>> GetMenusByRoleIdAsync(int roleId)
        {
            var menus = new List<Menu>();
            using (var Context = new CSP_RedemptionContext())
            {
                var menuRole = await Context.RoleMenu.Where(x => x.RoleId == roleId).ToListAsync();
                foreach (var item in menuRole)
                {
                    var menu = await Context.Menu.SingleAsync(x => x.Id == item.MenuId);
                    menus.Add(menu);
                }
                return menus;
            }
        }

        public async Task<Menu> GetMenusBtIdAsync(int menuId)
        {
            using (var Context = new CSP_RedemptionContext())
            {

                return await Context.Menu.Where(x => x.Id == menuId).FirstOrDefaultAsync();
            }
        }
    }
}
