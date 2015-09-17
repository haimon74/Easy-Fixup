using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AffiliateBannerMap : EntityTypeConfiguration<AffiliateBanner>
    {
        public AffiliateBannerMap()
        {
            // Primary Key
            this.HasKey(t => t.ab_id);

            // Properties
            this.Property(t => t.ab_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ab_image)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AffiliateBanners");
            this.Property(t => t.ab_id).HasColumnName("ab_id");
            this.Property(t => t.ab_name).HasColumnName("ab_name");
            this.Property(t => t.ab_image).HasColumnName("ab_image");
            this.Property(t => t.ab_deleted).HasColumnName("ab_deleted");
        }
    }
}
