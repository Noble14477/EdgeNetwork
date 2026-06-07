using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Common;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Interface;
using EdgeNetworkDomain.Common;
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
            var sender = await _walletRepository.GetByIdAsync(dto.SenderWalletId);
            if (sender is null)
                throw new InvalidOperationException("Sender wallet not found");

            if (sender.UserId != requestingUserId)
            {
                throw new InvalidOperationException("You are not authorized to transfer from this wallet.");
            }

            var receiver = await _walletRepository.GetByAccountNumberAsync(dto.ReceiverAccountNumber);
            if(receiver is null)
                throw new InvalidOperationException("Receiver's account number not found");

            if (sender.AccountNumber.Value == dto.ReceiverAccountNumber)
                throw new InvalidDataException("You cannot transfer to your own wallet");

            var amount = new Money(dto.Amount, dto.Currency);

            sender.Debit(amount);
            receiver.Credit(amount);

            var debitTransaction = Transaction.Create(sender.Id, amount, TransactionType.Debit, $"Transfer to {dto.ReceiverAccountNumber}");
            var creditTransaction = Transaction.Create(receiver.Id, amount, TransactionType.Credit, $"Transfer from {sender.AccountNumber.Value}");

            debitTransaction.MarkCompleted();
            creditTransaction.MarkCompleted();

            await _transactionRepository.AddAsync(debitTransaction);
            await _transactionRepository.AddAsync(creditTransaction);

            _walletRepository.Update(sender);
            _walletRepository.Update(receiver);


       

            

            try
            {
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
                Amount = t.Amount,
                Currency = t.Currency,
                Type = t.Type.ToString(),
                Status = t.Status.ToString(),
                Reference = t.Reference,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            });
        }
           
        public async Task<PagedResult<TransactionDto>> GetTransactionHistoryAsync( Guid walletId, Guid requestingUserId, TransactionFilterDto filter)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found.");

            if (wallet.UserId != requestingUserId)
                throw new InvalidOperationException("You are not authorized to view these transactions.");
            
            var domainFilter = new TransactionFilter
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Type = filter.Type,
                Status = filter.Status,
                DateFrom = filter.DateFrom,
                DateTo = filter.DateTo
            };

            var (items, totalCount) = await _transactionRepository.GetFilteredAsync(walletId, domainFilter);

            var dtos = items.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Currency = t.Currency,
                Type = t.Type.ToString(),
                Status = t.Status.ToString(),
                Reference = t.Reference,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            });

            return new PagedResult<TransactionDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

    }

}
