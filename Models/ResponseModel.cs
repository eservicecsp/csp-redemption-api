using CSP_Redemption_WebApi.Entities.Models;
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

    public class IsExistResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsExist { get; set; }
        public int ConsumerId { get; set; }
        public string StatusTypeCode { get; set; }
        public int CampaignType { get; set; }
        public int[] Pieces { get; set; }
    }

    public class RedemptionResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int ConsumerId { get; set; }
        public string StatusTypeCode { get; set; }
        public int CampaignType { get; set; }
        public int[] Pieces { get; set; }
    }

    #region Consumer
    public class ConsumersResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ConsumerModel> Consumers { get; set; }
    }

    public class ConsumerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TumbolCode { get; set; }
        public string AmphurCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ZipCode { get; set; }
        public int ConsumerSourceId { get; set; }
        public int BrandId { get; set; }
        public int? CampaignId { get; set; }
        public int? Point { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ConsumersByPaginationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int length { get; set; }
        public List<Consumer> data { get; set; }
    }

    #endregion


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
        public List<NavigationModel> navigations { get; set; }
    }

    public class NavigationModel
    {
        public string id { get; set; }
        public string title { get; set; }
        //public string translate { get; set; }
        public string type { get; set; }
        public List<ChildModel> children { get; set; }
    }

    public class ChildModel
    {
        public string id { get; set; }
        public string title { get; set; }
        //public string translate { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public List<SubChild> children { get; set; }
        public string url { get; set; }
        public BadgeModel badge { get; set; }
    }

    public class BadgeModel
    {
        public string title { get; set; }
        //public string translate { get; set; }
        public string bg { get; set; }
        public string fg { get; set; }
    }

    public class SubChild
    {
        public string id { get; set; }
        public string title { get; set; }
        //public string translate { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public BadgeModel badge { get; set; }
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


    #region Function

    public class FunctionsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<FunctionModel> Functions { get; set; }
    }

    public class FunctionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int ParentId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool IsInternal { get; set; }
        public bool IsExternal { get; set; }
        public bool IsActived { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    #endregion
}
