using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ProfileAnswerMap : EntityTypeConfiguration<ProfileAnswer>
    {
        public ProfileAnswerMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.pq_id });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pq_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.pa_value)
                .IsRequired()
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("ProfileAnswers");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pq_id).HasColumnName("pq_id");
            this.Property(t => t.pa_value).HasColumnName("pa_value");
            this.Property(t => t.pa_approved).HasColumnName("pa_approved");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.ProfileAnswers)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.ProfileQuestion)
                .WithMany(t => t.ProfileAnswers)
                .HasForeignKey(d => d.pq_id).WillCascadeOnDelete(false);

        }
    }
}
