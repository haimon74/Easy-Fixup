using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoAlbumMap : EntityTypeConfiguration<PhotoAlbum>
    {
        public PhotoAlbumMap()
        {
            // Primary Key
            this.HasKey(t => t.pa_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pa_name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PhotoAlbums");
            this.Property(t => t.pa_id).HasColumnName("pa_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pa_name).HasColumnName("pa_name");
            this.Property(t => t.pa_access).HasColumnName("pa_access");
            this.Property(t => t.pa_cover_photoid).HasColumnName("pa_cover_photoid");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoAlbums)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
