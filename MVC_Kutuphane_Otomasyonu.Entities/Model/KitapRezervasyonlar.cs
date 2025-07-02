using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    public class KitapRezervasyonlar
    {
        public int Id { get; set; }

        [Required]
        public int KitapId { get; set; }
        public virtual Kitaplar Kitap { get; set; }

        [Required]
        public int UyeId { get; set; }
        public virtual Uyeler Uye { get; set; }

        [Required]
        public DateTime RezervasyonTarihi { get; set; }

        [Required]
        public DateTime RezervasyonSonTarihi { get; set; }

        [Required]
        public bool RezervasyonDurumu { get; set; } // true: aktif, false: pasif
    }
}
