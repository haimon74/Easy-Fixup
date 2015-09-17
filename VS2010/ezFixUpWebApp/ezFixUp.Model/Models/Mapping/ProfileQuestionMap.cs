using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ProfileQuestionMap : EntityTypeConfiguration<ProfileQuestion>
    {
        public ProfileQuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.pq_id);

            // Properties
            this.Property(t => t.pq_name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.pq_altname)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.pq_description)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.pq_hint)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.pq_parent_question_choices)
                .HasMaxLength(3000);

            // Table & Column Mappings
            this.ToTable("ProfileQuestions");
            this.Property(t => t.pq_id).HasColumnName("pq_id");
            this.Property(t => t.pt_id).HasColumnName("pt_id");
            this.Property(t => t.pq_name).HasColumnName("pq_name");
            this.Property(t => t.pq_altname).HasColumnName("pq_altname");
            this.Property(t => t.pq_description).HasColumnName("pq_description");
            this.Property(t => t.pq_hint).HasColumnName("pq_hint");
            this.Property(t => t.pq_edit_style).HasColumnName("pq_edit_style");
            this.Property(t => t.pq_show_style).HasColumnName("pq_show_style");
            this.Property(t => t.pq_search_style).HasColumnName("pq_search_style");
            this.Property(t => t.pq_required).HasColumnName("pq_required");
            this.Property(t => t.pq_priority).HasColumnName("pq_priority");
            this.Property(t => t.pq_requires_approval).HasColumnName("pq_requires_approval");
            this.Property(t => t.pq_visible_male).HasColumnName("pq_visible_male");
            this.Property(t => t.pq_visible_female).HasColumnName("pq_visible_female");
            this.Property(t => t.pq_visible_couple).HasColumnName("pq_visible_couple");
            this.Property(t => t.pq_match_field).HasColumnName("pq_match_field");
            this.Property(t => t.pq_viewpaidonly).HasColumnName("pq_viewpaidonly");
            this.Property(t => t.pq_editpaidonly).HasColumnName("pq_editpaidonly");
            this.Property(t => t.pq_parent_question_id).HasColumnName("pq_parent_question_id");
            this.Property(t => t.pq_parent_question_choices).HasColumnName("pq_parent_question_choices");

            // Relationships
            this.HasRequired(t => t.ProfileTopic)
                .WithMany(t => t.ProfileQuestions)
                .HasForeignKey(d => d.pt_id).WillCascadeOnDelete(false);

        }
    }
}
