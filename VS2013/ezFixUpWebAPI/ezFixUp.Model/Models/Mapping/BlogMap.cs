using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            // Primary Key
            this.HasKey(t => t.b_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.b_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.b_description)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Blogs");
            this.Property(t => t.b_id).HasColumnName("b_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.b_name).HasColumnName("b_name");
            this.Property(t => t.b_description).HasColumnName("b_description");
            this.Property(t => t.b_datecreated).HasColumnName("b_datecreated");
            this.Property(t => t.b_settings).HasColumnName("b_settings");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Blogs)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
