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
    public class KullaniciHareketleriDAL:GenericRepository<KutuphaneContext,KullaniciHareketleri>
    {
        KutuphaneContext context =new KutuphaneContext();
        public object AylikVeriler;
        public object ToplamKHGSayisi;
        public object AltiAyToplamKHGSayisi;
        public(string KullaniciAdi,int GirisSayisi) KullaniciGirisSayilari()
        {
            var result = context.Set<KullaniciHareketleri>().GroupBy(x => new { x.KullaniciId, x.Kullanicilar.KullaniciAdi }).
                Select(y => new
                {
                    KullaniciAdi=y.Key.KullaniciAdi,
                    GirisSayisi=y.Count()
                }).OrderByDescending(x=>x.GirisSayisi).FirstOrDefault();
            if(result != null)
            {
                return (result.KullaniciAdi,result.GirisSayisi);
            }
            return (null, 0);//Varsayılan değer
        }
        public object KullaniciHareketleriGozleme()
        {
            DateTime altiAyOnce=DateTime.Now.AddMonths(-6);
            AylikVeriler = context.KullaniciHareketleri.Where(x => x.Tarih >= altiAyOnce).
                GroupBy(a => new
                {
                    Ay = a.Tarih.Month,
                    Yil = a.Tarih.Year,
                }).Select(b => new
                {
                    Ay = b.Key.Ay,
                    yil = b.Key.Yil,
                    HareketSay = b.Count()
                }).OrderBy(a => a.yil).ThenBy(y => y.Ay).ToList();
            ToplamKHGSayisi=context.KullaniciHareketleri.Count();
            AltiAyToplamKHGSayisi = context.KullaniciHareketleri.Count(x => x.Tarih >= altiAyOnce);
            return AylikVeriler;
        }
    }
}
