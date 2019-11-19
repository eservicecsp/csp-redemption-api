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
        public int CompanyId { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public CompanyModel Company { get; set; }
        public RoleModel Role { get; set; }
    }

    public class AuthorizationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Navigation> Navigations { get; set; }
    }

    public class Navigation
    {
        public string id { get; set; }
        public string title { get; set; }
        //public string translate { get; set; }
        public string type { get; set; }
        public List<Child> children { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string title { get; set; }
        //public string translate { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public List<SubChild> children { get; set; }
        public string url { get; set; }
        public Badge badge { get; set; }
    }

    public class Badge
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
        public Badge badge { get; set; }
    }
    #endregion

    #region Company
    public class CompanyModel
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
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion
}
