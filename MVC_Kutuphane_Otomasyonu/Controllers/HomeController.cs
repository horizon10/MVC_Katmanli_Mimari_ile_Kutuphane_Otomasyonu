using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        KutuphaneContext context=new KutuphaneContext();
        HakkimizdaDAL hakkimizdaDAL=new HakkimizdaDAL();
        IletisimDAL iletisimDAL=new IletisimDAL();
        public ActionResult Index()
        {
            try
            {
                var kitaplar = context.Kitaplar.Include("KitapTurleri").ToList();
                if (kitaplar == null || !kitaplar.Any())
                {
                    // Boş liste oluşturarak view'ın çalışmasını sağla
                    kitaplar = new List<Kitaplar>();
                }
                return View(kitaplar);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<Kitaplar>());
            }
        }


        public ActionResult About()
        {
            var model= hakkimizdaDAL.GetAll(context);
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
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Contact(Iletisim model)
        {
            if (ModelState.IsValid) 
            {
                model.Tarih=DateTime.Now;
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
    }
}