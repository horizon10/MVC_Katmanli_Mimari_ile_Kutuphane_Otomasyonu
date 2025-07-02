using MVC_Kutuphane_Otomasyonu.Entities.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Model.Context
{
    public class KutuphaneContext :DbContext
    {
        public KutuphaneContext():base("Kutuphane")
        {

        }
        public DbSet<Duyurular> Duyurular { get; set; }    
        public DbSet<EmanetKitaplar> EmanetKitaplar { get; set; }    
        public DbSet<Hakkimizda> Hakkimizda { get; set; }    
        public DbSet<Iletisim> Iletisim { get; set; }    
        public DbSet<KitapHareketleri> KitapHareketler{ get; set; }    
        public DbSet<KitapKayitHareketleri> KitapKayitHareketleri { get; set; }    
        public DbSet<Kitaplar> Kitaplar { get; set; }
        public DbSet<KitapTurleri> KitapTurleri { get; set; }
        public DbSet<KullaniciHareketleri> KullaniciHareketleri { get; set; }
        public DbSet<Kullanicilar> Kullanicilar { get; set; }
        public DbSet<KullaniciRolleri> KullaniciRolleri { get; set; }
        public DbSet<Roller> Roller { get; set; }
        public DbSet<Uyeler> Uyeler { get; set; }
        public DbSet<Yorumlar> Yorumlar { get; set; }
        public DbSet<KitapRezervasyonlar> kitapRezervasyonlar   { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DuyurularMap());
            modelBuilder.Configurations.Add(new EmanetKitaplarMap());
            modelBuilder.Configurations.Add(new HakkimizdaMap());
            modelBuilder.Configurations.Add(new IletisimMap());
            modelBuilder.Configurations.Add(new KitapHareketleriMap());
            modelBuilder.Configurations.Add(new KitapKayitHareketleriMap());
            modelBuilder.Configurations.Add(new KitaplarMap());
            modelBuilder.Configurations.Add(new KitapTurleriMap());
            modelBuilder.Configurations.Add(new KullaniciHareketleriMap());
            modelBuilder.Configurations.Add(new KullanicilarMap());
            modelBuilder.Configurations.Add(new KullaniciRolleriMap());
            modelBuilder.Configurations.Add(new RollerMap());
            modelBuilder.Configurations.Add(new UyelerMap());
        }
    }
}
