using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoCommentMap : EntityTypeConfiguration<PhotoComment>
    {
        public PhotoCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.pc_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pc_comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PhotoComments");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pc_comment).HasColumnName("pc_comment");
            this.Property(t => t.pc_date).HasColumnName("pc_date");

            // Relationships
            this.HasRequired(t => t.Photo)
                .WithMany(t => t.PhotoComments)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoComments)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
