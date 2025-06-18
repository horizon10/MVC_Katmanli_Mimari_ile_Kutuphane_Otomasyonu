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
    
    public class KitapTurleriValidator:AbstractValidator<KitapTurleri>
    {
        public KitapTurleriValidator() 
        {
            RuleFor(x => x.KitapTuru).NotEmpty().WithMessage("Kitap Türü alanı boş geçilemez");
            RuleFor(x => x.KitapTuru).MinimumLength(5).WithMessage("Kitap Türü alanı 5 karakterden az olmamalıdır.");
            RuleFor(x => x.KitapTuru).MaximumLength(150).WithMessage("KitapTurleri alanı en fazla 150 karakter olabilir.");

        }
    }
}
