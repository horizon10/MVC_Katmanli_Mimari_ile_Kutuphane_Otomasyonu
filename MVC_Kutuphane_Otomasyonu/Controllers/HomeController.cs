using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Data.Entity;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        KutuphaneContext context = new KutuphaneContext();
        HakkimizdaDAL hakkimizdaDAL = new HakkimizdaDAL();
        IletisimDAL iletisimDAL = new IletisimDAL();

        public ActionResult Index(string arama, string yazar, string tur, int page = 1, int pageSize = 6)
        {
            var kitaplar = context.Kitaplar.Include("KitapTurleri").AsQueryable();

            if (!string.IsNullOrEmpty(arama))
                kitaplar = kitaplar.Where(x => x.KitapAdi.Contains(arama));

            if (!string.IsNullOrEmpty(yazar))
                kitaplar = kitaplar.Where(x => x.YazarAdi == yazar);

            if (!string.IsNullOrEmpty(tur))
                kitaplar = kitaplar.Where(x => x.KitapTurleri.KitapTuru == tur);

            var totalCount = kitaplar.Count();
            var sayfaSayisi = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.SayfaSayisi = sayfaSayisi;
            ViewBag.SuankiSayfa = page;

            var kitapListesi = kitaplar
                                .OrderBy(x => x.KitapAdi)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            return View(kitapListesi);
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
            var kitap = context.Kitaplar
                .Include(k => k.KitapTurleri)
                .Include(k => k.Yorumlar.Select(y => y.Uye))
                .FirstOrDefault(k => k.Id == id);

            if (kitap == null)
            {
                return HttpNotFound("Kitap bulunamadı.");
            }

            // Benzer kitapları getir (aynı türdeki kitaplar)
            ViewBag.BenzerKitaplar = context.Kitaplar
                .Include(k => k.KitapTurleri)
                .Where(k => k.KitapTurleri.Id == kitap.KitapTurleri.Id && k.Id != kitap.Id)
                .OrderByDescending(k => k.EklemeTarihi)
                .Take(4)
                .ToList();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YorumEkle(Yorumlar yorum, int kitapId)
        {
            if (Session["AktifUye"] == null)
            {
                TempData["ErrorMessage"] = "Yorum yapabilmek için giriş yapmalısınız.";
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                var uye = (Uyeler)Session["AktifUye"];

                yorum.UyeId = uye.Id;
                yorum.KitapId = kitapId;
                yorum.YorumTarihi = DateTime.Now;

                context.Yorumlar.Add(yorum);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Yorumunuz başarıyla eklendi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Yorum eklenirken bir hata oluştu. Lütfen tüm alanları doldurun.";
            }

            return RedirectToAction("KitapDetay", new { id = kitapId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YorumSil(int id)
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var yorum = context.Yorumlar
                .Include(y => y.Uye)
                .FirstOrDefault(y => y.Id == id);

            if (yorum == null)
            {
                TempData["ErrorMessage"] = "Yorum bulunamadı.";
                return RedirectToAction("Index");
            }

            var uye = (Uyeler)Session["AktifUye"];

            // Sadece yorum sahibi veya admin silebilir
            if (yorum.UyeId == uye.Id || User.IsInRole("Admin"))
            {
                context.Yorumlar.Remove(yorum);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Bu işlem için yetkiniz yok.";
            }

            return RedirectToAction("KitapDetay", new { id = yorum.KitapId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RezerveEt(int kitapId)
        {
            if (Session["AktifUye"] == null)
            {
                TempData["ErrorMessage"] = "Rezervasyon yapabilmek için giriş yapmalısınız.";
                return RedirectToAction("Login");
            }

            var kitap = context.Kitaplar.Find(kitapId);
            if (kitap == null)
            {
                return HttpNotFound("Kitap bulunamadı.");
            }

            if (kitap.StokAdedi <= 0)
            {
                TempData["ErrorMessage"] = "Bu kitap şu anda stokta bulunmamaktadır.";
                return RedirectToAction("KitapDetay", new { id = kitapId });
            }

            var uye = (Uyeler)Session["AktifUye"];

            // Aynı kitap için zaten rezervasyon var mı kontrolü
            var existingReservation = context.kitapRezervasyonlar
                .FirstOrDefault(r => r.KitapId == kitapId && r.UyeId == uye.Id && r.RezervasyonDurumu == true);

            if (existingReservation != null)
            {
                TempData["ErrorMessage"] = "Bu kitap için zaten rezervasyonunuz bulunmaktadır.";
                return RedirectToAction("KitapDetay", new { id = kitapId });
            }

            // Rezervasyon oluştur
            var rezervasyon = new KitapRezervasyonlar
            {
                KitapId = kitapId,
                UyeId = uye.Id,
                RezervasyonTarihi = DateTime.Now,
                RezervasyonSonTarihi = DateTime.Now.AddDays(3), // 3 gün rezervasyon süresi
                RezervasyonDurumu = true
            };

            context.kitapRezervasyonlar.Add(rezervasyon);
            kitap.StokAdedi -= 1; // Stoktan düş
            context.SaveChanges();

            TempData["SuccessMessage"] = "Kitap başarıyla rezerve edildi! 3 gün içinde kütüphaneden alabilirsiniz.";
            return RedirectToAction("KitapDetay", new { id = kitapId });
        }
        public ActionResult Rezervasyonlarim()
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var uye = (Uyeler)Session["AktifUye"];

            var rezervasyonlar = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .Where(r => r.UyeId == uye.Id && r.RezervasyonDurumu == true)
                .OrderBy(r => r.RezervasyonSonTarihi)
                .ToList();

            return View(rezervasyonlar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RezervasyonIptal(int id)
        {
            if (Session["AktifUye"] == null)
            {
                return RedirectToAction("Login");
            }

            var uye = (Uyeler)Session["AktifUye"];
            var rezervasyon = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .FirstOrDefault(r => r.Id == id && r.UyeId == uye.Id);

            if (rezervasyon == null)
            {
                TempData["ErrorMessage"] = "Rezervasyon bulunamadı veya iptal edilemez.";
                return RedirectToAction("Rezervasyonlarim");
            }

            try
            {
                // Kitap stoğunu artır
                rezervasyon.Kitap.StokAdedi += 1;

                // Rezervasyonu iptal et
                rezervasyon.RezervasyonDurumu = false;

                context.SaveChanges();

                TempData["SuccessMessage"] = "Rezervasyon başarıyla iptal edildi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İptal işlemi sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Rezervasyonlarim");
        }


    }
}