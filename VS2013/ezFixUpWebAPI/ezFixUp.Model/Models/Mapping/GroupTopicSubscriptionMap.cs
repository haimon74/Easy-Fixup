using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupTopicSubscriptionMap : EntityTypeConfiguration<GroupTopicSubscription>
    {
        public GroupTopicSubscriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.gts_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("GroupTopicSubscriptions");
            this.Property(t => t.gts_id).HasColumnName("gts_id");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gts_dateupdated).HasColumnName("gts_dateupdated");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupTopicSubscriptions)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.GroupTopic)
                .WithMany(t => t.GroupTopicSubscriptions)
                .HasForeignKey(d => d.gt_id);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupTopicSubscriptions)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
