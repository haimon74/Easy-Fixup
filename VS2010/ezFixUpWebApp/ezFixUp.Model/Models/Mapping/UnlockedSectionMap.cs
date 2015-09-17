using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class UnlockedSectionMap : EntityTypeConfiguration<UnlockedSection>
    {
        public UnlockedSectionMap()
        {
            // Primary Key
            this.HasKey(t => t.us_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.us_targetusername)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("UnlockedSections");
            this.Property(t => t.us_id).HasColumnName("us_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.us_type).HasColumnName("us_type");
            this.Property(t => t.us_targetusername).HasColumnName("us_targetusername");
            this.Property(t => t.us_targetid).HasColumnName("us_targetid");
            this.Property(t => t.us_unlockeduntil).HasColumnName("us_unlockeduntil");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.UnlockedSections)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.UnlockedSections1)
                .HasForeignKey(d => d.us_targetusername).WillCascadeOnDelete(false);

        }
    }
}
