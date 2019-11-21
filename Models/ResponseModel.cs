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
}
