using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoMap : EntityTypeConfiguration<Photo>
    {
        public PhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.p_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.p_name)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.p_description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.p_approvedby)
                .HasMaxLength(20);

            this.Property(t => t.p_facecrop)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Photos");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pa_id).HasColumnName("pa_id");
            this.Property(t => t.p_image).HasColumnName("p_image");
            this.Property(t => t.p_name).HasColumnName("p_name");
            this.Property(t => t.p_description).HasColumnName("p_description");
            this.Property(t => t.p_approved).HasColumnName("p_approved");
            this.Property(t => t.p_primary).HasColumnName("p_primary");
            this.Property(t => t.p_explicit).HasColumnName("p_explicit");
            this.Property(t => t.p_private).HasColumnName("p_private");
            this.Property(t => t.p_approvedby).HasColumnName("p_approvedby");
            this.Property(t => t.p_approvedtimestamp).HasColumnName("p_approvedtimestamp");
            this.Property(t => t.p_facecrop).HasColumnName("p_facecrop");
            this.Property(t => t.p_manual_approval).HasColumnName("p_manual_approval");
            this.Property(t => t.p_salute).HasColumnName("p_salute");

            // Relationships
            this.HasOptional(t => t.PhotoAlbum)
                .WithMany(t => t.Photos)
                .HasForeignKey(d => d.pa_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Photos)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
