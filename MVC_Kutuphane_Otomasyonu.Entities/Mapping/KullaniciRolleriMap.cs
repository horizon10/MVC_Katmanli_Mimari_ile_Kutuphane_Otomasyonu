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
    public class KullaniciRolleriMap:EntityTypeConfiguration<KullaniciRolleri>
    {
        public KullaniciRolleriMap()
        {
            this.ToTable("KullaniciRolleri");
            this.HasKey(x => x.Id);//Primary Key
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);//Otomatik Artan Sayı

            this.HasRequired(x => x.Kullanicilar).WithMany(x => x.KullaniciRolleri).HasForeignKey(x => x.KullaniciId);
            this.HasRequired(x => x.Roller).WithMany(x => x.KullaniciRolleri).HasForeignKey(x => x.RolId);
        }
    }
}
