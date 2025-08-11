using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Features.Commands.CreateTransaction
{
    public class CreateTransactionsValidator : AbstractValidator<CreateTransactionsCommand>
    {
        public CreateTransactionsValidator() {
            RuleFor(x => x.TransactionRequest.Pan)
                   .NotEmpty().WithMessage("El nombre no puede estar vacío")
                   .MaximumLength(19).WithMessage("El nombre no puede superar los 19 caracteres"); // Según la norma ISO/IEC 7812, el PAN puede tener hasta 19 dígitos
            RuleFor(x => x.TransactionRequest.Expiry)
                     .NotEmpty().WithMessage("La fecha de expiración no puede estar vacía")
                     .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$").WithMessage("El formato de la fecha de expiración debe ser MM/AA"); // Formato MM/AA
            RuleFor(x => x.TransactionRequest.Amount)
                        .GreaterThan(0).WithMessage("El monto debe ser mayor que cero");
            RuleFor(x => x.TransactionRequest.Currency)
                        .NotEmpty().WithMessage("La moneda no puede estar vacía")
                        .Length(3).WithMessage("La moneda debe tener 3 caracteres"); // Código ISO 4217 de 3 letras
            RuleFor(x => x.TransactionRequest.Cvv)
                        .NotEmpty().WithMessage("El CVV no puede estar vacío")
                        .Matches(@"^\d{3,4}$").WithMessage("El CVV debe tener 3 o 4 dígitos"); // CVV de 3 o 4 dígitos
            RuleFor(x => x.TransactionRequest.MerchantId)
                        .NotEmpty().WithMessage("El ID del comerciante no puede estar vacío")
                        .MaximumLength(50).WithMessage("El ID del comerciante no puede superar los 50 caracteres");
        }
        
    }
}
