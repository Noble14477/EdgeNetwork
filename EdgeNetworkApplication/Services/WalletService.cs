using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkApplication.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Wallet> CreateWalletAsync(Guid userId, string currency)
        {
            var wallet = Wallet.Create(userId, currency);
            await _walletRepository.AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();
            return wallet;
        }

        public async Task FundWalletAsync(FundWalletDto dto)
        {
            var wallet = await _walletRepository.GetByIdAsync(dto.WalletId);
            if(wallet is null)
                throw new InvalidOperationException("Wallet not found");

            var amount = new Money(dto.Amount, dto.Currency);
            wallet.Credit(amount);

            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync();   
        }

        public async Task TransferFundsAsync(TransferFundsDto dto)
        {
            var senderWallet = await _walletRepository.GetByIdAsync(dto.SenderWalletId);   
            if(senderWallet is null)
                throw new InvalidOperationException("Sender wallet not found");

            var receiverWallet = await _walletRepository.GetByIdAsync(dto.ReceiverWalletId);
            if(receiverWallet is null)
                throw new InvalidOperationException("Receiver wallet not found");

            var amount = new Money(dto.Amount, dto.Currency);

            senderWallet.Debit(amount);
            receiverWallet.Credit(amount);

            _walletRepository.Update(senderWallet);
            _walletRepository.Update(receiverWallet);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Wallet?> GetWalletAsync(Guid userId)
        {
            return await _walletRepository.GetByIdAsync(userId);
        }
    }
}
