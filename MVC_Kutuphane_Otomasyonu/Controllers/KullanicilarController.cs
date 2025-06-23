using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Model;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using MVC_Kutuphane_Otomasyonu.Entities.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    //denerken sürekli giriş yapmamak adına yorum satırına aldım 
    //[Authorize(Roles="Admin,Moderatör")] 
    [AllowAnonymous]
    public class KullanicilarController : Controller
    {
        KutuphaneContext context = new KutuphaneContext();
        KullanicilarDAL kullanicilarDAL = new KullanicilarDAL();
        KullaniciRolleriDAL KullaniciRolleriDAL=new KullaniciRolleriDAL();
        RollerDAL RollerDAL = new RollerDAL();  
        // GET: Kullanicilar
        public ActionResult Index()
        {
            var model=kullanicilarDAL.GetAll(context);
            return View(model);
        }
        public ActionResult Ekle()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Ekle(Kullanicilar entity)
        {
            if (!ModelState.IsValid) 
            {
                return View(entity);
            }
            kullanicilarDAL.InsertorUpdate(context,entity);
            kullanicilarDAL.Save(context);
            return RedirectToAction("Index2");
        }
        public ActionResult Duzenle(int? id)
        {
            if (id == null)
            {
                return HttpNotFound("Id değeri girilmedi");
            }
            var model=kullanicilarDAL.GetById(context,id);
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Duzenle(Kullanicilar entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }
            kullanicilarDAL.InsertorUpdate(context, entity);
            kullanicilarDAL.Save(context);
            return RedirectToAction("Index2");
        }
        public ActionResult Sil(int? id)
        {
            kullanicilarDAL.Delete(context, x => x.Id == id);
            kullanicilarDAL.Save(context);
            return RedirectToAction("Index2");
        }
        public ActionResult Index2()
        {
            var kullanicilar = kullanicilarDAL.GetAll(context, tbl: "KullaniciRolleri");
            var roller = RollerDAL.GetAll(context);
            return View(new KullaniciRolViewModel { Kullanicilar=kullanicilar,Roller=roller});
        }
        public ActionResult KRolleri(int id)
        {
            var model = KullaniciRolleriDAL.GetAll(context, x => x.KullaniciId==id,"Roller");
            if (model != null) 
            {
                return View(model);
            }
            return HttpNotFound();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Kullanicilar entity)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            var model = kullanicilarDAL.GetByFilter(context, x => x.Email == entity.Email && x.Sifre == entity.Sifre);
            if (model != null)
            {
                FormsAuthentication.SetAuthCookie(entity.Email, false);
                return RedirectToAction("Index", "KitapTurleri");
            }
            ViewBag.error = "Kullanıcı adı veya şifre yanlış";
            return View();
        }
        [AllowAnonymous]
        public ActionResult KayitOl()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult KayitOl(Kullanicilar entity, string sifreTekrar, bool kabul)
        {
            if (!ModelState.IsValid)//Model doğrulanmazsa
            {
                return View(entity);
            }
            else//Model Doğrulanırsa
            {
                if (entity.Sifre != sifreTekrar)//Şifreler uyuşmazsa
                {
                    ViewBag.sifreError = "Şifreler uyuşmuyor!";
                    return View();
                }
                else//şifreler uyuşursa
                {
                    if(!kabul)//Şartlar kabul edilmemişse
                    {
                        ViewBag.kabulError = "Lütfen şartları kabul ettiğinizi onaylayın!";
                        return View();
                    }
                    else//Şartlar kabul edilmişse
                    {
                        entity.KayıtTarihi=DateTime.Now;
                        kullanicilarDAL.InsertorUpdate(context, entity);
                        kullanicilarDAL.Save(context);
                        return RedirectToAction("Login");
                    }
                }
            }
        }
        [AllowAnonymous]
        public ActionResult SifremiUnuttum()
        {
            return View() ;
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult SifremiUnuttum(Kullanicilar entity)
        {
            var model = kullanicilarDAL.GetByFilter(context, x => x.Email == entity.Email);
            if (model != null)
            {
                Guid rastgele = Guid.NewGuid();
                model.Sifre = rastgele.ToString().Substring(0, 8);
                kullanicilarDAL.Save(context);
                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("kendihesabın", "Şifre sıfırlama");
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Değiştirme İsteği";
                mail.Body += "Merhaba " + model.AdiSoyadi + "<br/> Kullanıcı Adınız=" + model.KullaniciAdi + "<br/> Şifreniz=" + model.Sifre;
                NetworkCredential net = new NetworkCredential("kendihesabın", "kendi şifren");
                client.Credentials = net;
                try
                {
                    client.Send(mail);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.hata = "Mail gönderilirken hata oluştu: " + ex.Message;
                    return View();
                }
                //return RedirectToAction("Login");
            }
            else if (model == null && entity.Email != null)
            {
                ViewBag.hata = "Böyle bir e-mail adresi bulunamadı.";
                return View();
            }
            return View();
        }
    }
}