using System.Threading.Tasks;
using Bank.Entries.Core.DTOs;

namespace Bank.Entries.Core.Interfaces
{
    public interface ITransferService
    {
        Task<bool> InternalTransfer(TransferDTO internalTransferDTO);
    }
}