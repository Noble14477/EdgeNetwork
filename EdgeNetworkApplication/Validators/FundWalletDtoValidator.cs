using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using FluentValidation;

namespace EdgeNetworkApplication.Validators
{
    public class FundWalletDtoValidator : AbstractValidator<FundWalletDto>
    {
        public FundWalletDtoValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("Wallet ID is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Wallet ID format.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter ISO code.");
        }
    }
}
