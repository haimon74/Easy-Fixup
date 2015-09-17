using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VideoProfileMap : EntityTypeConfiguration<VideoProfile>
    {
        public VideoProfileMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.vp_approved, t.vp_private });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VideoProfiles");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.vp_approved).HasColumnName("vp_approved");
            this.Property(t => t.vp_private).HasColumnName("vp_private");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.VideoProfiles)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
