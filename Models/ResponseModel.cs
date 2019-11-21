using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    #region Staff
    public class AuthenticationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class StaffsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<StaffModel> Staffs { get; set; }
    }

    public class StaffModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public int BrandId { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public BrandModel Brand { get; set; }
        public RoleModel Role { get; set; }
    }

    public class AuthorizationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<AuthorizationModel> RoleMenus { get; set; }
    }

    public class AuthorizationModel
    {
        public int Id { get; set; }
        public bool IsReadOnly { get; set; }
    }

    #endregion

    #region Brand
    public class BrandModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActived { get; set; }
    }
    #endregion

    #region Role
    public class RoleModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion

    #region Address
    public class ZoneResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ZoneModel> zones { get; set; }
    }

    public class ZoneModel
    {
   
        public int Id { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
    }

    public class ProvinceResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ProvinceModel> provinces { get; set; }
    }

    public class ProvinceModel
    {

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public int ZoneId { get; set; }
    }

    public class AmphurResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<AmphurModel> amphurs { get; set; }
    }

    public class AmphurModel
    {

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public string ProvinceCode { get; set; }
    }
    public class TumbolResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<TumbolModel> tumbols { get; set; }
    }

    public class TumbolModel
    {

        public string Code { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public string AmphurCode { get; set; }
    }
    #endregion
}
