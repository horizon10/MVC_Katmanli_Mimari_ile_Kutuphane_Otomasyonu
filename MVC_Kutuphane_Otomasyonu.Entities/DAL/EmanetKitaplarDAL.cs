using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using MVC_Kutuphane_Otomasyonu.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.DAL
{
    using System.Collections.Generic;

    public class EmanetKitaplarDAL : GenericRepository<KutuphaneContext, EmanetKitaplar>
    {
        KutuphaneContext context = new KutuphaneContext();

        public (string KitapAdi, string KitapTuru, int Toplam) EnCokEmanetEdilenKitap()
        {
            var result = context.EmanetKitaplar
                .GroupBy(x => new { x.Kitaplar.KitapAdi, x.Kitaplar.KitapTurleri.KitapTuru })
                .Select(g => new {
                    KitapAdi = g.Key.KitapAdi,
                    KitapTuru = g.Key.KitapTuru,
                    Toplam = g.Sum(x => x.KitapSayisi)
                })
                .OrderByDescending(x => x.Toplam)
                .FirstOrDefault();

            return result != null ? (result.KitapAdi, result.KitapTuru, result.Toplam) : ("Yok", "Yok", 0);
        }

        public (string KitapAdi, string KitapTuru, int Toplam) EnAzEmanetEdilenKitap()
        {
            var result = context.EmanetKitaplar
                .GroupBy(x => new { x.Kitaplar.KitapAdi, x.Kitaplar.KitapTurleri.KitapTuru })
                .Select(g => new {
                    KitapAdi = g.Key.KitapAdi,
                    KitapTuru = g.Key.KitapTuru,
                    Toplam = g.Sum(x => x.KitapSayisi)
                })
                .OrderBy(x => x.Toplam)
                .FirstOrDefault();

            return result != null ? (result.KitapAdi, result.KitapTuru, result.Toplam) : ("Yok", "Yok", 0);
        }
    }


}
