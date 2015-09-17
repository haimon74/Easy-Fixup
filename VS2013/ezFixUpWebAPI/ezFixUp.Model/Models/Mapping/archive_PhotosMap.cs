using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class archive_PhotosMap : EntityTypeConfiguration<archive_Photos>
    {
        public archive_PhotosMap()
        {
            // Primary Key
            this.HasKey(t => new { t.p_id, t.u_username, t.p_name, t.p_description, t.p_approved, t.p_primary, t.p_explicit, t.p_private });

            // Properties
            this.Property(t => t.p_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.p_name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.p_description)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("archive_Photos");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.p_image).HasColumnName("p_image");
            this.Property(t => t.p_name).HasColumnName("p_name");
            this.Property(t => t.p_description).HasColumnName("p_description");
            this.Property(t => t.p_approved).HasColumnName("p_approved");
            this.Property(t => t.p_primary).HasColumnName("p_primary");
            this.Property(t => t.p_explicit).HasColumnName("p_explicit");
            this.Property(t => t.p_private).HasColumnName("p_private");
        }
    }
}
