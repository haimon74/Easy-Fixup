using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class MessagesHistoryCacheMap : EntityTypeConfiguration<MessagesHistoryCache>
    {
        public MessagesHistoryCacheMap()
        {
            // Primary Key
            this.HasKey(t => new { t.mhc_fromusername, t.mhc_tousername });

            // Properties
            this.Property(t => t.mhc_fromusername)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.mhc_tousername)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MessagesHistoryCache");
            this.Property(t => t.mhc_fromusername).HasColumnName("mhc_fromusername");
            this.Property(t => t.mhc_tousername).HasColumnName("mhc_tousername");
        }
    }
}
