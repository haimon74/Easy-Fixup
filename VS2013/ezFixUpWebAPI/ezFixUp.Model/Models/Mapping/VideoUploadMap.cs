using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VideoUploadMap : EntityTypeConfiguration<VideoUpload>
    {
        public VideoUploadMap()
        {
            // Primary Key
            this.HasKey(t => t.vu_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VideoUploads");
            this.Property(t => t.vu_id).HasColumnName("vu_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.vu_processed).HasColumnName("vu_processed");
            this.Property(t => t.vu_approved).HasColumnName("vu_approved");
            this.Property(t => t.vu_private).HasColumnName("vu_private");
        }
    }
}
