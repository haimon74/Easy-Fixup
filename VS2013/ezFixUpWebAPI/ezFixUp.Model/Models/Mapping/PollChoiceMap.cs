using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PollChoiceMap : EntityTypeConfiguration<PollChoice>
    {
        public PollChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.pc_id);

            // Properties
            this.Property(t => t.pc_answer)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("PollChoices");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.pc_answer).HasColumnName("pc_answer");

            // Relationships
            this.HasRequired(t => t.Poll)
                .WithMany(t => t.PollChoices)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);

        }
    }
}
