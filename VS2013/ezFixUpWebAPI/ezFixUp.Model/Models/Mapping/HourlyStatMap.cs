using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class HourlyStatMap : EntityTypeConfiguration<HourlyStat>
    {
        public HourlyStatMap()
        {
            // Primary Key
            this.HasKey(t => t.hs_time);

            // Properties
            // Table & Column Mappings
            this.ToTable("HourlyStats");
            this.Property(t => t.hs_time).HasColumnName("hs_time");
            this.Property(t => t.hs_usersonline).HasColumnName("hs_usersonline");
            this.Property(t => t.hs_newusers).HasColumnName("hs_newusers");
            this.Property(t => t.hs_sentmessages).HasColumnName("hs_sentmessages");
        }
    }
}
