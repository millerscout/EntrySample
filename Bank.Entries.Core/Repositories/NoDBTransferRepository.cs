using Bank.Entries.Core.DTOs;
using Bank.Entries.Core.Interfaces;
using Bank.Entries.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entries.Core.Repositories
{
    public class NoDBTransferRepository : ITransferRepository
    {
        public NoDBTransferRepository()
        {
            Database = new List<BankAccount>() {
                new BankAccount(1, 1, 2, 1, 1000),
                new BankAccount(2, 1, 3, 1, 0)
            };
        }

        public List<BankAccount> Database { get; }

        public Task<BankAccount> GetBankAccount(int branch, int number, int digit)
        {
            //var query = "select Id, Branch, Number, Digit, Balance from BankAccount where branch = @branch and number = @number and digit = @digit";


            return Task.FromResult(Database.FirstOrDefault(q => q.Branch == branch && q.Number == number && q.Digit == digit));
        }

        public async Task<bool> Transfer(TransferDTO internalTransferDTO)
        {
            var receiver = await GetBankAccount(internalTransferDTO.ReceiverBranch, internalTransferDTO.ReceiverNumber, internalTransferDTO.ReceiverDigit);
            var sender = await GetBankAccount(internalTransferDTO.SenderBranch, internalTransferDTO.SenderNumber, internalTransferDTO.SenderDigit);

            receiver.UpdateBalance(internalTransferDTO.Value);
            sender.UpdateBalance(internalTransferDTO.Value * -1);

            return true;

        }
    }
}
