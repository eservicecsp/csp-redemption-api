using CSP_Redemption_WebApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Models
{

    public class ImportDataBinding
    {
        public string file { get; set; }
        public string fileName { get; set; }
        public int brandId { get; set; }
        public int CampaignId { get; set; }
        public int createBy { get; set; }
    }

    #region Brand
    public class BrandRegisterRequestModel
    {
        public BrandModel Brand { get; set; }
        public StaffModel Staff { get; set; }
    }

    #endregion

    #region Consumer
    public class CheckExistConsumerRequestModel
    {
        public string Phone { get; set; }
        public int CampaignId { get; set; }
        public int ConsumerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
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
        public DateTime? BirthDate { get; set; }
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
        public string Code { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSkincare { get; set; }
        public bool IsMakeup { get; set; }
        public bool IsBodycare { get; set; }
        public bool IsSupplements { get; set; }
        public List<string> ProductType { get; set; }
    }

    public class PaginationModel
    {
        public int length { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        //public int previousPageIndex { get; set; }
        public string filter { get; set; }
        public FiltersModel filters { get; set; }
        public string sortActive { get; set; }
        public string sortDirection { get; set; }
        public int BrandId { get; set; }
        public int campaignId { get; set; }
    }

    public class FiltersModel
    {
        public int startAge { get; set; }
        public int endAge { get; set; }
        public int birthOfMonth { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool isSkincare { get; set; }
        public bool isMakeup { get; set; }
        public bool isBodycare { get; set; }
        public bool isSupplements { get; set; }
        public List<int> productTypes { get; set; }

    }

    public class CreateCampaignRequestModel
    {
        public CampaignModel Campaign { get; set; }
        public List<int> Peices { get; set; }
        public int Point { get; set; }
        public int Product { get; set; }
    }

    //public class CampaignModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public int CampaignTypeId { get; set; }
    //    public int BrandId { get; set; }
    //    public int? DealerId { get; set; }
    //    public string Url { get; set; }
    //    public int Quantity { get; set; }
    //    public int? TotalPeice { get; set; }
    //    public int? Waste { get; set; }
    //    public int? GrandTotal { get; set; }
    //    public DateTime? StartDate { get; set; }
    //    public DateTime? EndDate { get; set; }
    //    public string AlertMessage { get; set; }
    //    public string DuplicateMessage { get; set; }
    //    public string QrCodeNotExistMessage { get; set; }
    //    public string WinMessage { get; set; }
    //    public int? Rows { get; set; }
    //    public int? Columns { get; set; }
    //    public string CollectingType { get; set; }
    //    public int CreatedBy { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public List<Dealer> dealers { get; set; }

    //    public List<Collection> CollectingData { get; set; }
    //}


    #endregion

    public class CollectionModel
    {
        public int Id { get; set; }
  
        public int Quantity { get; set; }
        public int WasteQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string file { get; set; }
        public string extension { get; set; }

    }
}
