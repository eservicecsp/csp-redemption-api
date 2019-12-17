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
        Task<ResponseModel> Register(BrandRegisterRequestModel model);
    }
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            this._brandRepository = brandRepository;
        }
        public async Task<ResponseModel> Register(BrandRegisterRequestModel model)
        {
            var response = new ResponseModel();
            try
            {
                bool isExist = await this._brandRepository.GetBrandByCodeAsync(model.Brand.Code.ToUpper());
                if (isExist)
                {
                    response.IsSuccess = false;
                    response.Message = "Code is duplicated.";
                }
                else
                {
                    var brand = new Brand()
                    {
                        Code = model.Brand.Code.ToUpper(),
                        Name = model.Brand.Name,
                        IsOwner = false
                    };
                    string newPassword = Helpers.Argon2Helper.HashPassword(model.Staff.Email, model.Staff.Password) ;
                    var staff = new Staff()
                    {
                        FirstName = model.Staff.FirstName,
                        LastName = model.Staff.LastName,
                        Email = model.Staff.Email,
                        Password = newPassword,
                        Phone = model.Staff.Phone,
                        IsActived = true,
                        CreatedDate = DateTime.Now,
                    };

                    response.IsSuccess = await this._brandRepository.CreateAsync(brand, staff);
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            
            
            return response;
        }
    }
}
