using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        KutuphaneContext context = new KutuphaneContext();
        HakkimizdaDAL hakkimizdaDAL = new HakkimizdaDAL();
        IletisimDAL iletisimDAL = new IletisimDAL();

        public ActionResult Index()
        {
            try
            {
                var kitaplar = context.Kitaplar.Include("KitapTurleri").ToList();
                if (kitaplar == null || !kitaplar.Any())
                {
                    kitaplar = new List<Kitaplar>();
                }
                return View(kitaplar);
            }
            catch (Exception)
            {
                return View(new List<Kitaplar>());
            }
        }

        public ActionResult About()
        {
            var model = hakkimizdaDAL.GetAll(context);
            return View(model);
        }

        public ActionResult Duyurular()
        {
            var duyurular = context.Duyurular.OrderByDescending(x => x.Tarih).ToList();
            return View(duyurular);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Contact(Iletisim model)
        {
            if (ModelState.IsValid)
            {
                model.Tarih = DateTime.Now;
                iletisimDAL.InsertorUpdate(context, model);
                iletisimDAL.Save(context);
                TempData["Message"] = "Mesajınız başarıyla gönderildi.";
                return RedirectToAction("Contact");
            }
            return View(model);
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string sifre)
        {
            // Önce üyeleri kontrol et
            var uye = context.Uyeler.FirstOrDefault(x => x.Email == email && x.Sifre == sifre);
            if (uye != null)
            {
                Session["AktifUye"] = uye;
                Session["AktifKullanici"] = null;

                // Giriş yapılan kişi admin değilse auth cookie oluşturulmaz
                // Ama bu kişi sadece üyedir, admin yetkisi gerekmez
                return RedirectToAction("Index");
            }

            // Kullanıcı kontrolü (admin olabilir)
            var kullanici = context.Kullanicilar.FirstOrDefault(x => x.Email == email && x.Sifre == sifre);
            if (kullanici != null)
            {
                Session["AktifKullanici"] = kullanici;
                Session["AktifUye"] = null;

                // Burada admin için kimlik doğrulama yap (rollerde kullanılacak)
                FormsAuthentication.SetAuthCookie(kullanici.Email, false); // "false" -> beni hatırla devre dışı

                return RedirectToAction("Index");
            }

            ViewBag.Hata = "Email veya şifre hatalı.";
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Uyeler model, HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid)
            {
                if (Resim != null && Resim.ContentLength > 0)
                {
                    string dosyaAdi = Path.GetFileName(Resim.FileName);
                    string yol = Path.Combine(Server.MapPath("~/images"), dosyaAdi);
                    Resim.SaveAs(yol);
                    model.ResimYolu = "/images/" + dosyaAdi;
                }

                model.KayıtTarihi = DateTime.Now;
                context.Uyeler.Add(model);
                context.SaveChanges();

                TempData["Mesaj"] = "Kayıt başarılı. Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult KitapDetay(int id)
        {
            if (Session["AktifUye"] == null)
            {
                TempData["LoginMessage"] = "Kitap detaylarını görmek için giriş yapmalısınız.";
                return RedirectToAction("Login");
            }

            var kitap = context.Kitaplar.Include("KitapTurleri").FirstOrDefault(k => k.Id == id);
            if (kitap == null)
                return HttpNotFound("Kitap bulunamadı.");

            return View(kitap);
        }

        [HttpGet]
        public ActionResult Profil()
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var uye = (Uyeler)Session["AktifUye"];
            var model = context.Uyeler.FirstOrDefault(x => x.Id == uye.Id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Profil(Uyeler guncellenenUye, HttpPostedFileBase Resim, string YeniSifre)
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var uye = (Uyeler)Session["AktifUye"];
            var dbUye = context.Uyeler.FirstOrDefault(x => x.Id == uye.Id);

            if (dbUye == null)
            {
                TempData["Mesaj"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Profil");
            }

            if (!string.IsNullOrWhiteSpace(guncellenenUye.AdiSoyadi))
                dbUye.AdiSoyadi = guncellenenUye.AdiSoyadi;

            if (!string.IsNullOrWhiteSpace(guncellenenUye.Telefon))
                dbUye.Telefon = guncellenenUye.Telefon;

            if (!string.IsNullOrWhiteSpace(guncellenenUye.Adres))
                dbUye.Adres = guncellenenUye.Adres;

            if (!string.IsNullOrWhiteSpace(YeniSifre))
            {
                dbUye.Sifre = YeniSifre;
            }

            if (Resim != null && Resim.ContentLength > 0)
            {
                string uzanti = Path.GetExtension(Resim.FileName);
                string dosyaAdi = Guid.NewGuid() + uzanti;
                string kayitYolu = Server.MapPath("~/images/" + dosyaAdi);
                Resim.SaveAs(kayitYolu);
                dbUye.ResimYolu = "/images/" + dosyaAdi;
            }

            context.SaveChanges();

            Session["AktifUye"] = dbUye;

            TempData["Mesaj"] = "Bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Profil");
        }
        public ActionResult Emanetlerim()
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var uye = (Uyeler)Session["AktifUye"];

            var emanetler = context.EmanetKitaplar
                .Include("Kitaplar")
                .Where(x => x.UyeId == uye.Id && x.KitapIadeTarihi == null)
                .ToList();

            return View(emanetler); // ViewBag değil, model olarak gönderiyoruz
        }


    }
}