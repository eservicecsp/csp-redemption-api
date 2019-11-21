﻿using System;
using System.Collections.Generic;

namespace CSP_Redemption_WebApi.Entities.Models
{
    public partial class Transaction
    {
        public Transaction()
        {
            QrCode = new HashSet<QrCode>();
        }

        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int ConsumerId { get; set; }
        public string Token { get; set; }
        public int Point { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TransactionTypeId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Consumer Consumer { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual ICollection<QrCode> QrCode { get; set; }
    }
}