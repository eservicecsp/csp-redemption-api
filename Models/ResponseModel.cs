﻿using CSP_Redemption_WebApi.Entities.Models;
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

    public class FileResponseDataBinding
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public byte[] File { get; set; }
    }
    public class IsExistResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsExist { get; set; }
        public int ConsumerId { get; set; }
        public string StatusTypeCode { get; set; }
        public int CampaignType { get; set; }
        public int CollectingType { get; set; }
        public int BrandId { get; set; }
        public List<CollectionModel> CollectingData { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[] Pieces { get; set; }
        public int? TotalPieces { get; set; }
        public ConsumerRequestModel consumer { get; set; }
    }

    public class RedemptionResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int ConsumerId { get; set; }
        public string StatusTypeCode { get; set; }
        public int CampaignType { get; set; }
        public int CollectingType { get; set; }
        public int BrandId { get; set; }
        public List<CollectionModel> CollectingData { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[] Pieces { get; set; }
        public int? TotalPieces { get; set; }
    }

    #region Consumer
    public class ConsumersResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ConsumerModel> Consumers { get; set; }
        public ConsumerModel Consumer { get; set; }
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
    public class ExportConsumerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Tumbol { get; set; }
        public string Amphur { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public int? Point { get; set; }
    }

    public class ConsumersByPaginationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Length { get; set; }
        public List<ConsumerModel> Data { get; set; }
    }

    public class EnrollmentsByPaginationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int length { get; set; }
        public List<Enrollment> data { get; set; }
    }

    #endregion

    #region QrCode
    public class QrCodeResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int length { get; set; }
        public List<QrCodeModel> data { get; set; }
    }
    public class QrCodeModel
    {
        public string Token { get; set; }
        public int? Peice { get; set; }
        public string Code { get; set; }
        public int? ConsumerId { get; set; }
        public int? TransactionId { get; set; }
        public int? Point { get; set; }
        public DateTime? ScanDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullUrl { get; set; }

    }

    #endregion

    #region Transaction

    public class TransactionResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int length { get; set; }
        public List<TransactionModel> data { get; set; }
    }
    public class TransactionModel
    {
        public int Id { get; set; }
        public int? ConsumerId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public int Point { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public string TransactionType { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public int TotalPoint { get; set; }

    }


    #endregion

    #region Campaign

    public class CampaignsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<CampaignModel> Campaigns { get; set; }
        public CampaignModel Campaign { get; set; }
        public List<CampaignPaginationModel> CampaignPagination { get; set; }
        public int Length { get; set; }
    }

    public class CampaignPaginationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CampaignType { get; set; }
        public string CampaignStatusType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalQrCode { get; set; }
        public int TotalFail { get; set; }
        public int TotalEmpty { get; set; }
        public int TotalDuplicate { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalTran { get; set; }

    }
    public class CampaignModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CampaignTypeId { get; set; }
        public int? CampaignStatusId { get; set; }
        public int BrandId { get; set; }
        public int Product { get; set; }
        public int? DealerId { get; set; }
        public string Url { get; set; }
        public int Quantity { get; set; }
        public int? TotalPeice { get; set; }
        public int? Waste { get; set; }
        public int? GrandTotal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AlertMessage { get; set; }
        public string DuplicateMessage { get; set; }
        public string QrCodeNotExistMessage { get; set; }
        public string WinMessage { get; set; }
        public int? Rows { get; set; }
        public int? Columns { get; set; }
        public string CollectingType { get; set; }
        public int CreatedBy { get; set; }
        public string Tel { get; set; }
        public string Facebook { get; set; }
        public string Line { get; set; }
        public string Web { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Dealer> Dealers { get; set; }

        public List<CollectionModel> CollectingData { get; set; }
    }

    #endregion

    #region Campaign Type
    public class CampaignTypesResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<CampaignTypeModel> CampaignTypes { get; set; }
    }

    public class CampaignTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }

    #endregion

    #region Dealer
    public class DealersResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<DealerModel> Dealers { get; set; }
    }
    public class DealerResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public DealerModel Dealer { get; set; }
    }
    public class DealerModel
    {
        public int Id { get; set; }
        public string BranchNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TaxNo { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public int BrandId { get; set; }
        public int CreatedBy { get; set; }
        public string Address1 { get; set; }
        public string TumbolCode { get; set; }
        public string AmphurCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ZipCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    #endregion

    #region Staff
    public class AuthenticationResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string ResetPasswordToken { get; set; }        
    }

    public class StaffsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<StaffModel> Staffs { get; set; }
    }
    public class StaffResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public StaffModel Staff { get; set; }
    }

    public class ResetPasswordTokenResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
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
    public class BrandsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<BrandModel> Brands { get; set; }
    }

    public class BrandResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public BrandModel Brand { get; set; }
    }
    public class BrandModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
        public StaffModel Staff { get; set; }
    }
    #endregion

    #region Role
    public class RoleModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FunctionModel> Functions { get; set; }
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

    public class ProvincesResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ProvinceModel> Provinces { get; set; }
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
        public string ZipCode { get; set; }
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

    #region Product

    public class ProductsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ProductModel> Products { get; set; }
    }

    public class ProductResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ProductModel product { get; set; }
    }
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BrandId { get; set; }
        public int? ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<ProductAttachmentModel> Attachments { get; set; }

    }

    public class ProductAttachmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
        public string Extension { get; set; }
    }
    #endregion

    #region Promotion
    public class PromotionsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<PromotionModel> Promotions { get; set; }
    }

    public class PromotionResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public PromotionModel Promotion { get; set; }
    }

    public class PromotionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int PromotionTypeId { get; set; }
        public PromotionTypeModel PromotionType { get; set; }
        public int? PromotionSubTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MemberDiscount { get; set; }
        public int? BirthDateDiscount { get; set; }
        public int? ProductGroupDiscount { get; set; }
        public ImageModel image1 { get; set; }
        public ImageModel image2 { get; set; }
        public ImageModel image3 { get; set; }
        public ImageModel backgroundImage { get; set; }
        public bool IsActived { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ProductId { get; set; }
        public string Tel { get; set; }
        public string Facebook { get; set; }
        public string Line { get; set; }
        public string Web { get; set; }
    }

    public class ImageModel
    {
        public string name { get; set; }
        public string path { get; set; }
        public string file { get; set; }
        public string extension { get; set; }
        public string imageUrl { get; set; }
    }

    public class PromotionTypesResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<PromotionTypeModel> PromotionTypes { get; set; }
    }

    public class PromotionTypeResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public PromotionTypeModel PromotionType { get; set; }
    }

    public class PromotionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class PromotionSubTypeResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<PromotionSubTypeModel> promotionSubTypes { get; set; }
    }

    public class PromotionSubTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #region Role
    public class RolesResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<RoleModel> roles { get; set; }
    }

    public class RoleResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public RoleModel Role { get; set; }
    }

    #endregion

    #region Chart

    public class ChartResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<ChartsModel> charts { get; set; }
        public List<GraphModel> graphs { get; set; }
        public List<MarkerProvincesModel>  MarkerProvinces { get; set; }
    }
    public class ChartsModel
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public class GraphModel
    {
        public string name { get; set; }
        public List<ChartsModel> series { get; set; }
    }

    public class MarkerProvincesModel
    {
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Total { get; set; }
    }


    #endregion

    #region ReoductType

    public class ProductsTypeResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ProductTypeModel>  ProductTypes { get; set; }
        public ProductTypeModel  productType { get; set; }
    }

    public class ProductTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public bool IsActived { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    #endregion

    #region ContactUs

    public class ContactUsModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ContactUs contactUs { get; set; }
    }
    #endregion
}
