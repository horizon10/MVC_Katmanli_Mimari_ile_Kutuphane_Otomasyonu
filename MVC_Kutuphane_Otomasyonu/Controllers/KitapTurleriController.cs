using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    public class KitapTurleriController : Controller
    {
        KutuphaneContext context=new KutuphaneContext();
        KitapTurleriDAL KitapTurleriDAL=new KitapTurleriDAL();
        // GET: KitapTurleri
        public ActionResult Index(string ara,int? page)
        {
            var model = KitapTurleriDAL.GetAll(context).ToPagedList(page??1,3);
            if (ara != null)
            {
                model = KitapTurleriDAL.GetAll(context,x=>x.KitapTuru.Contains(ara)).ToPagedList(page ?? 1, 3);
            }
            return View("Index",model);
        }
        public ActionResult Ekle()
        {
            
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Ekle(KitapTurleri kitapTurleri)
        {
            if (ModelState.IsValid)
            {
                KitapTurleriDAL.InsertorUpdate(context, kitapTurleri);
                KitapTurleriDAL.Save(context);
                return RedirectToAction("Index");
            }
            return View(kitapTurleri);
        }
        public ActionResult Duzenle(int? id) 
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var model = KitapTurleriDAL.GetById(context, id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Duzenle(KitapTurleri kitapTurleri)
        {
            if (ModelState.IsValid)
            {
                KitapTurleriDAL.InsertorUpdate(context, kitapTurleri);
                KitapTurleriDAL.Save(context);
                return RedirectToAction("Index");
            }
            return View(kitapTurleri);
        }
        public ActionResult Sil(int? id)
        {
            KitapTurleriDAL.Delete(context, x=>x.Id==id);
            KitapTurleriDAL.Save(context);
            return RedirectToAction("Index");
        }
    }
}