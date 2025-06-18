using FluentValidation;
using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Validation
{
    
    public class RollerValidator:AbstractValidator<Roller>
    {
        public RollerValidator()
        {

            RuleFor(x => x.Rol).NotEmpty().WithMessage("Rol alanı boş geçilemez.");
            RuleFor(x => x.Rol).MaximumLength(50).WithMessage("Rol alanı en fazla 50 karakter olabilir.");
            
        }
    }
}
