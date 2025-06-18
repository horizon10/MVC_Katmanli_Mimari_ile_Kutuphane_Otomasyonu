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
    
    public class HakkimizdaValidatior:AbstractValidator<Hakkimizda>
    {
        public HakkimizdaValidatior() 
        {
            RuleFor(x => x.Icerik).NotEmpty().WithMessage("İçerik alanı boş geçilemez.");
            RuleFor(x => x.Icerik).MaximumLength(500).WithMessage("İçerik alanı en fazla 500 karakter olabilir.");
        }
    }
}
