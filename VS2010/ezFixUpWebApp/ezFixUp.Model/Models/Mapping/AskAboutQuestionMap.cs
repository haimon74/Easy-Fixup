using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AskAboutQuestionMap : EntityTypeConfiguration<AskAboutQuestion>
    {
        public AskAboutQuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.q_id);

            // Properties
            this.Property(t => t.q_name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.q_display)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("AskAboutQuestions");
            this.Property(t => t.q_id).HasColumnName("q_id");
            this.Property(t => t.q_name).HasColumnName("q_name");
            this.Property(t => t.q_active).HasColumnName("q_active");
            this.Property(t => t.q_order).HasColumnName("q_order");
            this.Property(t => t.q_display).HasColumnName("q_display");
        }
    }
}
