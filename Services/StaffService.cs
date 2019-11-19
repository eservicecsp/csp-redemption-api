﻿using CSP_Redemption_WebApi.Entities.Models;
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
        Task<StaffsResponseModel> GetStaffsByCompanyIdAsync(int companyId);
    }

    public class StaffService: IStaffService
    {
        private readonly IConfiguration configuration;
        private readonly IStaffRepository staffRepository;

        public StaffService(
            IConfiguration configuration,
            IStaffRepository staffRepository
            )
        {
            this.configuration = configuration;
            this.staffRepository = staffRepository;
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
                    dbStaff.Company.Id,
                    "assets/images/avatars/profile.jpg");

                    response.IsSuccess = true;
                    response.Token = staffToken;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<StaffsResponseModel> GetStaffsByCompanyIdAsync(int companyId)
        {
            var response = new StaffsResponseModel();
            try
            {
                var dbStaffs = await this.staffRepository.GetStaffsByCompanyIdAsync(companyId);
                response.Staffs = new List<StaffModel>();
                foreach (var dbStaff in dbStaffs)
                {
                    response.Staffs.Add(new StaffModel()
                    {
                        Id = dbStaff.Id,
                        CompanyId = dbStaff.CompanyId,
                        Phone = dbStaff.Phone,
                        CreatedDate = dbStaff.CreatedDate,
                        CreatedBy = dbStaff.CreatedBy,
                        FirstName = dbStaff.FirstName,
                        LastName = dbStaff.LastName,
                        Email = dbStaff.Email,
                        IsActived = dbStaff.IsActived,
                        RoleId = dbStaff.RoleId,
                        Company = new CompanyModel()
                        {
                            Id = dbStaff.Company.Id,
                            Name = dbStaff.Company.Name
                        },
                        Role = new RoleModel()
                        {
                            Id = dbStaff.Role.Id,
                            Description = dbStaff.Role.Description,
                            CompanyId = dbStaff.Role.Id,
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
    }
}
