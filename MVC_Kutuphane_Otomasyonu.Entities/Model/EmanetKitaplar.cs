using FluentValidation.Attributes;
using MVC_Kutuphane_Otomasyonu.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    [Validator(typeof(EmanetKitaplarValidatior))]
    public class EmanetKitaplar
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public int KitapId { get; set; }
        public int KitapSayisi { get; set; }
        public DateTime KitapAldigiTarihi { get; set; }
        public DateTime KitapIadeTarihi { get; set; }
        
        public Kitaplar Kitaplar {  get; set; }
        public Uyeler Uyeler { get; set; }
    }
}
