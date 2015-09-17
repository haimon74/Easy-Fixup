using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class UserTipsStatuMap : EntityTypeConfiguration<UserTipsStatu>
    {
        public UserTipsStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.uts_id);

            // Properties
            this.Property(t => t.uts_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("UserTipsStatus");
            this.Property(t => t.uts_id).HasColumnName("uts_id");
            this.Property(t => t.uts_tip_id).HasColumnName("uts_tip_id");
            this.Property(t => t.uts_username).HasColumnName("uts_username");
            this.Property(t => t.uts_viewed_times).HasColumnName("uts_viewed_times");
            this.Property(t => t.uts_is_blocked).HasColumnName("uts_is_blocked");
            this.Property(t => t.uts_last_viewed_date).HasColumnName("uts_last_viewed_date");
        }
    }
}
