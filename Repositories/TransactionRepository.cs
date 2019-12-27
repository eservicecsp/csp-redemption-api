using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface ITransactionRepository
    {
        Task<bool> CreateTransactionJigSawAsync(Transaction transaction, QrCode qrCode);
        Task<bool> CreateTransactionEnrollmentAsync(Transaction transaction, QrCode qrCode, Enrollment enrollments);
        Task<bool> CreateTransactionPointAsync(Transaction transaction, QrCode qrCode);
        Task<bool> CreateTransactionErrorAsync(Transaction transaction);
        Task<Transaction> GetWinTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionByCampaignsIdAsync(PaginationModel data);
        Task<int> GetTransactionTotalByCampaignsIdAsync(PaginationModel data);
        Task<int> GetCountTransactionByTypeId(int campaignId, int typeId);
        Task<int> GetCountAllTransaction(int campaignId);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IQrCodeRepository qrCodeRepository;

        public TransactionRepository(
            IQrCodeRepository qrCodeRepository
        )
        {
            this.qrCodeRepository = qrCodeRepository;
        }
        public async Task<bool> CreateTransactionJigSawAsync(Transaction transaction, QrCode qrCode)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                using (var tran = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        Context.Transaction.Add(transaction);
                        if (await Context.SaveChangesAsync() > 0)
                        {
                            var data = new QrCode
                            {
                                Id = qrCode.Id,
                                Token = qrCode.Token,
                                CampaignId = qrCode.CampaignId,
                                Peice = qrCode.Peice,
                                ConsumerId = qrCode.ConsumerId,
                                TransactionId = transaction.Id,
                                Point = qrCode.Point,
                                ScanDate = qrCode.ScanDate
                            };
                            // await this.qrCodeRepository.UpdateAsync(qrCode);
                            var dbQrCode = await Context.QrCode.SingleAsync(x => x.Token == qrCode.Token && x.CampaignId == qrCode.CampaignId && x.ConsumerId == null);
                            Context.Entry(dbQrCode).CurrentValues.SetValues(data);

                            isSuccess = await Context.SaveChangesAsync() > 0;
                            tran.Commit();
                        }

                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        isSuccess = false;
                        throw;
                    }
                }


            }
            return isSuccess;
        }
        public async Task<bool> CreateTransactionEnrollmentAsync(Transaction transaction, QrCode qrCode, Enrollment enrollments)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                using (var tran = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var enrollmentsDb = await Context.Enrollment.SingleOrDefaultAsync(x => x.Tel == enrollments.Tel);
                        if(enrollmentsDb == null)
                        {
                            await Context.Enrollment.AddAsync(enrollments);
                            await Context.SaveChangesAsync();
                        }
                        else
                        {
                            enrollments.Id = enrollmentsDb.Id;
                        }
                        

                        await Context.Transaction.AddAsync(transaction);
                        await Context.SaveChangesAsync();

                        var dbQrCode = await Context.QrCode.SingleAsync(x => x.Token == qrCode.Token && x.CampaignId == qrCode.CampaignId && x.Code == qrCode.Code && x.EnrollmentId == null);
                        dbQrCode.EnrollmentId = enrollments.Id;
                        Context.Entry(dbQrCode).CurrentValues.SetValues(dbQrCode);

                        isSuccess = await Context.SaveChangesAsync() > 0;
                        tran.Commit();

                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        isSuccess = false;
                        throw;
                    }
                }


            }
            return isSuccess;
        }
        
        public async Task<bool> CreateTransactionPointAsync(Transaction transaction, QrCode qrCode)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                using (var tran = Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        Context.Transaction.Add(transaction);
                        if (await Context.SaveChangesAsync() > 0)
                        {
                            var data = new QrCode
                            {
                                Id = qrCode.Id,
                                Token = qrCode.Token,
                                CampaignId = qrCode.CampaignId,
                                Peice = qrCode.Peice,
                                ConsumerId = qrCode.ConsumerId,
                                TransactionId = transaction.Id,
                                Point = qrCode.Point,
                                ScanDate = qrCode.ScanDate
                            };
                            var dbQrCode = await Context.QrCode.SingleAsync(x => x.Token == qrCode.Token && x.CampaignId == qrCode.CampaignId && x.ConsumerId == null);
                            Context.Entry(dbQrCode).CurrentValues.SetValues(data);
                            if (await Context.SaveChangesAsync() > 0)
                            {
                                var dbCosumer = await Context.Consumer.SingleAsync(x => x.Id == qrCode.ConsumerId);
                                dbCosumer.Point = dbCosumer.Point + qrCode.Point;
                                isSuccess = await Context.SaveChangesAsync() > 0;
                            }



                            tran.Commit();
                        }


                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        isSuccess = false;
                        throw;
                    }
                }


            }
            return isSuccess;
        }
        public async Task<bool> CreateTransactionErrorAsync(Transaction transaction)
        {
            bool isSuccess = false;
            using (var Context = new CSP_RedemptionContext())
            {
                Context.Transaction.Add(transaction);
                isSuccess = await Context.SaveChangesAsync() > 0;

            }
            return isSuccess;
        }
        public async Task<Transaction> GetWinTransactionAsync(Transaction transaction)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Transaction.Where(x => x.CampaignId == transaction.CampaignId && x.Token == transaction.Token && x.TransactionTypeId == 1).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Transaction>> GetTransactionByCampaignsIdAsync(PaginationModel data)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var transactions = Context.Transaction.Include(x => x.Consumer).Include(x => x.TransactionType).Where(x => x.CampaignId == data.campaignId);
                if (data.filter != null)
                {
                    transactions = transactions.Where(x => x.Token.Contains(data.filter) ||
                                                 x.TransactionType.Name.Contains(data.filter) ||
                                                 x.ResponseMessage.Contains(data.filter) ||
                                                 x.Consumer.FirstName.Contains(data.filter) ||
                                                 x.Consumer.LastName.Contains(data.filter) ||
                                                 x.Consumer.Phone.Contains(data.filter) ||
                                                 x.Consumer.LastName.Contains(data.filter)
                                         );
                }

                if (data.sortActive != null)
                {
                    //Token
                    if (data.sortActive == "token" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.Token);
                    }
                    else if (data.sortActive == "token")
                    {
                        transactions = transactions.OrderBy(x => x.Token);
                    }

                    //status
                    if (data.sortActive == "status" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.TransactionType.Name);
                    }
                    else if (data.sortActive == "status")
                    {
                        transactions = transactions.OrderBy(x => x.TransactionType.Name);
                    }

                    //ResponseMesage
                    if (data.sortActive == "message" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.ResponseMessage);
                    }
                    else if (data.sortActive == "message")
                    {
                        transactions = transactions.OrderBy(x => x.ResponseMessage);
                    }

                    //FirstName
                    if (data.sortActive == "FirstName" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.Consumer.FirstName);
                    }
                    else if (data.sortActive == "FirstName")
                    {
                        transactions = transactions.OrderBy(x => x.Consumer.FirstName);
                    }

                    //LastName
                    if (data.sortActive == "LastName" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.Consumer.LastName);
                    }
                    else if (data.sortActive == "LastName")
                    {
                        transactions = transactions.OrderBy(x => x.Consumer.LastName);
                    }

                    //Phone
                    if (data.sortActive == "Phone" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.Consumer.Phone);
                    }
                    else if (data.sortActive == "Phone")
                    {
                        transactions = transactions.OrderBy(x => x.Consumer.Phone);
                    }

                    //Email
                    if (data.sortActive == "Email" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.Consumer.Email);
                    }
                    else if (data.sortActive == "Email")
                    {
                        transactions = transactions.OrderBy(x => x.Consumer.Email);
                    }

                    //Email
                    if (data.sortActive == "createDate" && data.sortDirection == "desc")
                    {
                        transactions = transactions.OrderByDescending(x => x.CreatedDate);
                    }
                    else if (data.sortActive == "createDate")
                    {
                        transactions = transactions.OrderBy(x => x.CreatedDate);
                    }

                }

                int length = await this.GetTransactionTotalByCampaignsIdAsync(data);
                int index = 0;
                if (data.pageIndex > 0)
                {
                    index = (data.pageIndex * data.pageSize);
                }


                if (index >= length)
                {
                    transactions = transactions.Skip(0).Take(data.pageSize);
                }
                else
                {
                    transactions = transactions.Skip(index).Take(data.pageSize);
                }
                return await transactions.ToListAsync();
            }
        }
        public async Task<int> GetTransactionTotalByCampaignsIdAsync(PaginationModel data)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var consumers = Context.Consumer.AsQueryable();
                var transactions = Context.Transaction.Include(x => x.Consumer).Include(x => x.TransactionType).Where(x => x.CampaignId == data.campaignId);
                if (data.filter != null)
                {
                    transactions = transactions.Where(x => x.Token.Contains(data.filter) ||
                                                x.TransactionType.Name.Contains(data.filter) ||
                                                x.ResponseMessage.Contains(data.filter) ||
                                                x.Consumer.FirstName.Contains(data.filter) ||
                                                x.Consumer.LastName.Contains(data.filter) ||
                                                x.Consumer.Phone.Contains(data.filter) ||
                                                x.Consumer.LastName.Contains(data.filter)
                                        );
                }


                return await transactions.CountAsync();
            }
        }

        public async Task<int> GetCountTransactionByTypeId(int campaignId, int typeId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Transaction.Where(x => x.CampaignId == campaignId && x.TransactionTypeId == typeId).CountAsync();
            }
        }
        public async Task<int> GetCountAllTransaction(int campaignId)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.Transaction.Where(x => x.CampaignId == campaignId).CountAsync();
            }
        }
    }
}
