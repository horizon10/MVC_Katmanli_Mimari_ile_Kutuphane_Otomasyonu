using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using MVC_Kutuphane_Otomasyonu.Entities.Repository;
using System.Collections.Generic;
using System.Linq;

namespace MVC_Kutuphane_Otomasyonu.Entities.DAL
{
    public class KitapTurleriDAL : GenericRepository<KutuphaneContext, KitapTurleri>
    {
        public List<TurIstatistikModel> GetKitapTuruIstatistik()
        {
            using (var context = new KutuphaneContext())
            {
                var toplam = context.Kitaplar.Count();

                var liste = context.Kitaplar
                    .GroupBy(k => k.KitapTurleri.KitapTuru)
                    .Select(g => new TurIstatistikModel
                    {
                        TurAdi = g.Key,
                        Adet = g.Count(),
                        Yuzde = (g.Count() * 100.0) / toplam
                    })
                    .ToList();

                return liste;
            }
        }
    }

    public class TurIstatistikModel
    {
        public string TurAdi { get; set; }
        public int Adet { get; set; }
        public double Yuzde { get; set; }
    }
}
