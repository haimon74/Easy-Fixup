using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AdPhotoMap : EntityTypeConfiguration<AdPhoto>
    {
        public AdPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.ap_id);

            // Properties
            this.Property(t => t.ap_description)
                .IsRequired();

            this.Property(t => t.ap_image)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AdPhotos");
            this.Property(t => t.ap_id).HasColumnName("ap_id");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.ap_description).HasColumnName("ap_description");
            this.Property(t => t.ap_image).HasColumnName("ap_image");

            // Relationships
            this.HasRequired(t => t.Ad)
                .WithMany(t => t.AdPhotos)
                .HasForeignKey(d => d.a_id).WillCascadeOnDelete(false);

        }
    }
}
