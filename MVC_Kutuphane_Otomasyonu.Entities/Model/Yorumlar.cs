using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model
{
    public class Yorumlar
    {
        public int Id { get; set; }

        [Required]
        public string Icerik { get; set; }

        public DateTime YorumTarihi { get; set; } = DateTime.Now;

        // İlişkiler
        public int KitapId { get; set; }
        public virtual Kitaplar Kitap { get; set; }

        public int UyeId { get; set; }
        public virtual Uyeler Uye { get; set; }
    }
}
