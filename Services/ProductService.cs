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
    public interface IProductService
    {
        Task<ProductsResponseModel> GetProductsByBrandIdAsync(int brandId);
        Task<ProductResponseModel> GetProductsByIdAsync(int id);
        Task<ResponseModel> CreateAsync(ProductModel product);
        Task<ResponseModel> UpdateAsync(ProductModel product);
    }
    public class ProductService : IProductService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly ProductRepository productRepository;

        public ProductService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, ProductRepository productRepository)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.productRepository = productRepository;
        }

        public async Task<ProductsResponseModel> GetProductsByBrandIdAsync(int brandId)
        {
            var response = new ProductsResponseModel();

            try
            {
                var webRoot = hostingEnvironment.ContentRootPath;

                string productAttachmentPath = string.Empty;
                string subDomain = this.configuration["SubDomain"];
                if (!string.IsNullOrEmpty(subDomain))
                {
                    productAttachmentPath = Path.Combine(webRoot, subDomain, "Attachments/Products");
                }
                else
                {
                    productAttachmentPath = Path.Combine(webRoot, "Attachments/Products");
                }

                var products = await this.productRepository.GetProductsByBrandIdAsync(brandId);
                response.Products = new List<ProductModel>();

                foreach (var product in products)
                {
                    var attachments = new List<ProductAttachmentModel>();
                    if (product.ProductAttachment.Count() > 0)
                    {
                        foreach (var attachment in product.ProductAttachment)
                        {
                            var filePath = Path.Combine(productAttachmentPath, attachment.Name);

                            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                            string base64String = Convert.ToBase64String(imageBytes);

                            attachments.Add(new ProductAttachmentModel()
                            {
                                Id = attachment.Id,
                                File = base64String,
                                Name = attachment.Name,
                                Path = null,
                                Extension = attachment.Extension
                            });
                        }
                    }
                    else
                    {
                        attachments = null;
                    }

                    response.Products.Add(new ProductModel()
                    {
                        Description = product.Description,
                        CreatedDate = product.CreatedDate,
                        BrandId = product.BrandId,
                        CreatedBy = product.CreatedBy,
                        CreatedName = $"{product.CreatedByNavigation.FirstName} { product.CreatedByNavigation.LastName}",
                        Id = product.Id,
                        Name = product.Name,
                        Attachments = attachments
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
        public async Task<ProductResponseModel> GetProductsByIdAsync(int id)
        {
            var response = new ProductResponseModel();

            try
            {
                var product = await this.productRepository.GetProductsByIdAsync(id);

                var webRoot = hostingEnvironment.ContentRootPath;

                string productAttachmentPath = string.Empty;
                string subDomain = this.configuration["SubDomain"];
                if (!string.IsNullOrEmpty(subDomain))
                {
                    productAttachmentPath = Path.Combine(webRoot, subDomain, "Attachments/Products");
                }
                else
                {
                    productAttachmentPath = Path.Combine(webRoot, "Attachments/Products");
                }

                if (product != null)
                {
                    response.product = new ProductModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Attachments = new List<ProductAttachmentModel>()
                    };

                    foreach (var attachment in product.ProductAttachment)
                    {
                        var filePath = Path.Combine(productAttachmentPath, attachment.Name);

                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(imageBytes);


                        response.product.Attachments.Add(new ProductAttachmentModel()
                        {
                            Id = attachment.Id,
                            File = base64String,
                            Name = attachment.Name,
                            Path = null,
                            Extension = attachment.Extension
                        });
                    }
                }


                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> CreateAsync(ProductModel product)
        {
            var response = new ResponseModel();

            try
            {
                var webRoot = hostingEnvironment.ContentRootPath;

                string productAttachmentPath = string.Empty;
                string subDomain = this.configuration["SubDomain"];
                if (!string.IsNullOrEmpty(subDomain))
                {
                    productAttachmentPath = Path.Combine(webRoot, subDomain, @"Attachments\Products");
                }
                else
                {
                    productAttachmentPath = Path.Combine(webRoot, @"Attachments\Products");
                }

                

                var iProductAttachments = new List<ProductAttachment>();
                foreach (var item in product.Attachments)
                {
                    File.WriteAllBytes(Path.Combine(productAttachmentPath, item.Name), Convert.FromBase64String(item.File));
                    iProductAttachments.Add(new ProductAttachment()
                    {
                        Id = 0,
                        Extension = item.Extension,
                        Name = item.Name,
                        Path = Path.Combine(productAttachmentPath, item.Name),
                        ProductId = product.Id,
                    });
                }

                var iProduct = new Product()
                {
                    Id = product.Id,
                    Description = product.Description,
                    CreatedDate = product.CreatedDate,
                    BrandId = product.BrandId,
                    CreatedBy = product.CreatedBy,
                    Name = product.Name,
                    ProductAttachment = iProductAttachments
                };

                response.IsSuccess = await this.productRepository.CreateAsync(iProduct);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> UpdateAsync(ProductModel product)
        {
            var response = new ResponseModel();

            try
            {
                var webRoot = hostingEnvironment.ContentRootPath;

                string productAttachmentPath = string.Empty;
                string subDomain = this.configuration["SubDomain"];
                if (!string.IsNullOrEmpty(subDomain))
                {
                    productAttachmentPath = Path.Combine(webRoot, subDomain, "Attachments/Products");
                }
                else
                {
                    productAttachmentPath = Path.Combine(webRoot, "Attachments/Products");
                }

                var dbProduct = await this.productRepository.GetProductsByIdAsync(product.Id);
                if (dbProduct != null)
                {
                    var uProductAttachments = new List<ProductAttachment>();

                    foreach (var item in product.Attachments)
                    {
                        File.WriteAllBytes(Path.Combine(productAttachmentPath, item.Name), Convert.FromBase64String(item.File));
                        uProductAttachments.Add(new ProductAttachment()
                        {
                            Id = 0,
                            Extension = item.Extension,
                            Name = item.Name,
                            Path = Path.Combine(productAttachmentPath, item.Name),
                            ProductId = dbProduct.Id
                        });
                    }

                   

                    var uProduct = new Product()
                    {
                        Id = dbProduct.Id,
                        Description = product.Description,
                        Name = product.Name,
                        CreatedDate = dbProduct.CreatedDate,
                        Brand = dbProduct.Brand,
                        BrandId = dbProduct.BrandId,
                        CreatedBy = dbProduct.CreatedBy,
                        ProductAttachment = uProductAttachments
                    };

                    response.IsSuccess = await this.productRepository.UpdateAsync(uProduct);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
