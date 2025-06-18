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
    
    public class KitaplarValidator:AbstractValidator<Kitaplar>
    {
        public KitaplarValidator()
        {
            RuleFor(x => x.BarkodNo).NotEmpty().WithMessage("BarkodNo alanı boş geçilemez.");
            RuleFor(x => x.KitapTuruId).NotEmpty().WithMessage("Kitap Türü alanı boş geçilemez.");
            RuleFor(x => x.BarkodNo).MaximumLength(30).WithMessage("BarkodNo alanı en fazla 30 karakter olabilir.");

            RuleFor(x => x.KitapAdi).NotEmpty().WithMessage("KitapAdi alanı boş geçilemez.");
            RuleFor(x => x.KitapAdi).MaximumLength(100).WithMessage("KitapAdi alanı en fazla 100 karakter olabilir.");
            RuleFor(x => x.YazarAdi).NotEmpty().WithMessage("YazarAdi alanı boş geçilemez.");
            RuleFor(x => x.YazarAdi).MaximumLength(100).WithMessage("YazarAdi alanı en fazla 100 karakter olabilir.");
            RuleFor(x => x.YayinEvi).NotEmpty().WithMessage("Yayinevi alanı boş geçilemez.");
            RuleFor(x => x.YayinEvi).MaximumLength(150).WithMessage("Yayinevi alanı en fazla 150 karakter olabilir.");
        }
    }
}
