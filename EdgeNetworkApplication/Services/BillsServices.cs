using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos.Bills;
using EdgeNetworkApplication.Interface;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.Enums.Bills;
using EdgeNetworkDomain.Interface;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkApplication.Services
{
    public class BillsServices : IBillsServices
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IBillsRepository _billsRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillsServices(IWalletRepository walletRepository, IBillsRepository billsRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _billsRepository = billsRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PurchaseAirtimeAsync(PurchaseAirtimeDto dto, Guid requestingUserId)
        {
            var wallet = await _walletRepository.GetByIdAsync(dto.WalletId);
            if (wallet is null)
            {
                throw new InvalidOperationException("Wallet not found");
            }

            if (wallet.UserId != requestingUserId)
            {
                throw new InvalidOperationException("Ypu are not authorized to use this wallet");
            }

            var amount = new Money(dto.Amount, dto.Currency);

            wallet.Debit(amount);
            _walletRepository.Update(wallet);

            var billPayment = BillPayment.Create(
                wallet.Id,
                BillType.Airtime,
                dto.Provider,
                dto.PhoneNumber,
                amount

                );

            var transaction = Transaction.Create(
                
                    wallet.Id,
                    amount,
                    TransactionType.Debit,
                    $"Airtime purchase for {dto.PhoneNumber} on {dto.Provider}"
                );

            await _billsRepository.AddAsync(billPayment);
            await _transactionRepository.AddAsync(transaction);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                billPayment.MarkCompleted();
                transaction.MarkCompleted();
                _billsRepository.Update(billPayment);
                _transactionRepository.Update(transaction);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                billPayment.MarkFailed();
                transaction.MarkFailed();
                throw;
            }
        }
    
        public async Task PurchaseDataAsync(PurchaseDataDto dto, Guid requestingUserId)
        {
            var wallet = await _walletRepository.GetByIdAsync(dto.WalletId);
            if(wallet is null)
            {
                throw new InvalidOperationException("Wallet not found");
            }

            if(wallet.UserId != requestingUserId)
            {
                throw new InvalidOperationException("You are not authorized to use this wallet");
            }

            var plan = DataPlans.GetPlan(dto.PlanId);
            if (plan is null)
            {
                throw new InvalidOperationException("Invalid data plan selected");
            }

            var amount = new Money(plan.Price, dto.Currency);

            wallet.Debit(amount);
            _walletRepository.Update(wallet);

            var billPayment = BillPayment.Create(
                    wallet.Id,
                    BillType.Data,
                    dto.Provider,
                    dto.PhoneNumber,
                    amount,
                    dto.PlanId
                
                );

            var transaction = Transaction.Create(
                    wallet.Id,
                    amount,
                    TransactionType.Debit,
                    $"{plan.Description} data purchase for {dto.PhoneNumber} on {dto.Provider}"
                );

            await _billsRepository.AddAsync(billPayment);
            await _transactionRepository.AddAsync(transaction);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                billPayment.MarkCompleted();
                transaction.MarkCompleted();
                _billsRepository.Update(billPayment);
                _transactionRepository.Update(transaction);
                await _unitOfWork.SaveChangesAsync();

            }
            catch
            {
                billPayment.MarkFailed();
                transaction.MarkFailed();
                throw;
            }
        }


        public IEnumerable<DataPlan> GetDataPlans() => DataPlans.Plans;
    }
}
