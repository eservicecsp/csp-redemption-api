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
        Task<RolesResponseModel> GetRolesAsync(int brandId);
        Task<RoleResponseModel> GetRoleAsync(int id);
        Task<ResponseModel> CreateAsync(RoleModel model);
        Task<ResponseModel> UpdateAsync(RoleModel model);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<RolesResponseModel> GetRolesAsync(int brandId)
        {
            var response = new RolesResponseModel();

            try
            {
                var roles = await this.roleRepository.GetRolesAsync(brandId);
                response.roles = new List<RoleModel>();
                foreach (var role in roles)
                {
                    var functions = new List<FunctionModel>();
                    foreach(var function in role.RoleFunction)
                    {
                        functions.Add(new FunctionModel()
                        {
                            Id = function.Function.Id,
                            Name = function.Function.Name
                        });
                    }

                    response.roles.Add(new RoleModel()
                    {
                        Id = role.Id,
                        Description = role.Description,
                        BrandId = role.BrandId,
                        Name = role.Name,
                        Functions = functions
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

        public async Task<RoleResponseModel> GetRoleAsync(int id)
        {
            var response = new RoleResponseModel();
            try
            {
                var role = await this.roleRepository.GetRoleAsync(id);
                if (role == null)
                {
                    response.Message = "Role not found.";
                    return response;
                }
                else
                {
                    var functions = new List<FunctionModel>();
                    foreach (var function in role.RoleFunction)
                    {
                        functions.Add(new FunctionModel()
                        {
                            Id = function.Function.Id,
                            Name = function.Function.Name,
                            Level = function.Function.Level,
                            Description = function.Function.Description,
                        });
                    }

                    response.Role = new RoleModel()
                    {
                        Id = role.Id,
                        Description = role.Description,
                        BrandId = role.BrandId,
                        Name = role.Name,
                        Functions = functions
                    };
                    
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> CreateAsync(RoleModel model)
        {
            var response = new ResponseModel();
            try
            {
                var role = new Role()
                {
                    Id = 0,
                    BrandId = model.BrandId,
                    Description = model.Description,
                    Name = model.Name
                };

                if (await this.roleRepository.CreateAsync(role))
                {
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAsync(RoleModel model)
        {
            var response = new ResponseModel();
            try
            {
                var role = await this.roleRepository.GetRoleAsync(model.Id);
                if (role == null)
                {
                    response.Message = "Role not found";
                }
                else
                {
                    role.Name = model.Name;
                    role.Description = model.Description;
                    role.RoleFunction = new List<RoleFunction>();
                    foreach(var function in model.Functions)
                    {
                        role.RoleFunction.Add(new RoleFunction()
                        {
                            RoleId = role.Id,
                            FunctionId = function.Id,
                            IsReadOnly = false,
                        });
                    }

                    if (await this.roleRepository.UpdateAsync(role))
                    {
                        response.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
