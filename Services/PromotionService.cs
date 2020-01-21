using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IPromotionService
    {
        Task<PromotionsResponseModel> GetPromotionsAsync(int brandId);
        Task<PromotionResponseModel> GetPromotionAsync(int brandId, int promotionId);
        Task<ResponseModel> CreateAsync(PromotionModel cPromotion);
        Task<ResponseModel> UpdateAsync(PromotionModel uPromotion);

        Task<PromotionsResponseModel> GetPromotionsValidAsync(int brandId);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPromotionTypeRepository _promotionTypeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        public PromotionService(
            IPromotionRepository promotionRepository,
            IPromotionTypeRepository promotionTypeRepository,
            IBrandRepository brandRepository,
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment
        )
        {
            _promotionRepository = promotionRepository;
            _promotionTypeRepository = promotionTypeRepository;
            _brandRepository = brandRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<PromotionsResponseModel> GetPromotionsAsync(int brandId)
        {
            var response = new PromotionsResponseModel();
            try
            {
                var promotions = await _promotionRepository.GetPromotionsAsync(brandId);
                response.Promotions = new List<PromotionModel>();
                foreach (var promotion in promotions)
                {
                    response.Promotions.Add(new PromotionModel()
                    {
                        Id = promotion.Id,
                        Description = promotion.Description,
                        CreatedDate = promotion.CreatedDate,
                        ModifiedDate = promotion.ModifiedDate,
                        BrandId = promotion.BrandId,
                        IsActived = promotion.IsActived,
                        CreatedBy = promotion.CreatedBy,
                        CreatedByName = $"{promotion.CreatedByNavigation.FirstName} {promotion.CreatedByNavigation.LastName}",
                        ModifiedBy = promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId,
                        PromotionType = new PromotionTypeModel()
                        {
                            Id = promotion.PromotionType.Id,
                            Name = promotion.PromotionType.Name,
                            Description = promotion.PromotionType.Description
                        }
                    });
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<PromotionsResponseModel> GetPromotionsValidAsync(int brandId)
        {
            var response = new PromotionsResponseModel();
            try
            {
                var promotions = await _promotionRepository.GetPromotionsValidAsync(brandId);
                response.Promotions = new List<PromotionModel>();
                foreach (var promotion in promotions)
                {
                    response.Promotions.Add(new PromotionModel()
                    {
                        Id = promotion.Id,
                        Description = promotion.Description,
                        CreatedDate = promotion.CreatedDate,
                        ModifiedDate = promotion.ModifiedDate,
                        BrandId = promotion.BrandId,
                        IsActived = promotion.IsActived,
                        CreatedBy = promotion.CreatedBy,
                        CreatedByName = $"{promotion.CreatedByNavigation.FirstName} {promotion.CreatedByNavigation.LastName}",
                        ModifiedBy = promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId,
                        PromotionType = new PromotionTypeModel()
                        {
                            Id = promotion.PromotionType.Id,
                            Name = promotion.PromotionType.Name,
                            Description = promotion.PromotionType.Description
                        }
                    });
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<PromotionResponseModel> GetPromotionAsync(int brandId, int promotionId)
        {
            var response = new PromotionResponseModel();
            try
            {
                var promotion = await _promotionRepository.GetPromotionAsync(brandId, promotionId);
                if (promotion != null)
                {
                    response.Promotion = new PromotionModel()
                    {
                        Id = promotion.Id,
                        Description = promotion.Description,
                        CreatedDate = promotion.CreatedDate,
                        ModifiedDate = promotion.ModifiedDate,
                        BrandId = promotion.BrandId,
                        IsActived = promotion.IsActived,
                        CreatedBy = promotion.CreatedBy,
                        CreatedByName = $"{promotion.CreatedByNavigation.FirstName} {promotion.CreatedByNavigation.LastName}",
                        ModifiedBy = promotion.ModifiedBy,
                        Name = promotion.Name,
                        PromotionTypeId = promotion.PromotionTypeId,
                        PromotionSubTypeId = promotion.PromotionSubTypeId,
                        MemberDiscount = promotion.MemberDiscount,
                        BirthDateDiscount = promotion.BirthDateDiscount,
                        ProductGroupDiscount = promotion.ProductGroupDiscount,
                        StartDate = promotion.StartDate.Value,
                        EndDate = promotion.EndDate.Value,
                        ProductId = promotion.ProductId,
                        Web = promotion.Web,
                        Tel = promotion.Tel,
                        Facebook = promotion.Facebook,
                        Line = promotion.Line
                    };

                    if (promotion.PromotionTypeId == 1)
                    {
                        ImageModel ImageBackground = new ImageModel();
                        if (promotion.ImageBackground != null)
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(promotion.ImageBackground);
                            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                            string[] Extensions = promotion.ImageBackground.Split('\\');
                            ImageBackground.name = Extensions[6];
                            //ImageBackground.path = promotion.ImageBackground;
                            ImageBackground.file = $"{promotion.ImageBackgroundExtention},{base64ImageRepresentation}";
                            ImageBackground.extension = null;
                        }
                        response.Promotion.backgroundImage = ImageBackground;
                        response.Promotion.image1 = new ImageModel();
                        response.Promotion.image2 = new ImageModel();
                        response.Promotion.image3 = new ImageModel();
                    }
                    else
                    {
                        ImageModel ImagePath1 = new ImageModel();
                        if (promotion.ImagePath1 != null)
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(promotion.ImagePath1);
                            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                            string[] Extensions = promotion.ImagePath1.Split('\\');
                            ImagePath1.name = Extensions[6];
                            //ImagePath1.path = promotion.ImagePath1;
                            ImagePath1.file = $"{promotion.ImageExtension1},{base64ImageRepresentation}";
                            ImagePath1.extension = null;
                            ImagePath1.imageUrl = promotion.ImageUrl1;

                        }
                        response.Promotion.image1 = ImagePath1;


                        ImageModel ImagePath2 = new ImageModel();
                        if (promotion.ImagePath2 != null)
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(promotion.ImagePath2);
                            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                            string[] Extensions = promotion.ImagePath2.Split('\\');
                            ImagePath2.name = Extensions[6];
                            //ImagePath1.path = promotion.ImagePath1;
                            ImagePath2.file = $"{promotion.ImageExtension2},{base64ImageRepresentation}";
                            ImagePath2.extension = null;
                            ImagePath2.imageUrl = promotion.ImageUrl2;
                        }
                        response.Promotion.image2 = ImagePath2;

                        ImageModel ImagePath3 = new ImageModel();
                        if (promotion.ImagePath3 != null)
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(promotion.ImagePath3);
                            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                            string[] Extensions = promotion.ImagePath3.Split('\\');
                            ImagePath3.name = Extensions[6];
                            //ImagePath1.path = promotion.ImagePath1;
                            ImagePath3.file = $"{promotion.ImageExtension3},{base64ImageRepresentation}";
                            ImagePath3.extension = null;
                            ImagePath3.imageUrl = promotion.ImageUrl3;
                        }
                        response.Promotion.image3 = ImagePath3;
                        response.Promotion.backgroundImage = new ImageModel();
                    }

                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> CreateAsync(PromotionModel cPromotion)
        {
            var response = new ResponseModel();
            try
            {
                var webRoot = _hostingEnvironment.ContentRootPath;

                string promotionsAttachmentPath = _configuration["Attachments:Promotions"];
                promotionsAttachmentPath = promotionsAttachmentPath.Replace("[#brandId#]", $"{cPromotion.BrandId.ToString()}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}");

                var brand = await _brandRepository.GetBrandAsync(cPromotion.BrandId);
                if (brand == null)
                {
                    response.Message = "Brand not found.";
                }

                var promotionType = await _promotionTypeRepository.GetPromotionTypeAsync(cPromotion.PromotionTypeId);
                if (promotionType == null)
                {
                    response.Message = "PromotionType not found";
                }
                var promotion = new Promotion()
                {
                    Id = 0,
                    Name = cPromotion.Name,
                    Description = cPromotion.Description,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = cPromotion.CreatedBy,
                    IsActived = cPromotion.IsActived,
                    Brand = brand,
                    PromotionType = promotionType,
                    StartDate = cPromotion.StartDate,
                    EndDate = cPromotion.EndDate,
                    Tel = cPromotion.Tel,
                    Facebook = cPromotion.Facebook,
                    Line = cPromotion.Line,
                    Web = cPromotion.Web
                };
                if (cPromotion.PromotionTypeId == 1)
                {
                    promotion.MemberDiscount = cPromotion.MemberDiscount;
                    promotion.BirthDateDiscount = cPromotion.BirthDateDiscount;
                    promotion.ProductGroupDiscount = cPromotion.ProductGroupDiscount;
                    promotion.ProductId = cPromotion.ProductId;


                    if (cPromotion.backgroundImage.name != "")
                    {
                        string[] Extensions = cPromotion.backgroundImage.file.Split(',');
                        var filePath = Path.Combine(promotionsAttachmentPath, cPromotion.backgroundImage.name);
                        if (!Directory.Exists(promotionsAttachmentPath))
                        {
                            Directory.CreateDirectory(promotionsAttachmentPath);
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                        promotion.ImageBackground = filePath;
                        promotion.ImageBackgroundExtention = Extensions[0];
                    }
                }
                else
                {
                    promotion.PromotionSubTypeId = cPromotion.PromotionSubTypeId;
                    if (cPromotion.image1.name != "" && cPromotion.image1.name != null)
                    {
                        string[] Extensions = cPromotion.image1.file.Split(',');
                        var filePath = Path.Combine(promotionsAttachmentPath, cPromotion.image1.name);
                        if (!Directory.Exists(promotionsAttachmentPath))
                        {
                            Directory.CreateDirectory(promotionsAttachmentPath);
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                        promotion.ImagePath1 = filePath;
                        promotion.ImageExtension1 = Extensions[0];
                        if (cPromotion.image1.imageUrl != "" && cPromotion.image1.imageUrl != null)
                        {
                            promotion.ImageUrl1 = cPromotion.image1.imageUrl;
                        }
                    }

                    if (cPromotion.image2.name != "" && cPromotion.image2.name != null)
                    {
                        string[] Extensions = cPromotion.image2.file.Split(',');
                        var filePath = Path.Combine(promotionsAttachmentPath, cPromotion.image2.name);
                        if (!Directory.Exists(promotionsAttachmentPath))
                        {
                            Directory.CreateDirectory(promotionsAttachmentPath);
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                        promotion.ImagePath2 = filePath;
                        promotion.ImageExtension2 = Extensions[0];
                        if (cPromotion.image2.imageUrl != "" && cPromotion.image2.imageUrl != null)
                        {
                            promotion.ImageUrl2 = cPromotion.image2.imageUrl;
                        }
                    }
                    if (cPromotion.image3.name != "" && cPromotion.image3.name != null)
                    {
                        string[] Extensions = cPromotion.image3.file.Split(',');
                        var filePath = Path.Combine(promotionsAttachmentPath, cPromotion.image3.name);
                        if (!Directory.Exists(promotionsAttachmentPath))
                        {
                            Directory.CreateDirectory(promotionsAttachmentPath);
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                        promotion.ImagePath3 = filePath;
                        promotion.ImageExtension3 = Extensions[0];
                        if (cPromotion.image3.imageUrl != "" && cPromotion.image3.imageUrl != null)
                        {
                            promotion.ImageUrl3 = cPromotion.image3.imageUrl;
                        }
                    }
                }





                bool isSuccess = await _promotionRepository.CreateAsync(promotion);
                if (isSuccess)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    Directory.Delete(promotionsAttachmentPath);
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAsync(PromotionModel uPromotion)
        {
            var response = new ResponseModel();
            try
            {
                string promotionsAttachmentPath = _configuration["Attachments:Promotions"];
                promotionsAttachmentPath = promotionsAttachmentPath.Replace("[#brandId#]", $"{uPromotion.BrandId.ToString()}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}");


                var dbPromotion = await _promotionRepository.GetPromotionAsync(uPromotion.BrandId, uPromotion.Id);
                var dbPromotionType = await _promotionTypeRepository.GetPromotionTypeAsync(uPromotion.PromotionTypeId);

                string oldPath = string.Empty;
                if (dbPromotion != null)
                {
                    dbPromotion.Id = uPromotion.Id;
                    dbPromotion.IsActived = uPromotion.IsActived;
                    dbPromotion.ModifiedDate = DateTime.UtcNow;
                    dbPromotion.ModifiedBy = uPromotion.ModifiedBy;
                    dbPromotion.Name = uPromotion.Name;
                    dbPromotion.Description = uPromotion.Description;
                    // dbPromotion.PromotionType = dbPromotionType;
                    dbPromotion.StartDate = uPromotion.StartDate;
                    dbPromotion.EndDate = uPromotion.EndDate;
                    dbPromotion.Tel = uPromotion.Tel;
                    dbPromotion.Facebook = uPromotion.Facebook;
                    dbPromotion.Line = uPromotion.Line;
                    dbPromotion.Web = uPromotion.Web;
                    dbPromotion.ProductId = uPromotion.ProductId;

                    if (dbPromotion.PromotionTypeId == 1)
                    {
                        dbPromotion.MemberDiscount = uPromotion.MemberDiscount;
                        dbPromotion.BirthDateDiscount = uPromotion.BirthDateDiscount;
                        dbPromotion.ProductGroupDiscount = uPromotion.ProductGroupDiscount;
                        if ( uPromotion.backgroundImage != null )
                        {
                            oldPath = dbPromotion.ImageBackground;
                            string[] Extensions = uPromotion.backgroundImage.file.Split(',');
                            var filePath = Path.Combine(promotionsAttachmentPath, uPromotion.backgroundImage.name);
                            if (!Directory.Exists(promotionsAttachmentPath))
                            {
                                Directory.CreateDirectory(promotionsAttachmentPath);
                            }

                            File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                            dbPromotion.ImageBackground = filePath;
                            dbPromotion.ImageBackgroundExtention = Extensions[0];
                        }
                    }
                    else
                    {
                        //dbPromotion.PromotionSubTypeId = uPromotion.PromotionSubTypeId;
                        if (uPromotion.image1.name != "" && uPromotion.image1.name  != null)
                        {
                            oldPath = dbPromotion.ImagePath1;
                            string[] Extensions = uPromotion.image1.file.Split(',');
                            var filePath = Path.Combine(promotionsAttachmentPath, uPromotion.image1.name);
                            if (!Directory.Exists(promotionsAttachmentPath))
                            {
                                Directory.CreateDirectory(promotionsAttachmentPath);
                            }

                            File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                            dbPromotion.ImagePath1 = filePath;
                            dbPromotion.ImageExtension1 = Extensions[0];
                        }

                        if (uPromotion.image2.name != "" && uPromotion.image2.name != null)
                        {
                            oldPath = dbPromotion.ImagePath2;
                            string[] Extensions = uPromotion.image2.file.Split(',');
                            var filePath = Path.Combine(promotionsAttachmentPath, uPromotion.image2.name);
                            if (!Directory.Exists(promotionsAttachmentPath))
                            {
                                Directory.CreateDirectory(promotionsAttachmentPath);
                            }

                            File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                            dbPromotion.ImagePath2 = filePath;
                            dbPromotion.ImageExtension2 = Extensions[0];
                        }
                        if (uPromotion.image3.name != "" && uPromotion.image3.name != null)
                        {
                            oldPath = dbPromotion.ImagePath3;
                            string[] Extensions = uPromotion.image3.file.Split(',');
                            var filePath = Path.Combine(promotionsAttachmentPath, uPromotion.image3.name);
                            if (!Directory.Exists(promotionsAttachmentPath))
                            {
                                Directory.CreateDirectory(promotionsAttachmentPath);
                            }

                            File.WriteAllBytes(filePath, Convert.FromBase64String(Extensions[1]));
                            dbPromotion.ImagePath3 = filePath;
                            dbPromotion.ImageExtension3 = Extensions[0];
                        }
                    }


                    bool isSuccess = await _promotionRepository.UpdateAsync(dbPromotion);
                    if (isSuccess)
                    {
                        response.IsSuccess = true;
                        if (oldPath != null)
                        {
                            var oldPathoArray = oldPath.Split('\\');
                            string delPath = $"\\\\{oldPathoArray[2]}\\{oldPathoArray[3]}\\{oldPathoArray[4]}\\{oldPathoArray[5]}";
                            Directory.Delete(delPath, true);
                        }

                    }
                    else
                    {
                        Directory.Delete(promotionsAttachmentPath);
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
