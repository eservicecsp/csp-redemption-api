using CSP_Redemption_WebApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IStaffService
    {
        Task<AuthenticationResponseModel> Authenticate();
    }

    public class StaffService: IStaffService
    {
        private readonly IConfiguration configuration;

        public StaffService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<AuthenticationResponseModel> Authenticate()
        {
            var response = new AuthenticationResponseModel();

            try
            {
                var requestPassword = Helpers.Argon2Helper.HashPassword("","");
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
