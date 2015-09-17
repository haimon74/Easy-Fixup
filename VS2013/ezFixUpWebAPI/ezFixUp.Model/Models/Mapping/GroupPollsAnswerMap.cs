using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupPollsAnswerMap : EntityTypeConfiguration<GroupPollsAnswer>
    {
        public GroupPollsAnswerMap()
        {
            // Primary Key
            this.HasKey(t => new { t.gt_id, t.u_username });

            // Properties
            this.Property(t => t.gt_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("GroupPollsAnswers");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gpc_id).HasColumnName("gpc_id");

            // Relationships
            this.HasRequired(t => t.GroupPollsChoice)
                .WithMany(t => t.GroupPollsAnswers)
                .HasForeignKey(d => d.gpc_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.GroupTopic)
                .WithMany(t => t.GroupPollsAnswers)
                .HasForeignKey(d => d.gt_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupPollsAnswers)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
