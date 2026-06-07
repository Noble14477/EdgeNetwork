using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using FluentValidation;

namespace EdgeNetworkApplication.Validators
{
    public class TransferFundsDtoValidator : AbstractValidator<TransferFundsDto>
    {
        public TransferFundsDtoValidator()
        {
            RuleFor(x => x.SenderWalletId)
                .NotEmpty().WithMessage("Sender wallet ID is required.");

            RuleFor(x => x.ReceiverAccountNumber)
                .NotEmpty().WithMessage("Receiver Account Number is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter code e.g. NGN, USD.");
        }
    }
}
