using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkApplication.Interface
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(Guid userId, string currency);
        Task FundWalletAsync(FundWalletDto dto);
        Task TransferFundsAsync(TransferFundsDto dto);
        Task<Wallet?> GetWalletAsync(Guid userId);
    }
}
