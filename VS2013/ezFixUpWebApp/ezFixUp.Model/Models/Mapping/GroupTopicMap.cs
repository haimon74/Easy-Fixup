using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupTopicMap : EntityTypeConfiguration<GroupTopic>
    {
        public GroupTopicMap()
        {
            // Primary Key
            this.HasKey(t => t.gt_id);

            // Properties
            this.Property(t => t.gt_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("GroupTopics");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.gt_name).HasColumnName("gt_name");
            this.Property(t => t.gt_datecreated).HasColumnName("gt_datecreated");
            this.Property(t => t.gt_dateupdated).HasColumnName("gt_dateupdated");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gt_posts).HasColumnName("gt_posts");
            this.Property(t => t.gt_locked).HasColumnName("gt_locked");
            this.Property(t => t.gt_stickyuntil).HasColumnName("gt_stickyuntil");
            this.Property(t => t.gt_poll).HasColumnName("gt_poll");
            this.Property(t => t.gt_views).HasColumnName("gt_views");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupTopics)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupTopics)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
