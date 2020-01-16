using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IContactUsService
    {
        Task<ContactUsModel> GetContactUsAsync(int brandId);
        Task<ResponseModel> CreateAsyn(ContactUs contactUs);
        Task<ResponseModel> UpdateAsyn(ContactUs contactUs);
    }
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository contactUsRepository;

        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            this.contactUsRepository = contactUsRepository;
        }

        public async Task<ContactUsModel> GetContactUsAsync(int brandId)
        {
            var response = new ContactUsModel();
            try
            {
                var contactUs = await this.contactUsRepository.GetContactUsByBrandIdAsync(brandId);
                response.IsSuccess = true;
                response.contactUs = contactUs;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseModel> CreateAsyn(ContactUs contactUs)
        {
            var response = new ResponseModel();
            try
            {
                var data = new ContactUs();
                data.BrandId = contactUs.BrandId;
                data.Tel = contactUs.Tel;
                data.Facebook = contactUs.Facebook;
                data.Line = contactUs.Line;
                data.Web = contactUs.Web;
                data.ShopOnline = contactUs.ShopOnline;
                response.IsSuccess = await this.contactUsRepository.CreateAsync(data);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseModel> UpdateAsyn(ContactUs contactUs)
        {
            var response = new ResponseModel();
            try
            {
                var data = new ContactUs();
                data.Id = contactUs.Id;
                data.BrandId = contactUs.BrandId;
                data.Tel = contactUs.Tel;
                data.Facebook = contactUs.Facebook;
                data.Line = contactUs.Line;
                data.Web = contactUs.Web;
                data.ShopOnline = contactUs.ShopOnline;
                response.IsSuccess = await this.contactUsRepository.UpdateAsync(data);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
