using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IEnrollmentService
    {
        Task<ResponseModel> ImportJob(ImportDataBinding data);
        Task<EnrollmentsByPaginationResponseModel> GetEnrollmentsByCampaignIdAsync(PaginationModel data);
        Task<ResponseModel> SendSelected(List<Enrollment> enrollments, string channel, int campaignId);
        Task<ResponseModel> SendAll(PaginationModel data, string channel, int campaignId);
    }
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly ICampaignRepository campaignRepository;
        private readonly IQrCodeRepository qrCodeRepository;
        public EnrollmentService
           (
             IEnrollmentRepository enrollmentRepository,
             IHostingEnvironment hostingEnvironment,
             IConfiguration configuration,
             ICampaignRepository campaignRepository ,
             IQrCodeRepository qrCodeRepository
           )
        {
            this.enrollmentRepository = enrollmentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.campaignRepository = campaignRepository;
            this.qrCodeRepository = qrCodeRepository;
        }
        public async Task<ResponseModel> ImportJob(ImportDataBinding data)
        {
            var response = new ResponseModel();
            DateTime now = DateTime.Now;
            string contentRoot = hostingEnvironment.ContentRootPath;
            string webRoot = hostingEnvironment.ContentRootPath;
            string AttachfilePath = string.Empty;
            string subDomain = this.configuration["SubDomain"];

            // temp directory in root directory
            var filePath = Path.Combine(AttachfilePath, data.fileName);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                if (!string.IsNullOrEmpty(subDomain))
                {
                    AttachfilePath = Path.Combine(webRoot, subDomain, "Temp");
                }
                else
                {
                    AttachfilePath = Path.Combine(webRoot, "Temp");
                }
                if (!Directory.Exists(AttachfilePath)) Directory.CreateDirectory(AttachfilePath);

                File.WriteAllBytes(filePath, Convert.FromBase64String(data.file.Split(',').Last()));

                string[] lines = System.IO.File.ReadAllLines(filePath, Encoding.GetEncoding(874));
                int countLine = 0;
                var enrollments = new List<Enrollment>();
                if (lines.Count() <= 1)
                {
                    response.IsSuccess = false;
                    response.Message = "Text file invalid format or format not supported.";
                    return response;
                }

                try
                {
                    foreach (string line in lines)
                    {
                        if ((countLine != 0) && (line.Split('|')[0] != ""))
                        {
                            enrollments.Add(new Enrollment()
                            {
                                FirstName = line.Split('|')[0],
                                LastName = line.Split('|')[1],
                                Tel = line.Split('|')[2],
                                Email = line.Split('|')[3],
                                CampaignId = data.CampaignId,
                                CreatedDate = now,
                                CreatedBy = data.createBy,
                                IsConsumer = false
                            });
                        }
                        countLine++;
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(filePath)) File.Delete(filePath);

                    response.IsSuccess = false;
                    response.Message = "Text file invalid format or format not supported.";
                    return response;
                }

                if (File.Exists(filePath)) File.Delete(filePath);

                if (enrollments.Count > 0)
                {
                    response.IsSuccess = await this.enrollmentRepository.ImportFileAsync(enrollments);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            return response;
        }

        public async Task<EnrollmentsByPaginationResponseModel> GetEnrollmentsByCampaignIdAsync(PaginationModel data)
        {
            var response = new EnrollmentsByPaginationResponseModel();
            try
            {
                var enrollments = await this.enrollmentRepository.GetEnrollmentsByBrandIdAsync(data, "WEB");
                if (enrollments != null)
                {
                    response.length = await this.enrollmentRepository.GetEnrollmentTotalByBrandIdAsync(data);
                    response.data = enrollments;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> SendSelected(List<Enrollment> enrollments, string channel, int campaignId)
        {
            var response = new ResponseModel();
            try
            {
                // int[] enrollmentIds;
                var campaign = await this.campaignRepository.GetCampaignByIdAsync(campaignId);
                if (campaign != null)
                {
                    var token = await this.qrCodeRepository.GetTokenByCompanyIdAsync(campaignId);
                    if(token != null)
                    {
                        string url = campaign.Url.Replace("[#campaignId#]", campaignId.ToString());
                        url = url.Replace("[#token#]", token.Token);
                        string[] str1 = url.Split("?".ToCharArray());
                        string fullUrl = $"{ str1[0]}/register?{str1[1]}";
                        //
                        var intArray = enrollments.Select(x => x.Id).ToArray();
                        var mainUri = this.configuration["GMCServices:Uri"];
                        var apiPath = this.configuration["GMCServices:AutomationApiPath"];
                        //var stringPayload = JsonConvert.SerializeObject(intArray);
                        var stringPayload = JsonConvert.SerializeObject(new
                        {
                            channel = channel,
                            type = "enrollment",
                            id = intArray,
                            url = fullUrl.ToString()

                        });
                        var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            var responseFromApi = await client.PostAsync(mainUri + apiPath, content);
                            var result = await responseFromApi.Content.ReadAsStringAsync();
                        }

                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Not found Token.";
                    }
                   
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found campaign.";
                }
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> SendAll(PaginationModel data, string channel, int campaignId)
        {
            var response = new ResponseModel();
            try
            {
                var campaign = await this.campaignRepository.GetCampaignByIdAsync(campaignId);
                if (campaign != null)
                {
                    var token = await this.qrCodeRepository.GetTokenByCompanyIdAsync(campaignId);
                    if (token != null)
                    {
                        var enrollments = await this.enrollmentRepository.GetEnrollmentsByBrandIdAsync(data, "API");
                        string url = campaign.Url.Replace("[#campaignId#]", campaignId.ToString());
                        url = url.Replace("[#token#]", token.Token);
                        string[] str1 = url.Split("?".ToCharArray());
                        string fullUrl = $"{ str1[0]}/register?{str1[1]}";

                        var intArray = enrollments.Select(x => x.Id).ToArray();
                        var mainUri = this.configuration["GMCServices:Uri"];
                        var apiPath = this.configuration["GMCServices:AutomationApiPath"];
                        var stringPayload = JsonConvert.SerializeObject(new
                        {
                            channel = channel,
                            type = "enrollment",
                            id = intArray,
                            url = fullUrl.ToString()

                        });
                        var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            var responseFromApi = await client.PostAsync(mainUri + apiPath, content);
                            var result = await responseFromApi.Content.ReadAsStringAsync();
                        }

                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Not found Token.";
                    }

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not found campaign.";
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
