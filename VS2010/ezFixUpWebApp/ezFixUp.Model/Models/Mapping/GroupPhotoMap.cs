using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupPhotoMap : EntityTypeConfiguration<GroupPhoto>
    {
        public GroupPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.gp_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.gp_name)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.gp_description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.gp_image)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("GroupPhotos");
            this.Property(t => t.gp_id).HasColumnName("gp_id");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gp_name).HasColumnName("gp_name");
            this.Property(t => t.gp_description).HasColumnName("gp_description");
            this.Property(t => t.gp_image).HasColumnName("gp_image");
            this.Property(t => t.gp_date).HasColumnName("gp_date");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupPhotos)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupPhotos)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
