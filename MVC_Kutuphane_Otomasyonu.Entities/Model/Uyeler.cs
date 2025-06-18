using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    [Validator(typeof(UyelerValidator))]
    public class Uyeler
    {
        public int Id { get; set; }
        public string AdiSoyadi { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public string Email { get; set; }
        public string ResimYolu { get; set; }
        public int OkulKitapSayisi { get; set; }
        
        public DateTime KayıtTarihi { get; set; }

        public List<EmanetKitaplar> EmanetKitaplar { get; set; }

    }
}
