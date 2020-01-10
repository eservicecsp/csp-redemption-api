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
        Task<List<Role>> GetRolesAsync(int brandId);
        Task<Role> GetRoleAsync(int id);
        Task<bool> CreateAsync(Role iRole);
        Task<bool> UpdateAsync(Role uRole);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly CSP_RedemptionContext _context;

        public RoleRepository(CSP_RedemptionContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAsync(int brandId)
        {
            return await _context.Role
                .Include(r => r.RoleFunction)
                .ThenInclude(x => x.Function)
                .Where(x => x.BrandId == brandId).ToListAsync();
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            return await _context.Role
                .Include(x => x.RoleFunction)
                .ThenInclude(x => x.Function)
                .FirstOrDefaultAsync(x => x.Id == id);
            //.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreateAsync(Role role)
        {
            await _context.Role.AddAsync(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var existingRole = await _context.Role
                       .Include(x => x.RoleFunction)
                       .SingleOrDefaultAsync(x => x.Id == role.Id);

                    if (existingRole != null)
                    {
                        // Update Role
                        _context.Entry(existingRole).CurrentValues.SetValues(role);

                        // Delete RoleFunction
                        foreach (var existingRoleFunction in existingRole.RoleFunction.ToList())
                        {
                            if (!role.RoleFunction.Any(x => x.FunctionId == existingRoleFunction.FunctionId && x.RoleId == existingRoleFunction.RoleId))
                                _context.RoleFunction.Remove(existingRoleFunction);
                        }

                        // Update and Insert RowFunction
                        foreach (var roleFunction in role.RoleFunction)
                        {
                            var existingRoleFunction = existingRole.RoleFunction
                                .Where(x => x.FunctionId == roleFunction.FunctionId && x.RoleId == roleFunction.RoleId)
                                .SingleOrDefault();

                            if (existingRoleFunction != null)
                                _context.Entry(existingRoleFunction).CurrentValues.SetValues(roleFunction);
                            else
                            {
                                var newRoleFunction = new RoleFunction()
                                {
                                    FunctionId = roleFunction.FunctionId,
                                    IsReadOnly = roleFunction.IsReadOnly,
                                    RoleId = roleFunction.RoleId
                                };
                                existingRole.RoleFunction.Add(newRoleFunction);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
