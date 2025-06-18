namespace MVC_Kutuphane_Otomasyonu.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vreatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Duyurular",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Baslik = c.String(nullable: false, maxLength: 150, unicode: false),
                        Duyuru = c.String(nullable: false, maxLength: 600),
                        Aciklama = c.String(),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.EmanetKitaplar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UyeId = c.Int(nullable: false),
                        KitapId = c.Int(nullable: false),
                        KitapSayisi = c.Int(nullable: false),
                        KitapAldigiTarihi = c.DateTime(nullable: false),
                        KitapIadeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kitaplar", t => t.KitapId, cascadeDelete: true)
                .Index(t => t.KitapId);
            
            CreateTable(
                "dbo.Kitaplar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarkodNo = c.String(nullable: false, maxLength: 30),
                        KitapTuruId = c.Int(nullable: false),
                        KitapAdi = c.String(nullable: false, maxLength: 100),
                        YazarAdi = c.String(nullable: false, maxLength: 100),
                        YayinEvi = c.String(nullable: false, maxLength: 150),
                        StokAdedi = c.Int(nullable: false),
                        SayfaSayisi = c.Int(nullable: false),
                        Aciklama = c.String(nullable: false),
                        EklemeTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(nullable: false),
                        SilindiMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KitapTurleri", t => t.KitapTuruId, cascadeDelete: true)
                .Index(t => t.KitapTuruId);
            
            CreateTable(
                "dbo.KitapHareketleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        UyeId = c.Int(nullable: false),
                        KitapId = c.Int(nullable: false),
                        YapilanIslem = c.String(nullable: false, maxLength: 150),
                        Aciklama = c.String(),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kitaplar", t => t.KitapId, cascadeDelete: true)
                .Index(t => t.KitapId);
            
            CreateTable(
                "dbo.KitapKayitHareketleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        UyeId = c.Int(nullable: false),
                        KitapId = c.Int(nullable: false),
                        YapilanIslem = c.String(nullable: false, maxLength: 150),
                        Aciklama = c.String(),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kitaplar", t => t.KitapId, cascadeDelete: true)
                .Index(t => t.KitapId);
            
            CreateTable(
                "dbo.KitapTurleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KitapTuru = c.String(nullable: false, maxLength: 150),
                        Aciklama = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hakkimizda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Icerik = c.String(nullable: false, maxLength: 500),
                        Aciklama = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Iletisim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdiSoyadi = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Baslik = c.String(nullable: false, maxLength: 200),
                        Mesaj = c.String(nullable: false, maxLength: 500),
                        Aciklama = c.String(nullable: false),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KullaniciHareketleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        Aciklama = c.String(),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kullanicilar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciAdi = c.String(nullable: false, maxLength: 20),
                        Sifre = c.String(nullable: false, maxLength: 15),
                        AdiSoyadi = c.String(nullable: false, maxLength: 100),
                        Telefon = c.String(nullable: false, maxLength: 20),
                        Adres = c.String(nullable: false, maxLength: 500),
                        Email = c.String(nullable: false, maxLength: 100),
                        KayıtTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KullaniciRolleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        RolId = c.Int(nullable: false),
                        Roller_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roller", t => t.Roller_Id)
                .Index(t => t.Roller_Id);
            
            CreateTable(
                "dbo.Roller",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rol = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uyeler",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdiSoyadi = c.String(nullable: false, maxLength: 100),
                        Telefon = c.String(nullable: false, maxLength: 20),
                        Adres = c.String(nullable: false, maxLength: 500),
                        Email = c.String(nullable: false, maxLength: 100),
                        ResimYolu = c.String(),
                        OkulKitapSayisi = c.Int(nullable: false),
                        KayıtTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KullaniciRolleri", "Roller_Id", "dbo.Roller");
            DropForeignKey("dbo.EmanetKitaplar", "KitapId", "dbo.Kitaplar");
            DropForeignKey("dbo.Kitaplar", "KitapTuruId", "dbo.KitapTurleri");
            DropForeignKey("dbo.KitapKayitHareketleri", "KitapId", "dbo.Kitaplar");
            DropForeignKey("dbo.KitapHareketleri", "KitapId", "dbo.Kitaplar");
            DropIndex("dbo.KullaniciRolleri", new[] { "Roller_Id" });
            DropIndex("dbo.KitapKayitHareketleri", new[] { "KitapId" });
            DropIndex("dbo.KitapHareketleri", new[] { "KitapId" });
            DropIndex("dbo.Kitaplar", new[] { "KitapTuruId" });
            DropIndex("dbo.EmanetKitaplar", new[] { "KitapId" });
            DropTable("dbo.Uyeler");
            DropTable("dbo.Roller");
            DropTable("dbo.KullaniciRolleri");
            DropTable("dbo.Kullanicilar");
            DropTable("dbo.KullaniciHareketleri");
            DropTable("dbo.Iletisim");
            DropTable("dbo.Hakkimizda");
            DropTable("dbo.KitapTurleri");
            DropTable("dbo.KitapKayitHareketleri");
            DropTable("dbo.KitapHareketleri");
            DropTable("dbo.Kitaplar");
            DropTable("dbo.EmanetKitaplar");
            DropTable("dbo.Duyurular");
        }
    }
}
