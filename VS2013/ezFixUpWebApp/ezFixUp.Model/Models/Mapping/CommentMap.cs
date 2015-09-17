using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.c_id);

            // Properties
            this.Property(t => t.c_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.c_to_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.c_comment_text)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Comments");
            this.Property(t => t.c_id).HasColumnName("c_id");
            this.Property(t => t.c_from_username).HasColumnName("c_from_username");
            this.Property(t => t.c_to_username).HasColumnName("c_to_username");
            this.Property(t => t.c_comment_text).HasColumnName("c_comment_text");
            this.Property(t => t.c_date_posted).HasColumnName("c_date_posted");
            this.Property(t => t.c_approved).HasColumnName("c_approved");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.c_from_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Comments1)
                .HasForeignKey(d => d.c_to_username).WillCascadeOnDelete(false);

        }
    }
}
