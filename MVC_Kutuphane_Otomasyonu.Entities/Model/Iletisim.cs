using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    [Validator(typeof(IletisimValidator))]
    public class Iletisim
    {
        public int Id { get; set; }
        public string AdiSoyadi { get; set; }
        public string Email { get; set; }
        public string Baslik { get; set; }
        public string Mesaj { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
    }
}
