using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BlogPostCommentMap : EntityTypeConfiguration<BlogPostComment>
    {
        public BlogPostCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.bpc_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.bpc_text)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BlogPostComments");
            this.Property(t => t.bpc_id).HasColumnName("bpc_id");
            this.Property(t => t.bp_id).HasColumnName("bp_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.bpc_text).HasColumnName("bpc_text");
            this.Property(t => t.bpc_dateposted).HasColumnName("bpc_dateposted");
            this.Property(t => t.bpc_approved).HasColumnName("bpc_approved");

            // Relationships
            this.HasRequired(t => t.BlogPost)
                .WithMany(t => t.BlogPostComments)
                .HasForeignKey(d => d.bp_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.BlogPostComments)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
