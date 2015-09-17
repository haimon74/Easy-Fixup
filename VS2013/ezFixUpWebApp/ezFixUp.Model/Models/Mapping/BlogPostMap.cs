using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BlogPostMap : EntityTypeConfiguration<BlogPost>
    {
        public BlogPostMap()
        {
            // Primary Key
            this.HasKey(t => t.bp_id);

            // Properties
            this.Property(t => t.bp_title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.bp_content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BlogPosts");
            this.Property(t => t.bp_id).HasColumnName("bp_id");
            this.Property(t => t.b_id).HasColumnName("b_id");
            this.Property(t => t.bp_title).HasColumnName("bp_title");
            this.Property(t => t.bp_content).HasColumnName("bp_content");
            this.Property(t => t.bp_dateposted).HasColumnName("bp_dateposted");
            this.Property(t => t.bp_reads).HasColumnName("bp_reads");
            this.Property(t => t.bp_approved).HasColumnName("bp_approved");

            // Relationships
            this.HasRequired(t => t.Blog)
                .WithMany(t => t.BlogPosts)
                .HasForeignKey(d => d.b_id).WillCascadeOnDelete(false);

        }
    }
}
