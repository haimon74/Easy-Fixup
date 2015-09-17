using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AudioAccessMap : EntityTypeConfiguration<AudioAccess>
    {
        public AudioAccessMap()
        {
            // Primary Key
            this.HasKey(t => new { t.aa_audioowner, t.aa_audioviewer });

            // Properties
            this.Property(t => t.aa_audioowner)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.aa_audioviewer)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("AudioAccess");
            this.Property(t => t.aa_audioowner).HasColumnName("aa_audioowner");
            this.Property(t => t.aa_audioviewer).HasColumnName("aa_audioviewer");
            this.Property(t => t.aa_dateaccessgranted).HasColumnName("aa_dateaccessgranted");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.AudioAccesses)
                .HasForeignKey(d => d.aa_audioowner).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.AudioAccesses1)
                .HasForeignKey(d => d.aa_audioviewer).WillCascadeOnDelete(false);

        }
    }
}
