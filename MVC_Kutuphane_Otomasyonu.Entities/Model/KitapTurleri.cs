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
    [Validator(typeof(KitapTurleriValidator))]
    public class KitapTurleri
    {
        public int Id { get; set; }
        [DisplayName("Kitap Türü")]
        public string KitapTuru { get; set; }
        [DisplayName("Açıklama")]
        public string Aciklama { get; set; }
        public List<Kitaplar> Kitaplar { get; set; }
    }
}
