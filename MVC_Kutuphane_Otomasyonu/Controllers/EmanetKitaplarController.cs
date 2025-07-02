using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Mapping;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class EmanetKitaplarController : Controller
    {
        // GET: EmanetKitaplar
        KutuphaneContext context = new KutuphaneContext();
        EmanetKitaplarDAL emanetKitaplarDAL = new EmanetKitaplarDAL();
        KitaplarDAL kitaplarDAL = new KitaplarDAL();
        KitapHareketleriDAL kitapHareketleriDAL = new KitapHareketleriDAL();
        KitapRezervasyonlarDAL rezervasyonDAL = new KitapRezervasyonlarDAL();
        public ActionResult Index()
        {
            var model = emanetKitaplarDAL.GetAll(context, x => x.KitapIadeTarihi == null, "Kitaplar", "Uyeler");
            return View(model);
        }
        public ActionResult Yazdir()
        {
            var model = emanetKitaplarDAL.GetAll(context, x => x.KitapIadeTarihi == null, "Kitaplar", "Uyeler");
            return new Rotativa.ActionAsPdf("EmanetListesi", model)
            { 
                FileName="EmanetKitaplarListesi.pdf",
                PageSize=Rotativa.Options.Size.A4,
                PageOrientation=Rotativa.Options.Orientation.Portrait,
                CustomSwitches="--disable-smart-shrinking"
            };
        }
        public ActionResult EmanetListesi()
        {
            var model = emanetKitaplarDAL.GetAll(context, x => x.KitapIadeTarihi == null, "Kitaplar", "Uyeler");
            return View(model);
        }
        public ActionResult EmanetKitapVer()
        {
            var rezervasyonlar = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .Include(r => r.Uye)
                .Where(r => r.RezervasyonDurumu)
                .OrderBy(r => r.RezervasyonTarihi)
                .AsEnumerable() 
                .Select(r => new {
                    Value = r.Id,
                    Text = $"{r.Uye.AdiSoyadi} - {r.Kitap.KitapAdi} (Rezerve: {r.RezervasyonTarihi:dd.MM.yyyy})"
                }).ToList();

            ViewBag.RezervasyonListe = new SelectList(rezervasyonlar, "Value", "Text");
            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi");
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EmanetKitapVer(EmanetKitaplar entity, int? RezervasyonId)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //Rezervasyon varsa iptal ediliyor
                        if (RezervasyonId.HasValue)
                        {
                            var rezervasyon = context.kitapRezervasyonlar.Find(RezervasyonId.Value);
                            if (rezervasyon != null)
                            {
                                rezervasyon.RezervasyonDurumu = false;
                                context.Entry(rezervasyon).State = EntityState.Modified;
                            }
                        }

                        // Emanet kaydı oluşturuluyor
                        entity.KitapAldigiTarihi = DateTime.Now;
                        emanetKitaplarDAL.InsertorUpdate(context, entity);

                        // Kitap hareket kaydı
                        var email = User.Identity.Name;
                        var kullanici = context.Kullanicilar.FirstOrDefault(k => k.Email == email);

                        var kitapHareket = new KitapHareketleri
                        {
                            KullaniciId = kullanici.Id,
                            KitapId = entity.KitapId,
                            UyeId = entity.UyeId,
                            YapilanIslem = RezervasyonId.HasValue ?
                                          "Rezervasyondan Emanet" :
                                          "Normal Emanet",
                            Tarih = DateTime.Now,
                            RezervasyonId = RezervasyonId
                        };

                        context.KitapHareketler.Add(kitapHareket);
                        emanetKitaplarDAL.Save(context);
                        transaction.Commit();

                        TempData["SuccessMessage"] = "Kitap başarıyla emanet verildi" +
                            (RezervasyonId.HasValue ? " (Rezervasyon iptal edildi)" : "");
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TempData["ErrorMessage"] = "Hata oluştu: " + ex.Message;
                    }
                }
            }

            // Hata durumunda formu tekrar göster
            ViewBag.RezervasyonListe = new SelectList(
                context.kitapRezervasyonlar
                    .Include(r => r.Kitap)
                    .Include(r => r.Uye)
                    .Where(r => r.RezervasyonDurumu)
                    .OrderBy(r => r.RezervasyonTarihi)
                    .Select(r => new {
                        Value = r.Id,
                        Text = $"{r.Uye.AdiSoyadi} - {r.Kitap.KitapAdi} (Rezerve Tarihi: {r.RezervasyonTarihi.ToString("dd.MM.yyyy")}"
                    }), "Value", "Text", RezervasyonId);

            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi", entity.UyeId);
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi", entity.KitapId);
            return View(entity);
        }
        public ActionResult Duzenle(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi");
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi");
            var model=emanetKitaplarDAL.GetByFilter(context,x => x.Id == id,"Uyeler","Kitaplar");
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Duzenle(EmanetKitaplar entity)
        {
            if (ModelState.IsValid)
            {
                emanetKitaplarDAL.InsertorUpdate(context, entity);
                emanetKitaplarDAL.Save(context);
                return RedirectToAction("Index");
            }
            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi");
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi");
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            emanetKitaplarDAL.Delete(context, x => x.Id == id);
            emanetKitaplarDAL.Save(context);
            return RedirectToAction("Index");
        }
        public ActionResult TeslimAl(int? id) 
        {
            var model=emanetKitaplarDAL.GetByFilter(context, x => x.Id == id);
            model.KitapIadeTarihi = DateTime.Now;

            var kitaplar=kitaplarDAL.GetByFilter(context,x=> x.Id == model.KitapId);
            kitaplar.StokAdedi=kitaplar.StokAdedi+model.KitapSayisi;
            emanetKitaplarDAL.Save(context);
            return RedirectToAction("Index");
        }
        public JsonResult GetRezervasyonDetay(int rezervasyonId)
        {
            var rezervasyon = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .Include(r => r.Uye)
                .FirstOrDefault(r => r.Id == rezervasyonId);

            if (rezervasyon == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                UyeId = rezervasyon.UyeId,
                KitapId = rezervasyon.KitapId,
                KitapAdi = rezervasyon.Kitap.KitapAdi,
                UyeAdi = rezervasyon.Uye.AdiSoyadi,
                RezervasyonTarihi = rezervasyon.RezervasyonTarihi.ToString("dd.MM.yyyy HH:mm")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
