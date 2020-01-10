using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IBrandService
    {
        Task<BrandsResponseModel> GetBrandsAsync();
        Task<BrandResponseModel> GetBrandAsync(int id);
        Task<ResponseModel> Register(BrandRegisterRequestModel model);
        Task<ResponseModel> UpdateAsync(BrandRegisterRequestModel model);
    }
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IEmailService _emailService;

        public BrandService(IBrandRepository brandRepository, IStaffRepository staffRepository, IEmailService emailService)
        {
            _brandRepository = brandRepository;
            _staffRepository = staffRepository;
            _emailService = emailService;
        }

        public async Task<BrandsResponseModel> GetBrandsAsync()
        {
            var response = new BrandsResponseModel();
            try
            {
                var brands = await _brandRepository.GetBrandsAsync();

                response.Brands = new List<BrandModel>();

                foreach (var brand in brands)
                {
                    response.Brands.Add(new BrandModel()
                    {
                        Id = brand.Id,
                        Code = brand.Code,
                        Name = brand.Name,
                        IsOwner = brand.IsOwner
                    });
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BrandResponseModel> GetBrandAsync(int id)
        {
            var response = new BrandResponseModel();
            try
            {
                var brand = await _brandRepository.GetBrandAsync(id);
                var staffs = await _staffRepository.GetStaffsByBrandIdAsync(brand.Id);
                var admin = staffs.Where(x => x.Role.Name == "Administrator").FirstOrDefault();
                if(admin == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Brand administrator not  found.";
                    return response;
                }

                response.Brand = new BrandModel()
                {
                    Id = brand.Id,
                    Code = brand.Code,
                    IsOwner = brand.IsOwner,
                    Name = brand.Name,
                    Staff = new StaffModel()
                    {
                        Id = admin.Id,
                        CreatedDate = admin.CreatedDate,
                        BrandId = admin.BrandId,
                        CreatedBy = admin.CreatedBy,
                        Email = admin.Email,
                        FirstName = admin.FirstName,
                        IsActived = admin.IsActived,
                        LastName = admin.LastName,
                        Password = admin.Password,
                        Phone = admin.Phone,
                        RoleId = admin.RoleId
                    }
                };
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> Register(BrandRegisterRequestModel model)
        {
            var response = new ResponseModel();
            try
            {
                bool isExist = await _brandRepository.GetBrandByCodeAsync(model.Brand.Code.ToUpper());
                Staff isExistStaff = await this._staffRepository.GetStaffByEmailAsync(model.Staff.Email);
                if (isExist)
                {
                    response.IsSuccess = false;
                    response.Message = "Code is duplicated.";
                }
                else if(isExistStaff != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Email is duplicated.";
                }
                else
                {
                    var brand = new Brand()
                    {
                        Code = model.Brand.Code.ToUpper(),
                        Name = model.Brand.Name,
                        IsOwner = false
                    };
                    string password =  DateTime.Now.ToString("yyyyMMddHHmmss");
                    //string newPassword = Helpers.Argon2Helper.HashPassword(model.Staff.Email, model.Staff.Password); 
                    string hashedPassword = Helpers.Argon2Helper.HashPassword(model.Staff.Email, password);
                    var staff = new Staff()
                    {
                        FirstName = model.Staff.FirstName,
                        LastName = model.Staff.LastName,
                        Email = model.Staff.Email,
                        Password = hashedPassword,
                        Phone = model.Staff.Phone,
                        IsActived = true,
                        CreatedDate = DateTime.Now,
                        ResetPasswordToken = Guid.NewGuid().ToString()
                    };

                    response.IsSuccess = await this._brandRepository.CreateAsync(brand, staff);

                    // send email to brand administrator
                    var toEmails = new List<string>();
                    toEmails.Add(staff.Email);

                    var parameters = new Dictionary<string, string>();
                    parameters.Add("name", $"{staff.FirstName} {staff.LastName}");
                    parameters.Add("email", $"{staff.Email}");
                    parameters.Add("password", $"{password}");
                    parameters.Add("url", $"http://localhost:4200/pages/auth/reset-password?email={model.Staff.Email}&token={staff.ResetPasswordToken}");

                    var email = new EmailModel()
                    {
                        To = toEmails,
                        Template = @"
Dear [#name#],

Your registration has been completed.

You registered with this email: [#email#]

You registered with this password: [#password#]

Please consider to change passsword at this [#url#]

Kind Regards,
Karmart Redemption
",
                        Subject = "Register - Karmart Redemption",
                        Parameters = parameters
                    };
                    
                    var emailResponse = _emailService.SendEmail(email);

                    response.Message = password;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ResponseModel> UpdateAsync(BrandRegisterRequestModel model)
        {
            var response = new ResponseModel();
            try
            {
                var brand = new Brand()
                {
                    Id = model.Brand.Id,
                    Name = model.Brand.Name,
                };
                var staff = new Staff()
                {
                    Id = model.Staff.Id,
                    FirstName = model.Staff.FirstName,
                    LastName = model.Staff.LastName,
                    Phone = model.Staff.Phone,
                    ModifiedBy = model.Staff.CreatedBy,
                    ModifiedDate = DateTime.Now,
                    IsActived = model.Staff.IsActived
                };

                response.IsSuccess = await this._brandRepository.UpdateAsync(brand, staff);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }


            return response;
        }
    }
}
