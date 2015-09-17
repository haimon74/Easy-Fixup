using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FAQMap : EntityTypeConfiguration<FAQ>
    {
        public FAQMap()
        {
            // Primary Key
            this.HasKey(t => t.q_id);

            // Properties
            this.Property(t => t.q_name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.q_question)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.q_answer)
                .IsRequired()
                .HasMaxLength(2500);

            // Table & Column Mappings
            this.ToTable("FAQs");
            this.Property(t => t.q_id).HasColumnName("q_id");
            this.Property(t => t.q_name).HasColumnName("q_name");
            this.Property(t => t.q_active).HasColumnName("q_active");
            this.Property(t => t.q_order).HasColumnName("q_order");
            this.Property(t => t.q_question).HasColumnName("q_question");
            this.Property(t => t.q_answer).HasColumnName("q_answer");
            this.Property(t => t.q_category_id).HasColumnName("q_category_id");
        }
    }
}
