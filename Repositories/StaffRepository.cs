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
        Task<List<Staff>> GetStaffsByBrandIdAsync(int brandId);
        Task<Staff> GetStaffByEmailAsync(string email);
        Task<Staff> GetStaffByIdAsync(int id);
        Task<bool> CreateAsync(Staff staff);
        Task<Staff> GetStaffByEmailAndBrandIdAsync(string email, int brandId);
        Task<bool> UpdateAsync(Staff staff);
    }

    public class StaffRepository: IStaffRepository
    {
        public async Task<List<Staff>> GetStaffsByBrandIdAsync(int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Brand)
                    .Include(x => x.Role)
                    .Where(x => x.BrandId == brandId).ToListAsync();
            }
        }

        public async Task<Staff> GetStaffByEmailAsync(string email)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Brand)
                    .Where(x => x.Email == email).FirstOrDefaultAsync();
            }
        }
        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff
                    .Include(x => x.Brand)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> CreateAsync(Staff staff)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Context.Staff.Add(staff);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        public async Task<Staff> GetStaffByEmailAndBrandIdAsync(string email, int brandId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Staff.Where(x => x.Email == email && x.BrandId == brandId).FirstOrDefaultAsync();
            }
        }
        public async Task<bool> UpdateAsync(Staff staff)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                Staff thisRow = await Context.Staff.SingleAsync(x => x.Id == staff.Id);
                thisRow.Id = thisRow.Id;
                thisRow.FirstName = staff.FirstName;
                thisRow.LastName = staff.LastName;
                thisRow.Email = thisRow.Email;
                if(!string.IsNullOrEmpty(staff.Password))
                    thisRow.Password = staff.Password;
                thisRow.Phone = staff.Phone;
                thisRow.BrandId = thisRow.BrandId;
                thisRow.RoleId = staff.RoleId;
                thisRow.IsActived = staff.IsActived;
                thisRow.CreatedBy = thisRow.CreatedBy;
                thisRow.CreatedDate = thisRow.CreatedDate;
                thisRow.ModifiedBy = staff.ModifiedBy;
                thisRow.ModifiedDate = staff.ModifiedDate;
                thisRow.ResetPasswordToken = staff.ResetPasswordToken;
                Context.Entry(thisRow).CurrentValues.SetValues(thisRow);
                return await Context.SaveChangesAsync() > 0;

            }
        }
    }
}
