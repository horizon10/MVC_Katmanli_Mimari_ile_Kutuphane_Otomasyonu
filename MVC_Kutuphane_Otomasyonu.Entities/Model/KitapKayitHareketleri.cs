using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    [Validator(typeof(KitapKayitHareketleriValidatior))]
    public class KitapKayitHareketleri
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public int UyeId { get; set; }
        public int KitapId { get; set; }
        public string YapilanIslem { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }

        public Kitaplar Kitaplar { get; set; }
    }
}
