using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupPollsChoiceMap : EntityTypeConfiguration<GroupPollsChoice>
    {
        public GroupPollsChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.gpc_id);

            // Properties
            this.Property(t => t.gpc_answer)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("GroupPollsChoices");
            this.Property(t => t.gpc_id).HasColumnName("gpc_id");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.gpc_answer).HasColumnName("gpc_answer");

            // Relationships
            this.HasRequired(t => t.GroupTopic)
                .WithMany(t => t.GroupPollsChoices)
                .HasForeignKey(d => d.gt_id).WillCascadeOnDelete(false);

        }
    }
}
