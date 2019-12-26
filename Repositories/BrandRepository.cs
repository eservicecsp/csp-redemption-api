using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IBrandRepository
    {
        Task<bool> CreateAsync(Brand brand, Staff staff);
        Task<bool> GetBrandByCodeAsync(string code);
        Task<Brand> GetBrandAsync(int id);
    }
    public class BrandRepository : IBrandRepository
    {
        private readonly CSP_RedemptionContext _context;
        public BrandRepository(CSP_RedemptionContext context)
        {
            this._context = context;
        }

        public async Task<bool> CreateAsync(Brand brand, Staff staff)
        {
            bool isSuccess = false;

            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {

                        await Context.Brand.AddAsync(brand);
                        if (await Context.SaveChangesAsync() > 0)
                        {
                            staff.BrandId = brand.Id;
                            staff.RoleId = 1;
                            await Context.Staff.AddAsync(staff);
                            await Context.SaveChangesAsync();

                            var role = new Role()
                            {
                                Name = "Administrator",
                                Description = "System Administrator",
                                BrandId = brand.Id
                            };
                            await Context.Role.AddAsync(role);
                            await Context.SaveChangesAsync();

                            var functions = await Context.Function.Where(x => x.IsExternal == true).ToListAsync();
                            var funcRole = new List<RoleFunction>();
                            foreach (var item in functions)
                            {
                                funcRole.Add(new RoleFunction()
                                {
                                    RoleId = role.Id,
                                    FunctionId = item.Id,
                                    IsReadOnly = false
                                });
                            };
                            await Context.RoleFunction.AddRangeAsync(funcRole);
                            isSuccess = await Context.SaveChangesAsync() > 0;
                            transaction.Commit();
                        }
                        else
                        {

                            return isSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                    }
                }
            }
            return isSuccess;
        }

        public async Task<bool> GetBrandByCodeAsync(string code)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Brand.AnyAsync(x => x.Code == code.ToUpper());
            }
        }

        public async Task<Brand> GetBrandAsync(int id)
        {
            return await _context.Brand.FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}
