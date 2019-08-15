using Bank.Entries.Core.DTOs;
using Bank.Entries.Core.Interfaces;
using Bank.Entries.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entries.Core
{
    public class TransferService 
    {
        private ITransferRepository transferRepository;

        public TransferService(ITransferRepository transferRepository)
        {
            this.transferRepository = transferRepository;
        }
        public async Task<bool> InternalTransfer(TransferDTO internalTransferDTO)
        {
            try
            {
                var sender = await transferRepository.GetBankAccount(internalTransferDTO.SenderBranch, internalTransferDTO.SenderNumber, internalTransferDTO.SenderDigit);

                var receiver = await transferRepository.GetBankAccount(internalTransferDTO.ReceiverBranch, internalTransferDTO.ReceiverNumber, internalTransferDTO.ReceiverDigit);

                if (!SenderHasFunds(sender, internalTransferDTO.Value)) return UserDoesNotHaveFunds();

                if (receiver==null) return ReceiverAccountNotFound();

                var result = await transferRepository.Transfer(internalTransferDTO);

                NotifyConsumers(internalTransferDTO);

                return true;
            }

            catch (Exception ex)
            {
                NotifyErrorToConsumers(ex);
                return false;
            }

        }

        private bool SenderHasFunds(BankAccount sender, decimal amount) => sender.Balance > amount;

        private void NotifyErrorToConsumers(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void NotifyConsumers(TransferDTO internalTransferDTO)
        {
            throw new NotImplementedException();
        }

        private bool ReceiverAccountNotFound()
        {
            throw new NotImplementedException();
        }

        private bool UserDoesNotHaveFunds()
        {
            throw new NotImplementedException();
        }
    }
}
