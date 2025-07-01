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
    [Authorize(Roles ="Admin")]
    public class RollerController : Controller
    {
        // GET: Roller
        KutuphaneContext context=new KutuphaneContext();
        RollerDAL rollerDAL = new RollerDAL();
        public ActionResult Index()
        {
            var model = rollerDAL.GetAll(context);
            return View(model);
        }
        public ActionResult Ekle() 
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Ekle(Roller entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }
            rollerDAL.InsertorUpdate(context, entity);
            rollerDAL.Save(context);
            return RedirectToAction("Index");
        }
        public ActionResult Duzenle(int? id)
        {
            if(id == null) 
                return HttpNotFound();

            var model = rollerDAL.GetById(context, id);
            if(model== null)
                return HttpNotFound("Rol Bulunamadı.");

            return View(model); 
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Duzenle(Roller entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }
            rollerDAL.InsertorUpdate(context, entity);
            rollerDAL.Save(context);
            return RedirectToAction("Index");
        }

        public ActionResult Sil(int id) 
        {
            rollerDAL.Delete(context, x => x.Id==id);
            rollerDAL.Save(context);
            return RedirectToAction("Index");
        }
    }

}