using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VideoAccessMap : EntityTypeConfiguration<VideoAccess>
    {
        public VideoAccessMap()
        {
            // Primary Key
            this.HasKey(t => new { t.va_videoviewer, t.va_videoowner });

            // Properties
            this.Property(t => t.va_videoviewer)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.va_videoowner)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VideoAccess");
            this.Property(t => t.va_videoviewer).HasColumnName("va_videoviewer");
            this.Property(t => t.va_videoowner).HasColumnName("va_videoowner");
            this.Property(t => t.va_dateaccessgranted).HasColumnName("va_dateaccessgranted");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.VideoAccesses)
                .HasForeignKey(d => d.va_videoowner).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.VideoAccesses1)
                .HasForeignKey(d => d.va_videoviewer).WillCascadeOnDelete(false);

        }
    }
}
