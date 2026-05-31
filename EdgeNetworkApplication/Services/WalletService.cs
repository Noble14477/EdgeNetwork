using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.Interface;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkApplication.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
        }

        public async Task<Wallet> CreateWalletAsync(Guid userId, string currency)
        {
            var wallet = Wallet.Create(userId, currency);
            await _walletRepository.AddAsync(wallet);
            await _unitOfWork.SaveChangesAsync();
            return wallet;
        }

        public async Task FundWalletAsync(FundWalletDto dto, Guid requestingUserId)
        {
            var wallet = await _walletRepository.GetByIdAsync(dto.WalletId);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found.");

            if (wallet.UserId != requestingUserId)
            {
                throw new InvalidOperationException("You are not authorized to fund this wallet.");
            }

            var amount = new Money(dto.Amount, dto.Currency);
            wallet.Credit(amount);

            // record the transaction
            var transaction = Transaction.Create(wallet.Id, amount, TransactionType.Credit, "Wallet funding");
            await _transactionRepository.AddAsync(transaction);

            _walletRepository.Update(wallet);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                transaction.MarkCompleted();
                _transactionRepository.Update(transaction);
                await _unitOfWork.SaveChangesAsync();

            }
            catch
            {
                transaction.MarkFailed();
                throw;
            }
        }

        public async Task TransferFundsAsync(TransferFundsDto dto, Guid requestingUserId)
        {
            var senderWallet = await _walletRepository.GetByIdAsync(dto.SenderWalletId);   
            if(senderWallet is null)
                throw new InvalidOperationException("Sender wallet not found");

            if (senderWallet.UserId != requestingUserId)
            {
                throw new InvalidOperationException("You are not authorized to transfer from this wallet.");
            }

            var receiverWallet = await _walletRepository.GetByIdAsync(dto.ReceiverWalletId);
            if(receiverWallet is null)
                throw new InvalidOperationException("Receiver wallet not found");

            var amount = new Money(dto.Amount, dto.Currency);

            senderWallet.Debit(amount);
            receiverWallet.Credit(amount);

            var debitTransaction = Transaction.Create(senderWallet.Id, amount, TransactionType.Debit, $"Transfer to {receiverWallet.Id}");
            var creditTransaction = Transaction.Create(receiverWallet.Id, amount, TransactionType.Credit, $"Transfer from {senderWallet.Id}");

            _walletRepository.Update(senderWallet);
            _walletRepository.Update(receiverWallet);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                debitTransaction.MarkCompleted();
                creditTransaction.MarkCompleted();
                _transactionRepository.Update(debitTransaction);
                _transactionRepository.Update(creditTransaction);
                await _unitOfWork.SaveChangesAsync();

            }
            catch {                 
                debitTransaction.MarkFailed();
                creditTransaction.MarkFailed();
                throw;
            }

        }

        public async Task<Wallet?> GetWalletAsync(Guid userId, Guid requestingUserId)
        {
            var wallet = await _walletRepository.GetByIdAsync(userId);
            if(wallet is null)
                throw new InvalidOperationException("Wallet not found");

            if(wallet.UserId != requestingUserId)
            {
                throw new InvalidOperationException("You are not authorized to view this wallet.");
            }

            return wallet;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionHistoryAsync(Guid walletId, Guid requestingUserId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found.");

            if (wallet.UserId != requestingUserId)
                throw new InvalidOperationException("You are not authorized to view these transactions.");

            var transactions = await _transactionRepository.GetByWalletIdAsync(walletId);

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount.Amount,
                Currency = t.Amount.Currency,
                Type = t.Type.ToString(),
                Status = t.Status.ToString(),
                Reference = t.Reference,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            });
        }
    }
}
