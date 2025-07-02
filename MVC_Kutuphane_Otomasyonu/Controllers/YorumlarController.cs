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
    public class YorumlarController : Controller
    {
        KutuphaneContext context=new KutuphaneContext();
        // GET: Yorumlar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Yorumlar()
        {
            var yorumlar = context.Yorumlar
                .Include(y => y.Kitap)
                .Include(y => y.Uye)
                .OrderByDescending(y => y.YorumTarihi)
                .ToList();

            return View(yorumlar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YorumSil(int id)
        {
            var yorum = context.Yorumlar.Find(id);
            if (yorum != null)
            {
                context.Yorumlar.Remove(yorum);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Yorum bulunamadı.";
            }

            return RedirectToAction("Yorumlar");
        }
    }
}