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
    public class KitapTurleriMap:EntityTypeConfiguration<KitapTurleri>
    {
        public KitapTurleriMap()
        {
            this.ToTable("KitapTurleri");
            this.HasKey(x => x.Id);//Primary Key
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);//Otomatik Artan Sayı
            this.Property(x => x.KitapTuru).IsRequired().HasMaxLength(150);
            this.Property(x => x.Aciklama).HasMaxLength(5000);

        }
    }
}
