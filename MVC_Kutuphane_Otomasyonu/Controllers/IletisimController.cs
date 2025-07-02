using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [Authorize(Roles ="Admin,Moderator")]
    public class IletisimController : Controller
    {
        // GET: Iletisim
        KutuphaneContext context=new KutuphaneContext();
        public ActionResult Index()
        {
            var mesajlar = context.Iletisim.OrderByDescending(x => x.Tarih).ToList();
            return View(mesajlar);
        }
        public ActionResult Detay(int  id)
        {
            var mesaj=context.Iletisim.FirstOrDefault(x => x.Id == id);
            if (mesaj == null)
            {
                return HttpNotFound("Mesaj bullunamadı");
            }
            return View(mesaj);
        }

        public ActionResult Sil(int id)
        {
            var mesaj = context.Iletisim.FirstOrDefault(x => x.Id == id);
            if (mesaj == null)
            {
                context.Iletisim.Remove(mesaj);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}