using CSP_Redemption_WebApi.Entities.DBContext;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Repositories
{
    public interface IQrCodeRepository
    {
        Task<QrCode> GetQrCode(QrCode qrCode);
        Task<bool> UpdateAsync(QrCode qrCode);
        Task<int[]> GetPiece(QrCode qrCode);
    }
    public class QrCodeRepository : IQrCodeRepository
    {
        public async Task<QrCode> GetQrCode(QrCode qrCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                return await Context.QrCode.DefaultIfEmpty(null).Where(x => x.Token == qrCode.Token && x.CampaignId == qrCode.CampaignId).FirstAsync();
            }
        }

        public async Task<bool> UpdateAsync(QrCode qrCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                //var data = new QrCode
                //{
                //    Token = qrCode.Token,
                //    CampaignId = qrCode.CampaignId,
                //    Peice = qrCode.Peice,
                //    ConsumerId = qrCode.ConsumerId,
                //    TransactionId = qrCode.TransactionId,
                //    Point = qrCode.Point,
                //    ScanDate = qrCode.ScanDate
                //};
                var dbQrCode = await Context.QrCode.SingleAsync(x => x.Token == qrCode.Token && x.CampaignId == qrCode.CampaignId && x.ConsumerId == null);
                dbQrCode.ConsumerId = qrCode.ConsumerId;
                //dbQrCode.TransactionId = qrCode.TransactionId;
                dbQrCode.ScanDate = qrCode.ScanDate;

                Context.Entry(dbQrCode).CurrentValues.SetValues(dbQrCode);
                return await Context.SaveChangesAsync() > 0;
            }
        }

        public async Task<int[]> GetPiece(QrCode qrCode)
        {
            using (var Context = new CSP_RedemptionContext())
            {
                var Piece = await Context.QrCode.Where(x => x.ConsumerId == qrCode.ConsumerId && x.CampaignId == qrCode.CampaignId).ToListAsync();
                return Piece.GroupBy(x => x.Peice.Value).Select(x => x.Key).ToArray();
            }
        }
    }
}
