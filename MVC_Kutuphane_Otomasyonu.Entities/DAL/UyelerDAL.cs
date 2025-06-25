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
    public class UyelerDAL:GenericRepository<KutuphaneContext,Uyeler>
    {
        KutuphaneContext context=new KutuphaneContext();
        public string EnCokUye;
        public string EnAzUye;
        public int EnCokSayi;
        public int EnAzSayi;

        public object UyeKitapModel;
        public object UyeKitapIslemleri()
        {
            EnCokUye = context.Uyeler.OrderByDescending(x => x.OkulKitapSayisi).FirstOrDefault().AdiSoyadi;
            EnAzUye= context.Uyeler.OrderBy(x => x.OkulKitapSayisi).FirstOrDefault().AdiSoyadi;

            EnCokSayi = context.Uyeler.OrderByDescending(x => x.OkulKitapSayisi).FirstOrDefault().OkulKitapSayisi;
            EnAzSayi=context.Uyeler.Min(x=>x.OkulKitapSayisi);

            double ToplamKitap = context.Uyeler.Sum(x => x.OkulKitapSayisi);
            UyeKitapModel = context.Uyeler.OrderByDescending(x => x.OkulKitapSayisi).Take(3).
                Select(x => new
                {
                    x.AdiSoyadi,
                    x.OkulKitapSayisi,
                    Yuzde=(x.OkulKitapSayisi*100)/ToplamKitap
                }).ToList();
            return UyeKitapModel;
        }
    }
}
