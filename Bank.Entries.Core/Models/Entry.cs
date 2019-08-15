using Bank.Entries.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Core.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int ReceiverBankAccountId { get; set; }
        public EntryType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }

    }
}
