using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IDealerService
    {
        Task<DealersResponseModel> GetDealersByBrandIdAsync(int brandId);
        Task<DealerResponseModel> GetDealersByIdAsync(int id);
        Task<DealerResponseModel> UpdateAsync(Dealer dealer);
        Task<ResponseModel> CreateAsync(Dealer dealer);
    }

    public class DealerService: IDealerService
    {
        private readonly IDealerRepository dealerRepository;
        public DealerService(IDealerRepository dealerRepository)
        {
            this.dealerRepository = dealerRepository;
        }

        public async Task<DealersResponseModel> GetDealersByBrandIdAsync(int brandId)
        {
            var response = new DealersResponseModel();

            try
            {
                var dealers = await this.dealerRepository.GetDealersByBrandIdAsync(brandId);
                response.Dealers = new List<DealerModel>();

                foreach (var dealer in dealers)
                {
                    response.Dealers.Add(new DealerModel()
                    {
                        CreatedDate = dealer.CreatedDate,
                        BrandId = dealer.BrandId,
                        CreatedBy = dealer.CreatedBy,
                        Id = dealer.Id,
                        Name = dealer.Name,
                        Email = dealer.Email,
                        TaxNo = dealer.TaxNo,
                        Phone = dealer.Phone,
                        Tel = dealer.Tel,
                        BranchNo = dealer.BranchNo,
                    });
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DealerResponseModel> GetDealersByIdAsync(int id)
        {
            var response = new DealerResponseModel();
            try
            {
               
                var dealers = await this.dealerRepository.GetDealersByIdAsync(id);
                var dealer = new DealerModel();
                if (dealers != null)
                {
                    dealer.Id = dealers.Id;
                    dealer.BranchNo = dealers.BranchNo;
                    dealer.Name = dealers.Name;
                    dealer.Email = dealers.Email;
                    dealer.TaxNo = dealers.TaxNo;
                    dealer.Phone = dealers.Phone;
                    dealer.Tel = dealers.Tel;
                    dealer.Address1 = dealers.Address1;
                    dealer.ProvinceCode = dealers.ProvinceCode;
                    dealer.AmphurCode = dealers.AmphurCode;
                    dealer.TumbolCode = dealers.TumbolCode;
                    dealer.ZipCode = dealers.ZipCode;
                }

                response.Dealer = dealer;
                response.IsSuccess = true;
           
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<DealerResponseModel> UpdateAsync(Dealer dealer)
        {
            var response = new DealerResponseModel();
            try
            {
                response.IsSuccess = await this.dealerRepository.UpdateAsync(dealer);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> CreateAsync(Dealer dealer)
        {
            var response = new ResponseModel();

            try
            {
                response.IsSuccess = await this.dealerRepository.CreateAsync(dealer);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
