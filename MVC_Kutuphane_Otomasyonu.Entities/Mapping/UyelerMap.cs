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
    public class UyelerMap:EntityTypeConfiguration<Uyeler>
    {
        public UyelerMap() 
        {
            this.ToTable("Uyeler");
            this.HasKey(x => x.Id);//Primary Key
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);//Otomatik Artan Sayı
            this.Property(x => x.AdiSoyadi).IsRequired().HasMaxLength(100);
            this.Property(x => x.Email).IsRequired().HasMaxLength(100);
            this.Property(x => x.Telefon).IsRequired().HasMaxLength(20);
            this.Property(x => x.Adres).IsRequired().HasMaxLength(500);

        }
    }
}
