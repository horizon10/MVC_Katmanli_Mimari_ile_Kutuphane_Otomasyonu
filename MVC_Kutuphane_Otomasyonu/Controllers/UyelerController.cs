using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UyelerController : Controller
    {
        // GET: Uyeler
        KutuphaneContext context = new KutuphaneContext();
        UyelerDAL uyelerDAL = new UyelerDAL();
        public ActionResult Index()
        {
            var model = uyelerDAL.GetAll(context);
            return View(model);
        }
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Ekle(Uyeler entity, HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid) 
            {
                if (Resim != null && Resim.ContentLength>0)
                {
                    var image = Path.GetFileName(Resim.FileName);
                    string path=Path.Combine(Server.MapPath("~/images"), image);
                    if (System.IO.File.Exists(path)==false)
                    {
                        Resim.SaveAs(path);
                    }
                    entity.ResimYolu = "/images/" + image;
                }
                uyelerDAL.InsertorUpdate(context, entity);
                uyelerDAL.Save(context);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        public ActionResult Duzenle(int? id)
        {
            if(id== null)
            {
                return HttpNotFound();
            }
            var model=uyelerDAL.GetByFilter(context,x=>x.Id==id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Duzenle(Uyeler entity, HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid)
            {
                var model = uyelerDAL.GetByFilter(context, x => x.Id == entity.Id);
                entity.ResimYolu=model.ResimYolu;
                if (Resim != null && Resim.ContentLength > 0)
                {
                    var image = Path.GetFileName(Resim.FileName);
                    string path = Path.Combine(Server.MapPath("~/images"), image);
                    if (System.IO.File.Exists(path) == false)
                    {
                        Resim.SaveAs(path);
                    }
                    entity.ResimYolu = "/images/" + image;
                }
                uyelerDAL.InsertorUpdate(context, entity);
                uyelerDAL.Save(context);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        public ActionResult Sil(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            uyelerDAL.Delete(context,x=>x.Id == id);
            uyelerDAL.Save(context);
            return RedirectToAction("Index");
        }
    }
}