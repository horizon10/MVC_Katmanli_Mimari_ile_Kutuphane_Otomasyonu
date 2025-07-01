using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Mapping;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
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
        KitapHareketleriDAL kitapHareketleriDAL=new KitapHareketleriDAL();
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
            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi");
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EmanetKitapVer(EmanetKitaplar entity)
        {
            if (ModelState.IsValid)
            {
                var email=User.Identity.Name;
                var modelKullanici = context.Kullanicilar.FirstOrDefault(k => k.Email == email);
                emanetKitaplarDAL.InsertorUpdate(context,entity);
                var kitapHareket = new KitapHareketleri
                {
                    KullaniciId = modelKullanici.Id,
                    KitapId = entity.KitapId,
                    UyeId = entity.UyeId,
                    YapilanIslem="Emanet Kitap İşlemi",
                    Tarih=DateTime.Now,
                };
                emanetKitaplarDAL.Save(context);
                return RedirectToAction("Index");
            }
            ViewBag.UyeListe = new SelectList(context.Uyeler, "Id", "AdiSoyadi");
            ViewBag.KitapListe = new SelectList(context.Kitaplar, "Id", "KitapAdi");
            return RedirectToAction("Index");
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
    }
}
