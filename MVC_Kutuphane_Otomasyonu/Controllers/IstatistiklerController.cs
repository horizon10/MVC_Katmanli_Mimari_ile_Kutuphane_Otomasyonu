using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    public class IstatistiklerController : Controller
    {
        KutuphaneContext context=new KutuphaneContext();
        KullaniciHareketleriDAL kullaniciHareketleriDAL=new KullaniciHareketleriDAL();
        KullanicilarDAL kullanicilarDAL=new KullanicilarDAL();
        UyelerDAL uyelerDAL=new UyelerDAL();
        // GET: Istatistikler
        public ActionResult Index()
        {
            //Üyeler
            uyelerDAL.UyeKitapIslemleri();
            ViewBag.EnCokUye = uyelerDAL.EnCokUye;
            ViewBag.EnAzUye = uyelerDAL.EnAzUye;
            ViewBag.EnCokSayi = uyelerDAL.EnCokSayi;
            ViewBag.EnAzSayi = uyelerDAL.EnAzSayi;
            ViewBag.UyeKitapModel= uyelerDAL.UyeKitapModel;

            //Kullanıcı Sayısı
            var KullaniciSayisiModel = kullanicilarDAL.GetAll(context);
            ViewBag.Count=KullaniciSayisiModel.Count;
            //En çok Giriş yapan KullanıcıAdı ve Giriş Sayısı
            var model = kullaniciHareketleriDAL.KullaniciGirisSayilari();
            ViewBag.KullaniciAdi=model.KullaniciAdi;
            ViewBag.GirisSayisi=model.GirisSayisi;
            return View();
        }
    }
}