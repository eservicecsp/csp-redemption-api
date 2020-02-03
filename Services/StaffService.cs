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
        Task<ResponseModel> CreateAsync(Staff staff);
        Task<StaffResponseModel> GetStaffsById(int id);
        Task<ResponseModel> UpdateAsync(Staff staff);
        Task<ResetPasswordTokenResponseModel> GetResetPasswordTokenAsync(string email);
        Task<ResponseModel> ResetPasswordAsync(Staff staff);
    }

    public class StaffService : IStaffService
    {
        private readonly IConfiguration configuration;
        private readonly IStaffRepository staffRepository;
        private readonly IFunctionRepository functionRepository;
        private readonly IRoleFunctionRepository roleFunctionRepository;
        private readonly IEmailService _emailService;

        public StaffService(
            IConfiguration configuration,
            IStaffRepository staffRepository,
            IFunctionRepository functionRepository,
            IRoleFunctionRepository roleFunctionRepository,
            IEmailService emailService
            )
        {
            this.configuration = configuration;
            this.staffRepository = staffRepository;
            this.functionRepository = functionRepository;
            this.roleFunctionRepository = roleFunctionRepository;
            _emailService = emailService;
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
                    response.ResetPasswordToken = dbStaff.ResetPasswordToken;
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

                if (string.IsNullOrEmpty(token))
                {
                    authorization.Message = "Unauthorized";
                    return authorization;
                }

                var staffId = Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId");

                var staff = await this.staffRepository.GetStaffByIdAsync(Convert.ToInt32(staffId));

                var functions = await this.functionRepository.GetFunctionsByRoleIdAsync(staff.RoleId);
                var parentFunctions = functions.Where(x => x.ParentId == 0).ToList();

                var navigations = new List<NavigationModel>();
                foreach (var function in functions.Where(x => x.ParentId != 0 && x.IsInternal == true && x.IsExternal == true).OrderBy(x => x.ParentId))
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
                                    if (existParent.children == null)
                                    {
                                        existParent.children = new List<SubChild>();
                                    }
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
                if (staff.Brand.IsOwner)
                {
                    foreach (var function in functions.Where(x => x.ParentId != 0 && x.IsInternal == true && x.IsExternal == false))
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
                                        if (existParent.children == null)
                                        {
                                            existParent.children = new List<SubChild>();
                                        }
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

        public async Task<ResponseModel> CreateAsync(Staff staff)
        {
            var response = new ResponseModel();

            try
            {
                var checkDup = await this.staffRepository.GetStaffByEmailAndBrandIdAsync(staff.Email, staff.BrandId);
                if (checkDup != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Email is duplicated.";
                }
                else
                {
                    response.IsSuccess = await this.staffRepository.CreateAsync(staff);
                }
               
            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<StaffResponseModel> GetStaffsById(int id)
        {
            var response = new StaffResponseModel();
            try
            {
                var staff = await this.staffRepository.GetStaffByIdAsync(id);
                var data = new StaffModel();
                if(staff != null)
                {
                    data.Id = staff.Id;
                    data.FirstName = staff.FirstName;
                    data.LastName = staff.LastName;
                    data.Phone = staff.Phone;
                    data.RoleId = staff.RoleId;
                    data.IsActived = staff.IsActived;
                    data.Email = staff.Email;

                }
                response.Staff = data;
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAsync(Staff staff)
        {
            var response = new ResponseModel();

            try
            {
                response.IsSuccess = await this.staffRepository.UpdateAsync(staff);

            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResetPasswordTokenResponseModel> GetResetPasswordTokenAsync(string email)
        {
            var response = new ResetPasswordTokenResponseModel();
            try
            {
                var staff = await this.staffRepository.GetStaffByEmailAsync(email);
                if(staff == null)
                {
                    response.Message = "Staff not found.";
                    return response;
                }

                staff.ResetPasswordToken = Guid.NewGuid().ToString();
                if (await this.staffRepository.UpdateAsync(staff))
                {
                    response.IsSuccess = true;
                    response.Token = staff.ResetPasswordToken;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> ResetPasswordAsync(Staff staff)
        {
            var response = new ResponseModel();
            try
            {
                var dbStaff = await this.staffRepository.GetStaffByEmailAsync(staff.Email);
                if(dbStaff == null)
                {
                    response.Message = "Staff not found.";
                    return response;
                }

                if(dbStaff.ResetPasswordToken != staff.ResetPasswordToken)
                {
                    response.Message = "Invalid token.";
                    return response;
                }

                dbStaff.Password = Helpers.Argon2Helper.HashPassword(staff.Email, staff.Password);
                dbStaff.ResetPasswordToken = null;
                if (await this.staffRepository.UpdateAsync(dbStaff))
                {
                    response.IsSuccess = true;
                }

                // send email to brand administrator
                var toEmails = new List<string>();
                toEmails.Add(staff.Email);

                var parameters = new Dictionary<string, string>();
                parameters.Add("name", $"{staff.FirstName} {staff.LastName}");
                parameters.Add("password", $"{staff.Password}");

                var email = new EmailModel()
                {
                    To = toEmails,
                    Template = @"
Dear [#name#],

Your password has been changed.

Your new password: [#password#]

If this is not you, please consider to contact system administrator.

Kind Regards,
Karmart Redemption
",
                    Subject = "Register - Karmart Redemption",
                    Parameters = parameters
                };

                var emailResponse = _emailService.SendEmail(email);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
    }
}
