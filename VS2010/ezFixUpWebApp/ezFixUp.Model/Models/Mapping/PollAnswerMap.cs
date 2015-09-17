using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PollAnswerMap : EntityTypeConfiguration<PollAnswer>
    {
        public PollAnswerMap()
        {
            // Primary Key
            this.HasKey(t => new { t.p_id, t.u_username });

            // Properties
            this.Property(t => t.p_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PollAnswers");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pc_id).HasColumnName("pc_id");

            // Relationships
            this.HasRequired(t => t.PollChoice)
                .WithMany(t => t.PollAnswers)
                .HasForeignKey(d => d.pc_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.PollAnswers)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
