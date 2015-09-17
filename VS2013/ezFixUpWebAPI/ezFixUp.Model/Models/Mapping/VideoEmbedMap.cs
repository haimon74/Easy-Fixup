using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VideoEmbedMap : EntityTypeConfiguration<VideoEmbed>
    {
        public VideoEmbedMap()
        {
            // Primary Key
            this.HasKey(t => t.ve_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ve_thumburl)
                .HasMaxLength(200);

            this.Property(t => t.ve_videourl)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ve_title)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("VideoEmbeds");
            this.Property(t => t.ve_id).HasColumnName("ve_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ve_thumburl).HasColumnName("ve_thumburl");
            this.Property(t => t.ve_videourl).HasColumnName("ve_videourl");
            this.Property(t => t.ve_title).HasColumnName("ve_title");
            this.Property(t => t.ve_sourcetype).HasColumnName("ve_sourcetype");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.VideoEmbeds)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
