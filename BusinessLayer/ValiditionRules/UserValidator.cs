using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValiditonRules
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Boş Geçemezsiniz");
            RuleFor(x => x.UserName).MinimumLength(3).WithMessage("3 Karakterden Aşağı Olamaz");
            RuleFor(x => x.UserName).MaximumLength(50).WithMessage("50 Karakteri Geçemezsiniz");

            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Boş Geçemezsiniz");
            RuleFor(x => x.NameSurname).MinimumLength(3).WithMessage("3 Karakterden Aşağı Olamaz");
            RuleFor(x => x.NameSurname).MaximumLength(100).WithMessage("50 Karakteri Geçemezsiniz");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Boş Geçemezsiniz");
            RuleFor(x => x.Email).MinimumLength(3).WithMessage("3 Karakterden Aşağı Olamaz");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("50 Karakteri Geçemezsiniz");
        }
    }
}