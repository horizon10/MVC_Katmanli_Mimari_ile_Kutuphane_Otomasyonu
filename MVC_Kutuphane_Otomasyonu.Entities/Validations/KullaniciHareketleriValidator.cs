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
    
    public class KullaniciHareketleriValidator:AbstractValidator<KullaniciHareketleri>
    {
        public KullaniciHareketleriValidator() 
        {

        }
    }
}
