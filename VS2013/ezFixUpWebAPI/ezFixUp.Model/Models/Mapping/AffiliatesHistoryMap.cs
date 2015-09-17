using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AffiliatesHistoryMap : EntityTypeConfiguration<AffiliatesHistory>
    {
        public AffiliatesHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ah_id);

            // Properties
            // Table & Column Mappings
            this.ToTable("AffiliatesHistory");
            this.Property(t => t.ah_id).HasColumnName("ah_id");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.ah_amount).HasColumnName("ah_amount");
            this.Property(t => t.ah_date_paid).HasColumnName("ah_date_paid");
            this.Property(t => t.ah_notes).HasColumnName("ah_notes");
            this.Property(t => t.ah_private_notes).HasColumnName("ah_private_notes");

            // Relationships
            this.HasRequired(t => t.Affiliate)
                .WithMany(t => t.AffiliatesHistories)
                .HasForeignKey(d => d.a_id);

        }
    }
}
