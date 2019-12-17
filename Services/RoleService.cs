using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IRoleService
    {
        Task<RoleResponseModel> GetRoleIdByBrandIsAsync(int brandId);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<RoleResponseModel> GetRoleIdByBrandIsAsync(int brandId)
        {
            var response = new RoleResponseModel();

            try
            {
                response.roles = await this.roleRepository.GetRoleIdByBrandIsAsync(brandId);
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
