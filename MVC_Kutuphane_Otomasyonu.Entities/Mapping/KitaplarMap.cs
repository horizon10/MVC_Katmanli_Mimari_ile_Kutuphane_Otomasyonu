using MVC_Kutuphane_Otomasyonu.Entities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Mapping
{
    public class KitaplarMap:EntityTypeConfiguration<Kitaplar>
    {
        public KitaplarMap()
        {
            this.ToTable("Kitaplar");
            this.HasKey(x => x.Id);//Primary Key
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);//Otomatik Artan Sayı
            this.Property(x => x.BarkodNo).IsRequired().HasMaxLength(30);
            this.Property(x => x.KitapAdi).IsRequired().HasMaxLength(100);
            this.Property(x => x.YazarAdi).IsRequired().HasMaxLength(100);
            this.Property(x => x.YayinEvi).IsRequired().HasMaxLength(150);
            this.Property(x => x.Aciklama).IsRequired().HasMaxLength(5000);

            this.HasRequired(x => x.KitapTurleri).WithMany(x => x.Kitaplar).HasForeignKey(x => x.KitapTuruId);
        }
    }
}
