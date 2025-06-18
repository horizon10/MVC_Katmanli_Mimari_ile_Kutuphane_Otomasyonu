using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    [Validator(typeof(HakkimizdaValidatior))]
    public class Hakkimizda
    {
        public int Id { get; set; }
        public string Icerik { get; set; }
        public string Aciklama { get; set; }
    }
}
