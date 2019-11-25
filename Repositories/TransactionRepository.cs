using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
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
        Task<bool> CreateTransactionPointAsync(Transaction transaction, QrCode qrCode);
        Task<bool> CreateTransactionErrorAsync(Transaction transaction);
        Task<Transaction> GetWinTransactionAsync(Transaction transaction);
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
                        if(await Context.SaveChangesAsync() > 0)
                        {
                            await this.qrCodeRepository.UpdateAsync(qrCode);
                            isSuccess = true;
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
                        if(await Context.SaveChangesAsync() > 0)
                        {
                            qrCode.TransactionId = transaction.Id;
                            await this.qrCodeRepository.UpdateAsync(qrCode);

                            isSuccess = true;
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
    }
}
