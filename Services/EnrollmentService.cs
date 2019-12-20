using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IEnrollmentService
    {
        Task<ResponseModel> ImportJob(ImportDataBinding data);
        Task<EnrollmentsByPaginationResponseModel> GetEnrollmentsByCampaignIdAsync(PaginationModel data);
    }
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        public EnrollmentService
           (
             IEnrollmentRepository enrollmentRepository,
             IHostingEnvironment hostingEnvironment,
             IConfiguration configuration
           )
        {
            this.enrollmentRepository = enrollmentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
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
                var enrollments = await this.enrollmentRepository.GetEnrollmentsByBrandIdAsync(data);
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
    }
}
