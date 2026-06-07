using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkApplication.Interface
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(Guid userId, string currency);
        Task FundWalletAsync(FundWalletDto dto, Guid requestingUserId);
        Task TransferFundsAsync(TransferFundsDto dto, Guid requestingUserId);
        Task<Wallet?> GetWalletAsync(Guid userId, Guid requestingUserId);
        Task<PagedResult<TransactionDto>> GetTransactionHistoryAsync(
            Guid walletId,
            Guid requestingUserId,
            TransactionFilterDto filter);
    }
}
