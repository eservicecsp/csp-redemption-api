﻿using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IFunctionService
    {
        Task<FunctionsResponseModel> GetFunctionsAsync(string type, int brandId, bool isActive = false);
    }

    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository functionRepository;
        private readonly IBrandRepository brandRepository;

        public FunctionService(
            IFunctionRepository functionRepository,
            IBrandRepository brandRepository)
        {
            this.functionRepository = functionRepository;
            this.brandRepository = brandRepository;
        }

        public async Task<FunctionsResponseModel> GetFunctionsAsync(string type, int brandId, bool isActive = false)
        {
            var response = new FunctionsResponseModel();
            try
            {
                var functions = await this.functionRepository.GetFunctionsAsync();
                switch (type)
                {
                    case "parent":
                        {
                            functions = functions.Where(x => x.ParentId == 0).ToList();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                var brand = await this.brandRepository.GetBrandAsync(brandId);
                if (!brand.IsOwner)
                    functions = functions.Where(x => x.IsExternal == true && x.IsActived == true).ToList();

                if (isActive)
                {
                    functions = functions.Where(x => x.IsActived == true).ToList();
                }


                response.Functions = new List<FunctionModel>();
                foreach (var function in functions)
                {
                    response.Functions.Add(new FunctionModel()
                    {
                        Id = function.Id,
                        Description = function.Description,
                        ModifiedDate = function.ModifiedDate,
                        Icon = function.Icon,
                        IsActived = function.IsActived,
                        IsExternal = function.IsExternal,
                        IsInternal = function.IsInternal,
                        Level = function.Level,
                        ModifiedBy = function.ModifiedBy,
                        Name = function.Name,
                        ParentId = function.ParentId,
                        Path = function.Path
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
