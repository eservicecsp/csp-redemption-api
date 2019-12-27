using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using FastMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<bool> ImportFileAsync(List<Enrollment> enrollments);
        Task<List<Enrollment>> GetEnrollmentsByBrandIdAsync(PaginationModel data, string type);
        Task<int> GetEnrollmentTotalByBrandIdAsync(PaginationModel data);
        //Task<bool> CreateAsync(Enrollment enrollments, QrCode qrCode);
    }
    public class EnrollmentRepository: IEnrollmentRepository
    {
        private readonly IConfiguration configuration;

        public EnrollmentRepository
            (
                IConfiguration configuration
            )
        {
            this.configuration = configuration;
        }
        public async Task<bool> ImportFileAsync(List<Enrollment> enrollments)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string connectionstring = this.configuration["ConnectionStrings:ApiDatabase"];
                        var copyParameters = new[]
                        {
                            nameof(Enrollment.FirstName),
                            nameof(Enrollment.LastName),
                            nameof(Enrollment.Tel),
                            nameof(Enrollment.Email),
                            nameof(Enrollment.CampaignId),
                            nameof(Enrollment.CreatedDate),
                            nameof(Enrollment.CreatedBy),
                            nameof(Enrollment.IsConsumer),
                        };
                        using (var bcp = new SqlBulkCopy(connectionstring))
                        {
                            using (var reader = ObjectReader.Create(enrollments, copyParameters))
                            {
                                SqlBulkCopyColumnMapping mapFirstName = new SqlBulkCopyColumnMapping(copyParameters[0], "FirstName");
                                SqlBulkCopyColumnMapping mapLastName = new SqlBulkCopyColumnMapping(copyParameters[1], "LastName");
                                SqlBulkCopyColumnMapping mapTel = new SqlBulkCopyColumnMapping(copyParameters[2], "Tel");
                                SqlBulkCopyColumnMapping mapEmail = new SqlBulkCopyColumnMapping(copyParameters[3], "Email");
                                SqlBulkCopyColumnMapping mapCampaignId = new SqlBulkCopyColumnMapping(copyParameters[4], "CampaignId");
                                SqlBulkCopyColumnMapping mapCreatedDate = new SqlBulkCopyColumnMapping(copyParameters[5], "CreatedDate");
                                SqlBulkCopyColumnMapping mapCreatedBy = new SqlBulkCopyColumnMapping(copyParameters[6], "CreatedBy");
                                SqlBulkCopyColumnMapping mapIsConsumer = new SqlBulkCopyColumnMapping(copyParameters[7], "IsConsumer");

                                bcp.ColumnMappings.Add(mapFirstName);
                                bcp.ColumnMappings.Add(mapLastName);
                                bcp.ColumnMappings.Add(mapTel);
                                bcp.ColumnMappings.Add(mapEmail);
                                bcp.ColumnMappings.Add(mapCampaignId);
                                bcp.ColumnMappings.Add(mapCreatedDate);
                                bcp.ColumnMappings.Add(mapCreatedBy);
                                bcp.ColumnMappings.Add(mapIsConsumer);
                                bcp.DestinationTableName = "[Enrollment]";
                                bcp.WriteToServer(reader);
                            }
                        }
                        isSuccess = true;
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
            return isSuccess;
        }
        public async Task<List<Enrollment>> GetEnrollmentsByBrandIdAsync(PaginationModel data, string type)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var enrollments = Context.Enrollment.Where(x => x.CampaignId == data.campaignId && x.IsConsumer == false);
                if (data.filter != null)
                {
                    enrollments = enrollments.Where(x => x.FirstName.Contains(data.filter) ||
                                                 x.LastName.Contains(data.filter) ||
                                                 x.Tel.Contains(data.filter) ||
                                                 x.Email.Contains(data.filter)
                                         );
                }

                if(type == "WEB")
                {
                    if (data.sortActive != null)
                    {
                        //FirstName
                        if (data.sortActive == "FirstName" && data.sortDirection == "desc")
                        {
                            enrollments = enrollments.OrderByDescending(x => x.FirstName);
                        }
                        else if (data.sortActive == "FirstName")
                        {
                            enrollments = enrollments.OrderBy(x => x.FirstName);
                        }

                        //LastName
                        if (data.sortActive == "LastName" && data.sortDirection == "desc")
                        {
                            enrollments = enrollments.OrderByDescending(x => x.LastName);
                        }
                        else if (data.sortActive == "LastName")
                        {
                            enrollments = enrollments.OrderBy(x => x.LastName);
                        }

                        //Phone
                        if (data.sortActive == "Tel" && data.sortDirection == "desc")
                        {
                            enrollments = enrollments.OrderByDescending(x => x.Tel);
                        }
                        else if (data.sortActive == "Tel")
                        {
                            enrollments = enrollments.OrderBy(x => x.Tel);
                        }

                        //Email
                        if (data.sortActive == "Email" && data.sortDirection == "desc")
                        {
                            enrollments = enrollments.OrderByDescending(x => x.Email);
                        }
                        else if (data.sortActive == "Email")
                        {
                            enrollments = enrollments.OrderBy(x => x.Email);
                        }

                    }

                    int length = await this.GetEnrollmentTotalByBrandIdAsync(data);
                    int index = 0;
                    if (data.pageIndex > 0)
                    {
                        index = (data.pageIndex * data.pageSize);
                    }


                    if (index >= length)
                    {
                        enrollments = enrollments.Skip(0).Take(data.pageSize);
                    }
                    else
                    {
                        enrollments = enrollments.Skip(index).Take(data.pageSize);
                    }
                }
                
                return await enrollments.ToListAsync();
            }
        }
        public async Task<int> GetEnrollmentTotalByBrandIdAsync(PaginationModel data)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var enrollments = Context.Enrollment.Where(x => x.CampaignId == data.campaignId && x.IsConsumer == false);
                if (data.filter != null)
                {
                    enrollments = enrollments.Where(x => x.FirstName.Contains(data.filter) ||
                                                 x.LastName.Contains(data.filter) ||
                                                 x.Tel.Contains(data.filter) ||
                                                 x.Email.Contains(data.filter)
                                         );
                }

                return await enrollments.CountAsync();
            }
        }

        //public async Task<bool> CreateAsync(Enrollment enrollments, QrCode qrCode )
        //{
        //    bool isSuccess = false;
        //    using (var Context = new CSP_RedemptionContext())
        //    {
        //        using (var transaction = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //        {
        //            try
        //            {
        //                await Context.Enrollment.AddAsync(enrollments);
        //                await Context.SaveChangesAsync();

        //                QrCode thisRow = await Context.QrCode.SingleAsync(x => x.Id == qrCode.Id);
        //                Context.Entry(thisRow).CurrentValues.SetValues(qrCode);
        //                await Context.SaveChangesAsync();

        //                isSuccess = true;
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }

        //        }
        //    }
        //    return isSuccess;
        //}
    }
}
