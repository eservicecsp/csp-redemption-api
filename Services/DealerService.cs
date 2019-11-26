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
    }
}
