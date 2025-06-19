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
    public class DuyurularController : Controller
    {
        // GET: Duyurular
        KutuphaneContext context=new KutuphaneContext();
        DuyurularDAL duyurularDAL = new DuyurularDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DuyuruList()
        {
            var model=duyurularDAL.GetAll(context);
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DuyuruEkle(Duyurular entity)
        {
            if (ModelState.IsValid)
            {
                duyurularDAL.InsertorUpdate(context, entity);
                duyurularDAL.Save(context);
                return Json(new { success = true, message = "İşlem başarıyla gerçekleşti" });
            }
            var errors = ModelState.ToDictionary(
                x => x.Key,
                x => x.Value.Errors.Select(a => a.ErrorMessage).ToArray()
            );
            return Json(new { success = false,JsonRequestBehavior.AllowGet });
        }
        public JsonResult DuyuruGetir(int? id)
        {
            var model=duyurularDAL.GetByFilter(context,x=>x.Id==id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DuyuruSil(int? Id)
        {
            duyurularDAL.Delete(context, x => x.Id == Id);
            duyurularDAL.Save(context);
            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult SeciliDuyuruSil(List<int> selectedIds)
        {
            if (selectedIds != null)
            {
                foreach (int id in selectedIds)
                {
                    duyurularDAL.Delete(context, x => x.Id == id);
                    duyurularDAL.Save(context);
                }
                return Json(new {success=true});
            }
            return Json(new {success=false});
        }
    }
}