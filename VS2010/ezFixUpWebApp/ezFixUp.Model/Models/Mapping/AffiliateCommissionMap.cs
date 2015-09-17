using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AffiliateCommissionMap : EntityTypeConfiguration<AffiliateCommission>
    {
        public AffiliateCommissionMap()
        {
            // Primary Key
            this.HasKey(t => t.ac_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ac_notes)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("AffiliateCommissions");
            this.Property(t => t.ac_id).HasColumnName("ac_id");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ph_id).HasColumnName("ph_id");
            this.Property(t => t.ac_timestamp).HasColumnName("ac_timestamp");
            this.Property(t => t.ac_notes).HasColumnName("ac_notes");
            this.Property(t => t.ac_amount).HasColumnName("ac_amount");

            // Relationships
            this.HasRequired(t => t.Affiliate)
                .WithMany(t => t.AffiliateCommissions)
                .HasForeignKey(d => d.a_id).WillCascadeOnDelete(false);
            this.HasOptional(t => t.PaymentHistory)
                .WithMany(t => t.AffiliateCommissions)
                .HasForeignKey(d => d.ph_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.AffiliateCommissions)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
