using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Models
{
    #region Consumer
    public class CheckExistConsumerRequestModel
    {
        public string Phone { get; set; }
        public int CampaignId { get; set; }
        public int ConsumerId { get; set; }
        public string Token { get; set; }
        public int Point { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public int TransactionTypeId { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ConsumerRequestModel
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
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public int ConsumerSourceId { get; set; }
        public int BrandId { get; set; }
        public int CampaignId { get; set; }
        public int? Point { get; set; }
        public string Token { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    #endregion
}
