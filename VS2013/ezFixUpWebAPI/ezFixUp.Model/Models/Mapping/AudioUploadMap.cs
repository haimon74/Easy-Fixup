using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AudioUploadMap : EntityTypeConfiguration<AudioUpload>
    {
        public AudioUploadMap()
        {
            // Primary Key
            this.HasKey(t => t.au_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.au_title)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("AudioUploads");
            this.Property(t => t.au_id).HasColumnName("au_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.au_title).HasColumnName("au_title");
            this.Property(t => t.au_approved).HasColumnName("au_approved");
            this.Property(t => t.au_private).HasColumnName("au_private");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.AudioUploads)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
