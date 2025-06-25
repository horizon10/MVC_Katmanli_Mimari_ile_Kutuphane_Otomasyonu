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
    }
}
