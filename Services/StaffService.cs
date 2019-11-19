using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IStaffService
    {
        Task<AuthenticationResponseModel> Authenticate(Staff staff);
        Task<AuthorizationResponseModel> Authorize(string token);
        Task<StaffsResponseModel> GetStaffsByCompanyIdAsync(int companyId);
    }

    public class StaffService: IStaffService
    {
        private readonly IConfiguration configuration;
        private readonly IStaffRepository staffRepository;
        private readonly IMenuRepository menuRepository;

        public StaffService(
            IConfiguration configuration,
            IStaffRepository staffRepository,
            IMenuRepository menuRepository
            )
        {
            this.configuration = configuration;
            this.staffRepository = staffRepository;
            this.menuRepository = menuRepository;
        }

        public async Task<AuthenticationResponseModel> Authenticate(Staff staff)
        {
            var response = new AuthenticationResponseModel();

            try
            {
                var requestPassword = Helpers.Argon2Helper.HashPassword(staff.Email, staff.Password);
                var dbStaff = await this.staffRepository.GetStaffByEmailAsync(staff.Email);
                if (dbStaff == null || requestPassword != dbStaff.Password)
                {
                    return response;
                }
                else
                {
                    string staffToken = Helpers.JwtHelper.GetToken(this.configuration["JWTAuthentication:Key"],
                    Convert.ToInt32(this.configuration["JWTAuthentication:ExpiredInMinutes"]),
                    this.configuration["JWTAuthentication:Issuer"],
                    this.configuration["JWTAuthentication:Audience"],
                    dbStaff.Id,
                    dbStaff.FirstName,
                    dbStaff.LastName,
                    dbStaff.Email,
                    dbStaff.Company.Id,
                    "assets/images/avatars/profile.jpg");

                    response.IsSuccess = true;
                    response.Token = staffToken;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<AuthorizationResponseModel> Authorize(string token)
        {
            AuthorizationResponseModel authorization = new AuthorizationResponseModel();
            try
            {
                var staffId = Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId");

                var staff = await this.staffRepository.GetStaffByIdAsync(Convert.ToInt32(staffId));

                var menus = await this.menuRepository.GetMenusByRoleIdAsync(staff.RoleId);
                var parentMenus = await this.menuRepository.GetMenusByParentIdAsync(0);

                var navigations = new List<Navigation>();

                foreach (var menu in menus.Where(x => x.ParentId != 0).OrderBy(x => x.ParentId))
                {
                    if (!(navigations.Any(x => x.id == menu.ParentId.ToString())))
                    {
                        try
                        {
                            var children = new List<Child>();
                            var existParent = parentMenus.Where(x => x.Id == menu.ParentId).Single();

                            children.Add(new Child()
                            {
                                id = menu.Id.ToString(),
                                title = menu.Name,
                                type = "item",
                                url = menu.Path,
                                icon = menu.Icon,
                                children = new List<SubChild>()
                            });
                            var navigation = new Navigation()
                            {
                                id = existParent.Id.ToString(),
                                title = existParent.Name,
                                type = "group",
                                children = children,
                            };
                            navigations.Add(navigation);
                        }
                        catch (Exception)
                        {
                            foreach (var item in navigations)
                            {
                                var existParent = item.children.Where(x => x.id == menu.ParentId.ToString()).FirstOrDefault();
                                if (existParent != null)
                                {
                                    var subChild = new SubChild()
                                    {
                                        id = menu.Id.ToString(),
                                        title = menu.Name,
                                        type = "item",
                                        url = menu.Path,
                                        icon = menu.Icon
                                    };
                                    existParent.children.Add(subChild);
                                    existParent.type = "collapsable";
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var existNavigation = navigations.Where(x => x.id == menu.ParentId.ToString()).Single();
                            var child = new Child()
                            {
                                id = menu.Id.ToString(),
                                title = menu.Name,
                                type = "item",
                                url = menu.Path,
                                icon = menu.Icon
                            };
                            existNavigation.children.Add(child);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    authorization.IsSuccess = true;
                }
                authorization.Navigations = navigations;
            }
            catch (Exception ex)
            {
                authorization.Message = ex.Message;
                throw;
            }
            return authorization;
        }

        public async Task<StaffsResponseModel> GetStaffsByCompanyIdAsync(int companyId)
        {
            var response = new StaffsResponseModel();
            try
            {
                var dbStaffs = await this.staffRepository.GetStaffsByCompanyIdAsync(companyId);
                response.Staffs = new List<StaffModel>();
                foreach (var dbStaff in dbStaffs)
                {
                    response.Staffs.Add(new StaffModel()
                    {
                        Id = dbStaff.Id,
                        CompanyId = dbStaff.CompanyId,
                        Phone = dbStaff.Phone,
                        CreatedDate = dbStaff.CreatedDate,
                        CreatedBy = dbStaff.CreatedBy,
                        FirstName = dbStaff.FirstName,
                        LastName = dbStaff.LastName,
                        Email = dbStaff.Email,
                        IsActived = dbStaff.IsActived,
                        RoleId = dbStaff.RoleId,
                        Company = new CompanyModel()
                        {
                            Id = dbStaff.Company.Id,
                            Name = dbStaff.Company.Name
                        },
                        Role = new RoleModel()
                        {
                            Id = dbStaff.Role.Id,
                            Description = dbStaff.Role.Description,
                            CompanyId = dbStaff.Role.Id,
                            Name = dbStaff.Role.Name
                        }
                    });
                };
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                throw;
            }
            return response;
        }
    }
}
