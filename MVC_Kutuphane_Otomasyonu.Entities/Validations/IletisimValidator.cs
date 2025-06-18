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
    
    public class IletisimValidator:AbstractValidator<Iletisim>
    {
        public IletisimValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı boş geçilemez.");
            RuleFor(x => x.Email).MaximumLength(150).WithMessage("İçerik alanı en fazla 150 karakter olabilir.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Lütfen bir mail adresi formatı giriniz.");
            ////

            RuleFor(x => x.AdiSoyadi).NotEmpty().WithMessage("Adı Soyadı alanı boş geçilemez.");
            RuleFor(x => x.AdiSoyadi).MaximumLength(100).WithMessage("Adı Soyadı alanı en fazla 100 karakter olabilir.");
            RuleFor(x => x.Baslik).NotEmpty().WithMessage("Başlık alanı boş geçilemez");
            RuleFor(x => x.Baslik).MaximumLength(200).WithMessage("Başlık alanı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Mesaj).NotEmpty().WithMessage("Mesaj alanı boş geçilemez.");
            RuleFor(x => x.Mesaj).MaximumLength(500).WithMessage("Mesaj alanı en fazla 500 karakter olabilir.");

        }

    }
}
