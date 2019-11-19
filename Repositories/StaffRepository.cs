using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IStaffRepository
    {
        Task<List<Staff>> GetStaffsByCompanyIdAsync(int companyId);
        Task<Staff> GetStaffByEmailAsync(string email);
        Task<Staff> GetStaffByIdAsync(int id);
    }

    public class StaffRepository: IStaffRepository
    {
        public async Task<List<Staff>> GetStaffsByCompanyIdAsync(int companyId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Company)
                    .Include(x => x.Role)
                    .Where(x => x.CompanyId == companyId).ToListAsync();
            }
        }

        public async Task<Staff> GetStaffByEmailAsync(string email)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Company)
                    .Where(x => x.Email == email).FirstOrDefaultAsync();
            }
        }
        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Company)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }
    }
}
