using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RezervasyonlarController : Controller
    {
        KutuphaneContext context =new KutuphaneContext();
        // GET: Rezervasyonlar
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Rezervasyonlar()
        {
            var rezervasyonlar = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .Include(r => r.Uye)
                .OrderByDescending(r => r.RezervasyonTarihi)
                .ToList();

            return View(rezervasyonlar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RezervasyonIptal(int id)
        {
            var rezervasyon = context.kitapRezervasyonlar
                .Include(r => r.Kitap)
                .FirstOrDefault(r => r.Id == id);

            if (rezervasyon != null)
            {
                // Kitap stoğunu artır
                rezervasyon.Kitap.StokAdedi += 1;

                // Rezervasyonu iptal et
                rezervasyon.RezervasyonDurumu = false;

                context.SaveChanges();

                TempData["SuccessMessage"] = "Rezervasyon başarıyla iptal edildi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rezervasyon bulunamadı.";
            }

            return RedirectToAction("Rezervasyonlar");
        }
    }
}