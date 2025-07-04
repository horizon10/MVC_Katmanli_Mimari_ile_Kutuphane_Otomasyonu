﻿using ClosedXML.Excel;
using MVC_Kutuphane_Otomasyonu.Entities.DAL;
using MVC_Kutuphane_Otomasyonu.Entities.Mapping;
using MVC_Kutuphane_Otomasyonu.Entities.Model.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Kutuphane_Otomasyonu.Controllers
{
    [Authorize(Roles = "Admin,Moderator,Kullanıcı")]
    public class IstatistiklerController : Controller
    {
        KutuphaneContext context=new KutuphaneContext();
        KullaniciHareketleriDAL kullaniciHareketleriDAL=new KullaniciHareketleriDAL();
        KullanicilarDAL kullanicilarDAL=new KullanicilarDAL();
        UyelerDAL uyelerDAL=new UyelerDAL();
        EmanetKitaplarDAL emanetKitaplarDAL=new EmanetKitaplarDAL();
        KitapTurleriDAL kitapTurleriDAL=new KitapTurleriDAL();
        // GET: Istatistikler
        public ActionResult Index()
        {
            if (Session["AktifKullanici"] == null)
            {
                return RedirectToAction("KullaniciLogin", "Home");
            }

            // Burada veri atama işlemi yapılmalı
            uyelerDAL.UyeKitapIslemleri();
            ViewBag.EnCokUye = uyelerDAL.EnCokUye;
            ViewBag.EnAzUye = uyelerDAL.EnAzUye;
            ViewBag.EnCokSayi = uyelerDAL.EnCokSayi;
            ViewBag.EnAzSayi = uyelerDAL.EnAzSayi;
            ViewBag.UyeKitapModel = uyelerDAL.UyeKitapModel;

            var KullaniciSayisiModel = kullanicilarDAL.GetAll(context);
            ViewBag.Count = KullaniciSayisiModel.Count;

            var model = kullaniciHareketleriDAL.KullaniciGirisSayilari();
            ViewBag.KullaniciAdi = model.KullaniciAdi;
            ViewBag.GirisSayisi = model.GirisSayisi;

            kullaniciHareketleriDAL.KullaniciHareketleriGozleme();
            ViewBag.AylikVeriler = kullaniciHareketleriDAL.AylikVeriler;
            ViewBag.ToplamKHGSayisi = kullaniciHareketleriDAL.ToplamKHGSayisi;
            ViewBag.AltiAyToplamKHGSayisi = kullaniciHareketleriDAL.AltiAyToplamKHGSayisi;

            var turIstatistik = kitapTurleriDAL.GetKitapTuruIstatistik();
            ViewBag.KitapTurIstatistik = turIstatistik;

            var enCok = emanetKitaplarDAL.EnCokEmanetEdilenKitap();
            var enAz = emanetKitaplarDAL.EnAzEmanetEdilenKitap();

            ViewBag.EnCokEmanetKitap = $"{enCok.KitapAdi} ({enCok.KitapTuru})";
            ViewBag.EnCokEmanetSayi = enCok.Toplam;

            ViewBag.EnAzEmanetKitap = $"{enAz.KitapAdi} ({enAz.KitapTuru})";
            ViewBag.EnAzEmanetSayi = enAz.Toplam;

            return View();
        }

        public ActionResult ExceleAktar()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Emanet Kitaplar");
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Uye";
                worksheet.Cell(1, 3).Value = "Kitap Adı";
                worksheet.Cell(1, 4).Value = "Kitap Sayısı";
                worksheet.Cell(1, 5).Value = "Kitap Aldığı Tarih";

                var model = emanetKitaplarDAL.GetAll(context, x => x.KitapIadeTarihi == null, "Uyeler", "Kitaplar");

                int row = 2;
                foreach (var item in model)
                {
                    worksheet.Cell(row, 1).Value = item.Id;
                    worksheet.Cell(row, 2).Value = item.Uyeler.AdiSoyadi;
                    worksheet.Cell(row, 3).Value = item.Kitaplar.KitapAdi;
                    worksheet.Cell(row, 4).Value = item.KitapSayisi;
                    worksheet.Cell(row, 5).Value = item.KitapAldigiTarihi;
                    row++;
                }
                using(var stream = new MemoryStream())//Verileri Ram de saklıyoruz. 
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),"application/vnd.openXmlformats-officedocument.spreadsheetml","EmanetKitapSeri.xlsx");
                }

            }
                return null;
        }
    }
}