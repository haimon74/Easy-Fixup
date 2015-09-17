using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PaymentHistoryMap : EntityTypeConfiguration<PaymentHistory>
    {
        public PaymentHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ph_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ph_paymentprocessor)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ph_description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ph_notes)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("PaymentHistory");
            this.Property(t => t.ph_id).HasColumnName("ph_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ph_paymentprocessor).HasColumnName("ph_paymentprocessor");
            this.Property(t => t.ph_amount).HasColumnName("ph_amount");
            this.Property(t => t.ph_description).HasColumnName("ph_description");
            this.Property(t => t.ph_notes).HasColumnName("ph_notes");
            this.Property(t => t.ph_status).HasColumnName("ph_status");
            this.Property(t => t.ph_timestamp).HasColumnName("ph_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.PaymentHistories)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
