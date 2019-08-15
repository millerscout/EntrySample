using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Core.DTOs
{
    public class TransferDTO
    {
        public int SenderBranch { get; set; }
        public int SenderNumber { get; set; }
        public int SenderDigit { get; set; }

        public int ReceiverBranch { get; set; }
        public int ReceiverNumber { get; set; }
        public int ReceiverDigit { get; set; }

        public decimal Value { get; set; }
    }
}
