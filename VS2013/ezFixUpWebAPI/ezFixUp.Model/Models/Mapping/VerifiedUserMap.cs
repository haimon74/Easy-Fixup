using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VerifiedUserMap : EntityTypeConfiguration<VerifiedUser>
    {
        public VerifiedUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.vu_verifiedby, t.vu_verifieduser });

            // Properties
            this.Property(t => t.vu_verifiedby)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.vu_verifieduser)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VerifiedUsers");
            this.Property(t => t.vu_verifiedby).HasColumnName("vu_verifiedby");
            this.Property(t => t.vu_verifieduser).HasColumnName("vu_verifieduser");
            this.Property(t => t.vu_timestamp).HasColumnName("vu_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.VerifiedUsers)
                .HasForeignKey(d => d.vu_verifiedby).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.VerifiedUsers1)
                .HasForeignKey(d => d.vu_verifieduser).WillCascadeOnDelete(false);

        }
    }
}
