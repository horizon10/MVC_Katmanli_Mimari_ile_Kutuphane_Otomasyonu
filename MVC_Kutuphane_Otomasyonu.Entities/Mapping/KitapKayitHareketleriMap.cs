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
    public class KitapKayitHareketleriMap:EntityTypeConfiguration<KitapKayitHareketleri>
    {
        public KitapKayitHareketleriMap()
        {
            this.ToTable("KitapKayitHareketleri");
            this.HasKey(x=> x.Id);//Primary Key
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);//Otomatik Artan Sayı
            this.Property(x => x.YapilanIslem).IsRequired().HasMaxLength(150);

            this.HasRequired(x => x.Kitaplar).WithMany(x => x.KitapKayitHareketleri).HasForeignKey(x => x.KitapId);

        }
    }
}
