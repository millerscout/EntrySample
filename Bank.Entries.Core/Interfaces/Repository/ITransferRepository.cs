using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Entries.Core.DTOs;
using Bank.Entries.Core.Models;

namespace Bank.Entries.Core.Interfaces
{
    public interface ITransferRepository
    {
        Task<bool> Transfer(TransferDTO internalTransferDTO);
        Task<BankAccount> GetBankAccount(int branch, int number, int digit);
    }
}
