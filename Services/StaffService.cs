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
        Task<StaffsResponseModel> GetStaffsByBrandIdAsync(int brandId);
    }

    public class StaffService : IStaffService
    {
        private readonly IConfiguration configuration;
        private readonly IStaffRepository staffRepository;
        private readonly IFunctionRepository functionRepository;
        private readonly IRoleFunctionRepository roleFunctionRepository;

        public StaffService(
            IConfiguration configuration,
            IStaffRepository staffRepository,
            IFunctionRepository functionRepository,
            IRoleFunctionRepository roleFunctionRepository
            )
        {
            this.configuration = configuration;
            this.staffRepository = staffRepository;
            this.functionRepository = functionRepository;
            this.roleFunctionRepository = roleFunctionRepository;
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
                    dbStaff.Brand.Id,
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

                var functions = await this.functionRepository.GetFunctionsByRoleIdAsync(staff.RoleId);
                var parentFunctions = functions.Where(x => x.ParentId == 0).ToList();

                var navigations = new List<NavigationModel>();
                foreach (var function in functions.Where(x => x.ParentId != 0).OrderBy(x => x.ParentId))
                {
                    if (!(navigations.Any(x => x.id == function.ParentId.ToString())))
                    {
                        try
                        {
                            var children = new List<ChildModel>();
                            var existParent = parentFunctions.Where(x => x.Id == function.ParentId).Single();

                            children.Add(new ChildModel()
                            {
                                id = function.Id.ToString(),
                                title = function.Name,
                                type = "item",
                                url = function.Path,
                                icon = function.Icon,
                                children = new List<SubChild>()
                            });
                            var navigation = new NavigationModel()
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
                                var existParent = item.children.Where(x => x.id == function.ParentId.ToString()).FirstOrDefault();
                                if (existParent != null)
                                {
                                    var subChild = new SubChild()
                                    {
                                        id = function.Id.ToString(),
                                        title = function.Name,
                                        type = "item",
                                        url = function.Path,
                                        icon = function.Icon
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
                            var existNavigation = navigations.Where(x => x.id == function.ParentId.ToString()).Single();
                            var child = new ChildModel()
                            {
                                id = function.Id.ToString(),
                                title = function.Name,
                                type = "item",
                                url = function.Path,
                                icon = function.Icon
                            };
                            existNavigation.children.Add(child);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                authorization.navigations = navigations;
                authorization.IsSuccess = true;
            }
            catch (Exception ex)
            {
                authorization.Message = ex.Message;
                throw;
            }
            return authorization;
        }

        public async Task<StaffsResponseModel> GetStaffsByBrandIdAsync(int brandId)
        {
            var response = new StaffsResponseModel();
            try
            {
                var dbStaffs = await this.staffRepository.GetStaffsByBrandIdAsync(brandId);
                response.Staffs = new List<StaffModel>();
                foreach (var dbStaff in dbStaffs)
                {
                    response.Staffs.Add(new StaffModel()
                    {
                        Id = dbStaff.Id,
                        BrandId = dbStaff.BrandId,
                        Phone = dbStaff.Phone,
                        CreatedDate = dbStaff.CreatedDate,
                        CreatedBy = dbStaff.CreatedBy,
                        FirstName = dbStaff.FirstName,
                        LastName = dbStaff.LastName,
                        Email = dbStaff.Email,
                        IsActived = dbStaff.IsActived,
                        RoleId = dbStaff.RoleId,
                        Brand = new BrandModel()
                        {
                            Id = dbStaff.Brand.Id,
                            Name = dbStaff.Brand.Name
                        },
                        Role = new RoleModel()
                        {
                            Id = dbStaff.Role.Id,
                            Description = dbStaff.Role.Description,
                            BrandId = dbStaff.Role.Id,
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
