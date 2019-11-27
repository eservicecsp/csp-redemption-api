using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IAddressService
    {
        Task<ZoneResponseModel> GetZones();
        Task<ProvincesResponseModel> GetProvincesByZoneId(int zoneId);
        Task<AmphurResponseModel> GetAmphursByProvinceCode(string provinceCode);
        Task<TumbolResponseModel> GetTumbolsByAmphurCode(string amphurCode);
        Task<ProvincesResponseModel> GetProvincesAsync();
    }
    public class AddressService : IAddressService
    {
        private readonly IZoneRepository zoneRepository;
        private readonly IProvinceRepository provinceRepository;
        private readonly IAmphurRepository amphurRepository;
        private readonly ITumbolRepository tumbolRepository;

        public AddressService
            (
            IZoneRepository zoneRepository,
            IProvinceRepository provinceRepository,
            IAmphurRepository amphurRepository,
            ITumbolRepository tumbolRepository
            )
        {
            this.zoneRepository = zoneRepository;
            this.provinceRepository = provinceRepository;
            this.amphurRepository = amphurRepository;
            this.tumbolRepository = tumbolRepository;
        }

        public async Task<ZoneResponseModel> GetZones()
        {
            var response = new ZoneResponseModel();
            try
            {
                var zones = new List<ZoneModel>();
                var dbZone = await this.zoneRepository.GetZoneAsync();
                foreach (var item in dbZone)
                {
                    zones.Add(new ZoneModel()
                    {
                        Id = item.Id,
                        NameTh = item.NameTh,
                        NameEn = item.NameEn,
                        Description = item.Description
                    });
                }
                response.IsSuccess = true;
                response.zones = zones;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ProvincesResponseModel> GetProvincesByZoneId(int zoneId)
        {
            var response = new ProvincesResponseModel();
            try
            {
                var provinces = new List<ProvinceModel>();
                var dbProvinces = await this.provinceRepository.GetProvincesByZoneAsync(zoneId);
                foreach (var item in dbProvinces)
                {
                    provinces.Add(new ProvinceModel()
                    {
                        Code = item.Code,
                        NameTh = item.NameTh,
                        NameEn = item.NameEn,
                        ZoneId = item.ZoneId
                    });
                }
                response.IsSuccess = true;
                response.Provinces = provinces;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<AmphurResponseModel> GetAmphursByProvinceCode(string provinceCode)
        {
            var response = new AmphurResponseModel();
            try
            {
                var amphurs = new List<AmphurModel>();
                var dbAmphurs = await this.amphurRepository.GetAmphursByProvinceCodeAsync(provinceCode);
                foreach(var item in dbAmphurs)
                {
                    amphurs.Add(new AmphurModel() { 
                    Code = item.Code,
                    NameTh = item.NameTh,
                    NameEn = item.NameEn,
                    ProvinceCode = item.ProvinceCode
                    });
                }
                response.IsSuccess = true;
                response.amphurs = amphurs;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<TumbolResponseModel> GetTumbolsByAmphurCode(string amphurCode)
        {
            var response = new TumbolResponseModel();
            try
            {
                var tumbols = new List<TumbolModel>();
                var dbTumbols = await this.tumbolRepository.GetTumbolsByAmphurCodeAsync(amphurCode);
                foreach (var item in dbTumbols)
                {
                    tumbols.Add(new TumbolModel() { 
                        Code = item.Code,
                        ZipCode = item.ZipCode,
                        NameTh =item.NameTh,
                        NameEn = item.NameEn,
                        AmphurCode = item.AmphurCode
                    });
                }
                response.IsSuccess = true;
                response.tumbols = tumbols;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            
            return response;
        }

        public async Task<ProvincesResponseModel> GetProvincesAsync()
        {
            var response = new ProvincesResponseModel();
            try
            {
                var provinces = await this.provinceRepository.GetProvincesAsync();
                response.Provinces = new List<ProvinceModel>();
                foreach(var province in provinces)
                {
                    response.Provinces.Add(new ProvinceModel()
                    {
                        ZoneId = province.ZoneId,
                        Code = province.Code,
                        NameEn = province.NameEn,
                        NameTh = province.NameTh,
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
