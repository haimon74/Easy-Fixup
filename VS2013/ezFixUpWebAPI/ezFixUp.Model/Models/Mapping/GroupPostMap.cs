using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupPostMap : EntityTypeConfiguration<GroupPost>
    {
        public GroupPostMap()
        {
            // Primary Key
            this.HasKey(t => t.gp_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.gp_editnotes)
                .HasMaxLength(200);

            this.Property(t => t.gp_post)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("GroupPosts");
            this.Property(t => t.gp_id).HasColumnName("gp_id");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gp_dateposted).HasColumnName("gp_dateposted");
            this.Property(t => t.gp_dateedited).HasColumnName("gp_dateedited");
            this.Property(t => t.gp_editnotes).HasColumnName("gp_editnotes");
            this.Property(t => t.gp_post).HasColumnName("gp_post");

            // Relationships
            this.HasRequired(t => t.GroupTopic)
                .WithMany(t => t.GroupPosts)
                .HasForeignKey(d => d.gt_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupPosts)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
