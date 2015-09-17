using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ProfileChoiceMap : EntityTypeConfiguration<ProfileChoice>
    {
        public ProfileChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.pc_id);

            // Properties
            this.Property(t => t.pc_value)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("ProfileChoices");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.pq_id).HasColumnName("pq_id");
            this.Property(t => t.pc_value).HasColumnName("pc_value");

            // Relationships
            this.HasRequired(t => t.ProfileQuestion)
                .WithMany(t => t.ProfileChoices)
                .HasForeignKey(d => d.pq_id).WillCascadeOnDelete(false);

        }
    }
}
