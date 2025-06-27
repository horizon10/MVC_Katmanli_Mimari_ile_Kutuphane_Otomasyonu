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
            var kitaplar = context.Kitaplar.Include("KitapTurleri").ToList();
            var duyurular = context.Duyurular.OrderByDescending(x => x.Tarih).ToList();
            ViewBag.KitaplarListesi = kitaplar;
            ViewBag.DuyurularListesi = duyurular;
            return View();
        }


        public ActionResult About()
        {
            var model= hakkimizdaDAL.GetAll(context);
            return View(model);
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